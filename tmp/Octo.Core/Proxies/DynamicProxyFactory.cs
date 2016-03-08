using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using Octo.Core.Attributes;
using Octo.Core.Metadata;

namespace Octo.Core.Proxies
{
    public class DynamicProxyFactory : IProxyFactory
    {
        private readonly ProxyGenerator _generator;

        public DynamicProxyFactory()
        {
            _generator = new ProxyGenerator();
        }

        public T Create<T>(T item, IGraphSession session) where T : class
        {
            var generator = new Generator(_generator, session);
            return generator.Create(item);
        }

        private class Generator
        {
            private readonly ProxyGenerator _generator;

            private readonly IGraphSession _session;

            public Generator(ProxyGenerator generator, IGraphSession session)
            {
                _generator = generator;
                _session = session;
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
                var options = new ProxyGenerationOptions(new EntityStateTrackerProxyGenerationHook());
                var proxy = _generator.CreateClassProxyWithTarget(type, item, options, new EntityStateTrackerInterceptor());
                var state = GetState(proxy);
                _session.Register(proxy, state);
                PopulateEntity(proxy);
                state.Clean();
                return proxy;
            }

            public object CreateTrackableList<T>(object parent, IList<T> item, PropertyInfo propertyInfo) where T : class
            {
                var source = new List<T>();
                var metadata = (RelationshipAttribute) propertyInfo.GetCustomAttributes(true)[0];
                for (int i = 0; i < item.Count; i++)
                {
                    var child = (T) CreateTrackableEntity(item[i].GetType(), item[i]);
                    source.Add(child);
                }
                var relationship = new Relationship
                {
                    IsReverse = metadata.Reverse,
                    Name = metadata.Name
                };
                var proxy = new TrackableCollection<T>(parent, relationship, source);
                return proxy;
            }

            private void PopulateEntity(object item)
            {
                var type = EntityUtils.GetTargetEntity(item).GetType();
                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    var child = property.GetValue(item);
                    var childType = property.PropertyType;
                    if (IsPrimitive(childType))
                    {
                        continue;
                    }
                    var proxy = CreateProxy(childType, property, item, child);
                    property.SetValue(item, proxy);
                }
            }

            private object CreateProxy(Type type, PropertyInfo propertyInfo, object parent, object item)
            {
                if (IsList(type))
                {
                    var method = CreateGenericMethod(type);
                    return method.Invoke(this, new[] {parent, item, propertyInfo});
                }
                return CreateTrackableEntity(type, item);
            }

            private static MethodBase CreateGenericMethod(Type type)
            {
                var method = typeof (Generator).GetMethod("CreateTrackableList").MakeGenericMethod(type.GetGenericArguments()[0]);
                return method;
            }

            private static bool IsList(Type type)
            {
                return typeof (IEnumerable).IsAssignableFrom(type) && type.IsGenericType;
            }

            private static bool IsPrimitive(Type type)
            {
                return Primitives.Contains(type);
            }

            private static EntityState GetState(object proxy)
            {
                return EntityStateTracker.Contains(proxy) ? EntityStateTracker.Get(proxy) : EntityStateTracker.Create(proxy);
            }

            private static readonly Type[] Primitives =
            {
                typeof (int),
                typeof (decimal),
                typeof (string),
                typeof (DateTime),
                typeof (TimeSpan),
                typeof (double),
                typeof (uint),
                typeof (float),
                typeof (bool)
            };
        }
    }
}
