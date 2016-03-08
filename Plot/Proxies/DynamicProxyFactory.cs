using System;
using System.Collections.Generic;
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

        public T Create<T>(T item, IGraphSession session, IEntityStateCache entityStateCache) where T : class
        {
            var generator = new Generator(session, _metadataFactory, entityStateCache);
            return generator.Create(item);
        }

        private class Generator
        {
            private readonly ProxyGenerator _generator;

            private readonly IGraphSession _session;

            private readonly IMetadataFactory _metadataFactory;

            private readonly ProxyGenerationOptions _options;

            private readonly IEntityStateCache _entityStateCache;

            public Generator(IGraphSession session, IMetadataFactory metadataFactory, IEntityStateCache entityStateCache)
            {
                _generator = new ProxyGenerator();
                _session = session;
                _metadataFactory = metadataFactory;
                _options = new ProxyGenerationOptions(new ProxyGenerationHook());
                _entityStateCache = entityStateCache;
            }

            public T Create<T>(T item)
            {
                return (T)Create(typeof(T), item);
            }

            private object Create(Type type, object item)
            {
                var interceptors = new IInterceptor[]
                {
                    new EntityStateInterceptor(_entityStateCache),
                    new RelationshipInterceptor(_metadataFactory, _entityStateCache)
                };
                var proxy = _generator.CreateClassProxyWithTarget(type, item, _options, interceptors);
                var state = GetState(proxy);
                _session.Register(proxy, state);
                Populate(proxy);
                state.Clean();
                return proxy;
            }

            public object CreateTrackableList<T>(NodeMetadata metadata, object parent, IList<T> item, PropertyInfo propertyInfo) where T : class
            {
                var source = new List<T>();
                for (int i = 0; i < item.Count; i++)
                {
                    var type = ProxyUtils.GetTargetType(item[i]);
                    var existing = (T)_session.Uow.Get(ProxyUtils.GetEntityId(item[i]), type);
                    var child = existing ?? (T) Create(type, item[i]);
                    source.Add(child);
                }
                var proxy = new TrackableCollection<T>(parent, metadata[propertyInfo.Name].Relationship, source, _entityStateCache);
                return proxy;
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
                var item = property.GetValue(parent);
                if (item == null)
                {
                    return null;
                }
                var method = CreateGenericMethod(type);
                return method.Invoke(this, new[] { metadata, parent, item, property });
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

            private static MethodBase CreateGenericMethod(Type type)
            {
                var method = typeof (Generator).GetMethod("CreateTrackableList").MakeGenericMethod(type.GetGenericArguments()[0]);
                return method;
            }

            private EntityState GetState(object proxy)
            {
                return _entityStateCache.Contains(proxy) ? _entityStateCache.Get(proxy) : _entityStateCache.Create(proxy);
            }
        }
    }
}
