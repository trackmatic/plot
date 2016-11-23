using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Neo4j.Driver.V1;
using Plot.Neo4j.Cypher;
using Plot.Metadata;

namespace Plot.Neo4j
{
    public static class Extensions
    {
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

        public static string CamelCase(string value)
        {
            if (char.IsUpper(value[0]))
            {
                var start = new[] { char.ToLower(value[0]) };
                var remainder = value.Skip(1).Take(value.Length).ToArray();
                return new string(start.Concat(remainder).ToArray());
            }
            return value;
        }

        public static ICypherFluentQuery MatchById(this ICypherFluentQuery cypher, NodeMetadata metadata)
        {
            return cypher.Match($"({CamelCase(metadata.Name)}:{metadata.Name})").Where($"{CamelCase(metadata.Name)}.Id in {{id}}");
        }

        public static ICypherFluentQuery IncludeRelationships(this ICypherFluentQuery cypher, NodeMetadata metadata)
        {
            return metadata.Properties.Where(x => x.HasRelationship && !x.Relationship.Lazy).Aggregate(cypher, (current, property) => current.OptionalMatch($"(({CamelCase(metadata.Name)}){StatementFactory.Relationship(property.Relationship)}({CamelCase(property.Name)}:{property.Type.Name}))"));
        }
    }
}
