using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Neo4j.Driver.V1;

namespace Plot.Neo4j
{
    public static class Extensions
    {
        private static bool IsEmpty(IRecord record, string key)
        {
            return !record.Keys.Contains(key) || record[key] == null;
        }

        public static List<T> ReadList<T>(this IRecord record, string key, Func<INode, T> factory)
        {
            key = Conventions.NamedParameterCase(key);
            if (IsEmpty(record, key))
            {
                return new List<T>();
            }
            return record[key].As<List<INode>>().Select(factory).ToList();
        }

        public static T Read<T>(this IRecord record, string key)
        {
            key = Conventions.NamedParameterCase(key);
            if (IsEmpty(record, key))
            {
                return default(T);
            }
            return record[key].As<T>();
        }

        public static T Read<T>(this IRecord record, string key, Func<INode,T> factory)
        {
            key = Conventions.NamedParameterCase(key);
            if (!record.Keys.Contains(key) || record[key] == null)
            {
                return default(T);
            }
            var node = record[key].As<INode>();
            return factory(node);
        }

        public static T Read<T>(this INode node, string key)
        {
            if (!node.Properties.ContainsKey(key))
            {
                return default(T);
            }
            return node[key].As<T>();
        }

        private static class PropertiesCache
        {
            private static readonly Dictionary<Type, IEnumerable<PropertyInfo>> Cache = new Dictionary<Type, IEnumerable<PropertyInfo>>();
            
            public static IEnumerable<PropertyInfo> GetProperties(object item)
            {
                if (item == null)
                {
                    return new List<PropertyInfo>();
                }

                lock (Cache)
                {
                    var key = item.GetType();
                    return Cache.ContainsKey(key) ? Cache[key] : Populate(key, item);
                }
            }

            private static IEnumerable<PropertyInfo> Populate(Type key, object item)
            {
                lock (Cache)
                {
                    var properties = item.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    Cache[key] = properties;
                    return properties;
                }
            }
        }

        public static IDictionary<string, object> ToDictionary(this object item)
        {
            return PropertiesCache.GetProperties(item).ToDictionary(prop => prop.Name, prop => prop.GetValue(item, null));
        }

        public static long ToUnixTimestamp(this DateTime datetime)
        {
            var unixRef = new DateTime(1970, 1, 1, 0, 0, 0);
            return (datetime.Ticks - unixRef.Ticks) / 10000000;
        }

        public static DateTime FromUnixTimestamp(this long timestamp)
        {
            var unixRef = new DateTime(1970, 1, 1, 0, 0, 0);
            return unixRef.AddSeconds(timestamp);
        }
    }
}
