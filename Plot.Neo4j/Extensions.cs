using System;
using System.Linq;
using Neo4jClient.Cypher;
using Plot.Neo4j.Cypher;
using Plot.Metadata;
using Plot.Neo4j.Queries;

namespace Plot.Neo4j
{
    public static class Extensions
    {
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

        public static ICypherFluentQuery Merge(this ICypherFluentQuery query, MatchNodeSnippet snippet)
        {
            return query.Merge(snippet.ToString());
        }

        public static ICypherFluentQuery Match(this ICypherFluentQuery query, MatchNodeSnippet snippet)
        {
            return query.Match(snippet.ToString());
        }

        public static ICypherFluentQuery Set(this ICypherFluentQuery query, SetSnippet snippet)
        {
            return query.Set(snippet.ToString());
        }

        public static ICypherFluentQuery With(this ICypherFluentQuery query, params WithSnippet[] snippets)
        {
            return query.With(string.Join(",", snippets.Select(x => x.ToString())));
        }

        public static ICypherFluentQuery WithParam(this ICypherFluentQuery query, ParamSnippet snippet, object value)
        {
            if (query.Query.QueryParameters.ContainsKey(snippet.ToString()))
            {
                return query;
            }

            return query.WithParam(snippet.ToString(), value);
        }

        public static ICypherFluentQuery CreateUnique(this ICypherFluentQuery query, CreateUniqueSnippet snippet)
        {
            return query.CreateUnique(snippet.ToString());
        }

        public static ICypherFluentQuery Match(this ICypherFluentQuery query, MatchRelationshipSnippet snippet)
        {
            return query.Match(snippet.ToString());
        }

        public static ICypherFluentQuery Delete(this ICypherFluentQuery query, params ParamSnippet[] snippets)
        {
            return query.Delete(string.Join(",", snippets.Select(x => x.ToString())));
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
            return metadata.Properties.Where(x => x.HasRelationship && !x.Relationship.Lazy).Aggregate(cypher, (current, property) => current.OptionalMatch($"(({CamelCase(metadata.Name)}){RelationshipSyntax.Create(property.Relationship)}({CamelCase(property.Name)}:{property.Type.Name}))"));
        }
    }
}
