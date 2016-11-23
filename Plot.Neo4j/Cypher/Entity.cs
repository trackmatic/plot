using Plot.Metadata;

namespace Plot.Neo4j.Cypher
{
    public class Entity
    {


        public Entity(NodeMetadata metadata, object value)
        {
            Metadata = metadata;
            Value = value;
        }

        public NodeMetadata Metadata { get; }
        public object Value { get; }

        public string Id => ProxyUtils.GetEntityId(Value);
    }
}
