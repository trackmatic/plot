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
                return (T)CreateTrackableEntity(typeof(T), item);
            }

            private object CreateTrackableEntity(Type type, object item)
            {
                if (_session.Uow.Contains(item))
                {
                    return item;
                }
                var interceptors = new IInterceptor[]
                {
                    new EntityStateInterceptor(_entityStateCache),
                    new RelationshipInterceptor(_metadataFactory, _entityStateCache)
                };
                var proxy = _generator.CreateClassProxyWithTarget(type, item, _options, interceptors);
                var state = GetState(proxy);
                _session.Register(proxy, state);
                PopulateEntity(proxy);
                state.Clean();
                return proxy;
            }

            public object CreateTrackableList<T>(NodeMetadata metadata, object parent, IList<T> item, PropertyInfo propertyInfo) where T : class
            {
                var source = new List<T>();
                for (int i = 0; i < item.Count; i++)
                {
                    var child = (T) CreateTrackableEntity(item[i].GetType(), item[i]);
                    source.Add(child);
                }
                var proxy = new TrackableCollection<T>(parent, metadata[propertyInfo.Name].Relationship, source, _entityStateCache);
                return proxy;
            }

            private void PopulateEntity(object item)
            {
                var type = ProxyUtils.GetTargetEntity(item).GetType();
                var metadata = _metadataFactory.Create(type);
                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    var child = property.GetValue(item);
                    var childType = property.PropertyType;
                    if (metadata[property.Name].IsPrimitive)
                    {
                        continue;
                    }
                    var proxy = CreateProxy(metadata, childType, property, item, child);
                    property.SetValue(item, proxy);
                }
            }

            private object CreateProxy(NodeMetadata metadata, Type type, PropertyInfo propertyInfo, object parent, object item)
            {
                if (item == null)
                {
                    return null;
                }
                var property = metadata[propertyInfo.Name];
                if (property.IsList)
                {
                    var method = CreateGenericMethod(type);
                    return method.Invoke(this, new[] { metadata, parent, item, propertyInfo});
                }
                return CreateTrackableEntity(type, item);
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
