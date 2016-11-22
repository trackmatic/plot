using System;
using System.Collections.Generic;
using System.Text;
using Neo4j.Driver.V1;

namespace Plot.Neo4j.Cypher
{
    public class CypherFluentQuery<T> : ICypherFluentQuery<T>
    {
        private readonly StringBuilder _builder;
        private ICypherReturn<T> _return;
           
        public CypherFluentQuery() : this(new StringBuilder(), null, new Dictionary<string, object>())
        {
            
        }

        private CypherFluentQuery(StringBuilder builder, ICypherReturn<T> @return, IDictionary<string, object> parameters)
        {
            _builder = builder;
            _return = @return;
            Parameters = parameters;
        }

        public string Statement => _builder.ToString().Trim('\r').Trim('\n');
        public IDictionary<string, object> Parameters { get; }

        public string GetDebugText()
        {
            return _builder.ToString();
        }

        public ICypherFluentQuery Match(string statement)
        {
            return Mutate(Append(Keywords.Match, statement));
        }

        public ICypherFluentQuery Set(string statement)
        {
            return Mutate(Append(Keywords.Set, statement));
        }

        public ICypherFluentQuery Merge(string statement)
        {
            return Mutate(Append(Keywords.Merge, statement));
        }

        public ICypherFluentQuery With(string statement)
        {
            return Mutate(Append(Keywords.With, statement));
        }

        public ICypherFluentQuery CreateUnique(string statement)
        {
            return Mutate(Append(Keywords.CreateUnique, statement));
        }

        public ICypherFluentQuery Delete(string statement)
        {
            return Mutate(Append(Keywords.Delete, statement));
        }

        public ICypherFluentQuery Where(string statement)
        {
            return Mutate(Append(Keywords.Where, statement));
        }

        public ICypherFluentQuery OptionalMatch(string statement)
        {
            return Mutate(Append(Keywords.OptionalMatch, statement));
        }

        public ICypherFluentQuery WithParam(string key, object value)
        {
            Parameters.Add(key, value);
            return new CypherFluentQuery<T>(_builder, _return, Parameters);
        }

        public ICypherFluentQuery OnCreate()
        {
            return Mutate(Append(Keywords.OnCreate));
        }

        public ICypherFluentQuery OnMatch()
        {
            return Mutate(Append(Keywords.OnMatch));
        }

        public ICypherFluentQuery<T> Return(Func<IRecord, T> map, Func<ICypherReturn<T>, ICypherReturn<T>> factory)
        {
            Append(Keywords.Return);
            _return = factory(new CypherReturn<T>(map, _builder));
            return this;
        }

        public ICypherFluentQuery<T> ReturnDistinct(Func<IRecord, T> map, Func<ICypherReturn<T>, ICypherReturn<T>> factory)
        {
            Append(Keywords.ReturnDistinct);
            _return = factory(new CypherReturn<T>(map, _builder));
            return this;
        }

        public T Map(IRecord record)
        {
            return _return.Map(record);
        }
        

        public bool ContainsParameter(string key)
        {
            return Parameters.ContainsKey(key);
        }

        public ICypherFluentQuery<T> Skip(int count)
        {
            return Mutate(Append(Keywords.Skip, count));
        }

        public ICypherFluentQuery<T> Limit(int count)
        {
            return Mutate(Append(Keywords.Limit, count));
        }

        public ICypherFluentQuery<T> OrderByDescending(string property)
        {
            return Mutate(Append(string.Format(Keywords.OrderByDescending, property)));
        }

        public ICypherFluentQuery<T> OrderBy(string property)
        {
            return Mutate(Append(Keywords.OrderBy, property));
        }

        private StringBuilder Append(params object[] items)
        {
            return _builder.AppendLine().Append(string.Join(" ", items));
        }

        private CypherFluentQuery<T> Mutate(StringBuilder builder)
        {
            return new CypherFluentQuery<T>(builder, _return, Parameters);
        }

        private static class Keywords
        {
            public const string With = "WITH";
            public const string Match = "MATCH";
            public const string OptionalMatch = "OPTIONAL MATCH";
            public const string OnCreate = "ON CREATE";
            public const string OnMatch = "ON MATCH";
            public const string Merge = "MERGE";
            public const string Delete = "DELETE";
            public const string Where = "WHERE";
            public const string Limit = "LIMIT";
            public const string Skip = "SKIP";
            public const string OrderBy = "ORDER BY";
            public const string OrderByDescending = "ORDER BY {0} DESC";
            public const string CreateUnique = "CREATE UNIQUE";
            public const string Set = "SET";
            public const string Return = "RETURN";
            public const string ReturnDistinct = "RETURN DISTINCT";
        }
    }
}
