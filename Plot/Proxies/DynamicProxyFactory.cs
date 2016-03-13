using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Plot.Metadata;

namespace Plot.Proxies
{
    public class DynamicProxyFactory : IProxyFactory
    {
        private readonly IMetadataFactory _metadataFactory;

        public DynamicProxyFactory(IMetadataFactory metadataFactory)
        {
            _metadataFactory = metadataFactory;
        }

        public T Create<T>(T item, IGraphSession session, EntityStatus status = EntityStatus.Clean) where T : class
        {
            var generator = new Generator(session, _metadataFactory, status);
            return generator.Create(item);
        }

        private class Generator
        {
            private readonly ProxyGenerator _generator;

            private readonly IGraphSession _session;

            private readonly IMetadataFactory _metadataFactory;

            private readonly ProxyGenerationOptions _options;

            private readonly IEntityStateCache _state;

            private readonly EntityStatus _status;

            public Generator(IGraphSession session, IMetadataFactory metadataFactory, EntityStatus status = EntityStatus.Clean)
            {
                _generator = new ProxyGenerator();
                _session = session;
                _metadataFactory = metadataFactory;
                _options = new ProxyGenerationOptions(new ProxyGenerationHook());
                _state = session.State;
                _status = status;
            }

            public T Create<T>(T item)
            {
                var proxy = (T)Create(typeof(T), item);
                if (proxy is IRequireSession)
                {
                    ((IRequireSession) proxy).Set(_session);
                }
                return proxy;
            }

            private object Create(Type type, object item)
            {
                var interceptors = new IInterceptor[]
                {
                    new EntityStateInterceptor(_state),
                    new RelationshipInterceptor(_metadataFactory, _state)
                };
                ProxyUtils.SetEntityId(item);
                var proxy = _generator.CreateClassProxy(type, _options, interceptors);
                Map(item, proxy);
                var state = GetState(proxy);
                _session.Register(proxy, state);
                Populate(proxy);
                state.Set(_status);
                return proxy;
            }

            private void Map(object source, object proxy)
            {
                foreach (var property in ProxyUtils.GetTargetEntityType(source).GetProperties())
                {
                    if (property.SetMethod == null)
                    {
                        continue;
                    }
                    var value = property.GetMethod.Invoke(source, null);
                    if (value == null)
                    {
                        continue;
                    }
                    if (ProxyUtils.IsProxy(value))
                    {
                        continue;
                    }
                    property.SetMethod.Invoke(proxy, new[] {value});
                }
            }

            private void Populate(object item)
            {
                var type = ProxyUtils.GetTargetType(item);
                var metadata = _metadataFactory.Create(type);
                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    var propertyMetadata = metadata[property.Name];
                    if (propertyMetadata.IsPrimitive)
                    {
                        continue;
                    }
                    if (propertyMetadata.IsIgnored)
                    {
                        continue;
                    }
                    var proxy = Factory(propertyMetadata)(metadata, property, item);
                    property.SetValue(item, proxy);
                }
            }

            private Func<NodeMetadata, PropertyInfo, object, object> Factory(PropertyMetadata property)
            {
                if (property.IsList)
                {
                    return List;
                }
                return Relationship;
            }

            private object List(NodeMetadata metadata, PropertyInfo property, object parent)
            {
                var type = property.PropertyType;
                var source = property.GetValue(parent);
                if (source == null)
                {
                    return null;
                }
                var genericType = typeof (TrackableCollection<>).MakeGenericType(type.GenericTypeArguments[0]);
                var items = Populate(((IEnumerable<object>)source).ToList());
                var args = new[] { parent, metadata[property.Name].Relationship, items, _state};
                var proxy = Activator.CreateInstance(genericType, args, null);
                return proxy;
            }

            private object Relationship(NodeMetadata metadata, PropertyInfo property, object parent)
            {
                var type = property.PropertyType;
                var item = property.GetValue(parent);
                if (item == null)
                {
                    return null;
                }
                var entity = _session.Uow.Get(ProxyUtils.GetEntityId(item), type) ?? item;
                
                return Create(type, entity);
            }

            private List<object> Populate(IList<object> source)
            {
                var destination = new List<object>();
                for (int i = 0; i < source.Count; i++)
                {
                    var type = ProxyUtils.GetTargetType(source[i]);
                    var existing = _session.Uow.Get(ProxyUtils.GetEntityId(source[i]), type);
                    var child = existing ?? Create(type, source[i]);
                    destination.Add(child);
                }
                return destination;
            }

            private static MethodBase CreateGenericMethod(Type type)
            {
                var method = typeof (Generator).GetMethod("CreateTrackableList").MakeGenericMethod(type.GetGenericArguments()[0]);
                return method;
            }

            private EntityState GetState(object proxy)
            {
                return _state.Contains(proxy) ? _state.Get(proxy) : _state.Create(proxy);
            }
        }
    }
}
