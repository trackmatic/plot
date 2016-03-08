using System;
using System.Linq;
using Neo4jClient.Cypher;
using Octo.N4j.Snippets;

namespace Octo.N4j
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
    }
}
