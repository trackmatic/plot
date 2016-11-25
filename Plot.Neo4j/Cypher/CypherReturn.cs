using System;
using System.Collections.Generic;
using System.Text;
using Neo4j.Driver.V1;
using Plot.Neo4j.Exceptions;

namespace Plot.Neo4j.Cypher
{
    public class CypherReturn<TResult> : ICypherReturn<TResult>
    {
        private readonly StringBuilder _builder;
        private readonly Dictionary<string, Return> _items;
        private readonly Func<IRecord, TResult> _map;

        public CypherReturn(Func<IRecord, TResult> map, StringBuilder builder)
        {
            _map = map;
            _builder = builder;
            _items = new Dictionary<string, Return>();
        }

        public ICypherReturn<TResult> Collect(Return item)
        {
            GuardAgainstDuplicateKeys(item);
            Append(Keywords.Collect, Conventions.NamedParameterCase(item.Property), Conventions.NamedParameterCase(item.Alias));
            _items.Add(item.Property, item);
            return this;
        }

        public ICypherReturn<TResult> CollectDistinct(Return item)
        {
            GuardAgainstDuplicateKeys(item);
            Append(Keywords.CollectDistinct, Conventions.NamedParameterCase(item.Property), Conventions.NamedParameterCase(item.Alias));
            _items.Add(item.Property, item);
            return this;
        }

        public ICypherReturn<TResult> Return(Return item)
        {
            GuardAgainstDuplicateKeys(item);
            Append(Keywords.Return, Conventions.NamedParameterCase(item.Property), Conventions.NamedParameterCase(item.Alias));
            _items.Add(item.Property, item);
            return this;
        }

        public ICypherReturn<TResult> Collect(string property, string alias = null)
        {
            return Collect(new Return(property, alias ?? property));
        }

        public ICypherReturn<TResult> CollectDistinct(string property, string alias = null)
        {
            return CollectDistinct(new Return(property, alias ?? property));
        }

        public ICypherReturn<TResult> Return(string property, string alias = null)
        {
            return Return(new Return(property, alias ?? property));
        }

        public TResult Map(IRecord record)
        {
            return _map(record);
        }

        private void Append(string format, params object[] parameters)
        {
            _builder.Append(_items.Count > 0 ? ", " : " ").AppendFormat(format, parameters);
        }

        private void GuardAgainstDuplicateKeys(Return item)
        {
            if (!_items.ContainsKey(item.Property))
            {
                return;
            }

            throw new DuplicateReturnStatementException(item);
        }

        private static class Keywords
        {
            public const string Return = "{0} AS {1}";
            public const string Collect = "COLLECT({0}) AS {1}";
            public const string CollectDistinct = "COLLECT(DISTINCT {0}) AS {1}";
        }
    }
}
