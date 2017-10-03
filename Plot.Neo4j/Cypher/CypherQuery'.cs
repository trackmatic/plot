using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Neo4j.Driver.V1;
using Plot.Metadata;

namespace Plot.Neo4j.Cypher
{
    public class CypherQuery<T> : ICypherQuery<T>
    {
        private readonly StringBuilder _builder;
        private ICypherReturn<T> _return;
           
        public CypherQuery() : this(new StringBuilder(), null, new Dictionary<string, object>())
        {
            
        }

        private CypherQuery(StringBuilder builder, ICypherReturn<T> @return, IDictionary<string, object> parameters)
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

        public ICypherQuery Match(string statement)
        {
            return Mutate(Append(Keywords.Match, statement));
        }

        public ICypherQuery Set(string statement)
        {
            return Mutate(Append(Keywords.Set, statement));
        }

        public ICypherQuery Merge(string statement)
        {
            return Mutate(Append(Keywords.Merge, statement));
        }

        public ICypherQuery With(string statement)
        {
            return Mutate(Append(Keywords.With, statement));
        }

        public ICypherQuery CreateUnique(string statement)
        {
            return Mutate(Append(Keywords.CreateUnique, statement));
        }

        public ICypherQuery Delete(string statement)
        {
            return Mutate(Append(Keywords.Delete, statement));
        }

        public ICypherQuery DetachDelete(string statement)
        {
            return Mutate(Append(Keywords.DetachDelete, statement));
        }

        public ICypherQuery Where(string statement)
        {
            return Mutate(Append(Keywords.Where, statement));
        }

        public ICypherQuery OptionalMatch(string statement)
        {
            return Mutate(Append(Keywords.OptionalMatch, statement));
        }

        public ICypherQuery WithParam(string key, object value)
        {
            if (Parameters.ContainsKey(key))
            {
                Parameters[key] = value;
            }
            else
            {
                Parameters.Add(key, value);
            }

            return Mutate(_builder);
        }

        public ICypherQuery RemoveParam(string key)
        {
            if (!Parameters.ContainsKey(key))
            {
                return this;
            }
            Parameters.Remove(key);
            return this;
        }

        public ICypherQuery OnCreate()
        {
            return Mutate(Append(Keywords.OnCreate));
        }

        public ICypherQuery OnMatch()
        {
            return Mutate(Append(Keywords.OnMatch));
        }

        public ICypherQuery<T> Return(Func<IRecord, T> map, Func<ICypherReturn<T>, ICypherReturn<T>> factory)
        {
            Append(Keywords.Return);
            _return = factory(new CypherReturn<T>(map, _builder));
            return this;
        }

        public ICypherQuery<T> ReturnDistinct(Func<IRecord, T> map, Func<ICypherReturn<T>, ICypherReturn<T>> factory)
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

        public ICypherQuery<T1> AsTypedQuery<T1>()
        {
            return (ICypherQuery<T1>)this;
        }

        public ICypherQuery IncludeRelationships(NodeMetadata metadata)
        {
            return metadata
                .Properties
                .Where(x => x.HasRelationship && !x.Relationship.Lazy)
                .OrderByDescending(x => x.Relationship.NotNull)
                .Aggregate((ICypherQuery)this, (current, property) => IncludeRelationship(current, property, metadata));
        }

        public ICypherQuery<T> Skip(int count)
        {
            return Mutate(Append(Keywords.Skip, count));
        }

        public ICypherQuery<T> Limit(int count)
        {
            return Mutate(Append(Keywords.Limit, count));
        }

        public ICypherQuery<T> OrderByDescending(string property)
        {
            return Mutate(Append(string.Format(Keywords.OrderByDescending, property)));
        }

        public ICypherQuery<T> OrderBy(string property)
        {
            return Mutate(Append(Keywords.OrderBy, property));
        }

        private ICypherQuery IncludeRelationship(ICypherQuery current, PropertyMetadata property, NodeMetadata metadata)
        {
            var statement = $"(({Conventions.NamedParameterCase(metadata.Name)}){StatementFactory.Relationship(property.Relationship)}({Conventions.NamedParameterCase(property.Name)}:{property.Type.Name}))";
            return property.Relationship.NotNull ? current.Match(statement) : current.OptionalMatch(statement);
        }

        private StringBuilder Append(params object[] items)
        {
            return _builder.AppendLine().Append(string.Join(" ", items));
        }

        private CypherQuery<T> Mutate(StringBuilder builder)
        {
            return new CypherQuery<T>(builder, _return, Parameters);
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
            public const string DetachDelete = "DETACH DELETE";
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
