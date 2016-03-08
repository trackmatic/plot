using System;
using System.Collections.Generic;
using Castle.DynamicProxy;

namespace Octo.Core
{
    public static class EntityUtils
    {
        public static string GetEntityId(object source)
        {
            var type = source.GetType();
            var propertry = type.GetProperty("Id");
            var id = propertry.GetValue(source);
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

        public static IEnumerable<T> Flush<T>(IList<T> list)
        {
            var trackable = list as ITrackableCollection<T>;
            if (trackable == null)
            {
                return new List<T>();
            }
            return trackable.Flush();
        }
    }
}
