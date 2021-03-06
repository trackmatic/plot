﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;
using Plot.Exceptions;
using Plot.Metadata;
using Plot.Proxies;

namespace Plot
{
    public static class ProxyUtils
    {
        public static string GetEntityId(object source)
        {
            var property = EntityIdUtils.GetId(source);
            var id = property.GetValue(source);
            if (id == null)
            {
                throw new PropertyNotSetException(Text.IdNull, source);
            }
            return id.ToString();
        }
        
        public static bool IsProxy(object source)
        {
            return source is IProxyTargetAccessor;
        }

        public static Type GetTargetEntityType(object source)
        {
            var proxy = source as IProxyTargetAccessor;
            if (proxy == null)
            {
                return source.GetType();
            }
            return proxy.DynProxyGetTarget().GetType().BaseType;
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
            var trackable = list as ITrackable;
            if (trackable == null)
            {
                throw new TrackableCollectionException(Text.FlushTrackableCollectionException);
            }
            return trackable.Flush();
        }

        public static IEnumerable<ITrackableRelationship> Flush(object item, RelationshipMetadata relationship)
        {
            var source = item as IProxyTargetAccessor;
            if (source == null)
            {
                throw new TrackableRelationshipException(Text.FlushTrackablerelationshipException);
            }
            var interceptors = GetInterceptors<RelationshipInterceptor>(source);
            return interceptors.Where(x => x.Contains(relationship)).Select(x => x.GetTrackableRelationship(relationship));
        }

        public static bool IsTrackable(IEnumerable list)
        {
            return list is ITrackableCollection;
        }

        public static IInterceptor[] GetInterceptors(object source)
        {
            return ((IProxyTargetAccessor) source).GetInterceptors();
        }

        public static IEnumerable<T> GetInterceptors<T>(IProxyTargetAccessor item)
            where T : IInterceptor
        {
            return item.GetInterceptors().Where(x => x is T).Cast<T>();
        }
    }
}
