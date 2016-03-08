using System;
using System.Collections;
using Castle.DynamicProxy;

namespace Plot
{
    public static class ProxyUtils
    {
        public static string GetEntityId(object source)
        {
            var type = source.GetType();
            var property = type.GetProperty("Id");
            if (property == null)
            {
                throw new InvalidOperationException("Entities must have an id property of type string");
            }
            var id = property.GetValue(source);
            if (id == null)
            {
                throw new InvalidOperationException($"Id of {source} cannot be null");
            }
            return id.ToString();
        }
        
        public static bool IsProxy(object source)
        {
            return source is IProxyTargetAccessor;
        }

        public static void MakeReadonly(object source)
        {
            EntityStateTracker.Get(source).Readonly();
        }

        public static EntityState GetState(object source)
        {
            return EntityStateTracker.Get(source);
        }

        public static object GetTargetEntity(object source)
        {
            var proxy = source as IProxyTargetAccessor;
            if (proxy == null)
            {
                return source;
            }
            return proxy.DynProxyGetTarget();
        }

        public static Type GetTargetType(object source)
        {
            var proxy = source as IProxyTargetAccessor;
            if (proxy == null)
            {
                return source.GetType();
            }
            return proxy.GetType().BaseType;
        }

        public static IEnumerable Flush(IEnumerable list)
        {
            var trackable = list as ITrackableCollection;
            if (trackable == null)
            {
                throw new InvalidOperationException("Collection is not trackable and cannot be flushed");
            }
            return trackable.Flush();
        }

        public static bool IsTrackable(IEnumerable list)
        {
            return list is ITrackableCollection;
        }
    }
}
