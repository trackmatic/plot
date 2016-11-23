using System;
using Plot.Neo4j.Cypher;

namespace Plot.Sample.Data.Params
{
    public class Parameter
    {
        private readonly string _name;

        private readonly Func<bool> _match;

        private readonly Func<object> _value;

        public Parameter(string name, Func<object> value, Func<bool> match)
        {
            _name = name;
            _match = match;
            _value = value;
        }

        public Parameter(string name, Func<object> value)
            : this(name, value, () => true)
        {

        }

        public ICypherQuery Append(ICypherQuery query)
        {
            if (!_match())
            {
                return query;
            }

            return query.WithParam(_name, _value());
        }
    }
}
