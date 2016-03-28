using Plot.Metadata;

namespace Plot.Neo4j.Cypher
{
    public class NodeIdentifierSnippet : IdentifierNameSnippet
    {
        private readonly NodeMetadata _metadata;

        public NodeIdentifierSnippet(NodeMetadata metadata, object item, params string[] segments) 
            : this(metadata, item)
        {
            foreach (var segment in segments)
            {
                Add(segment);
            }
        }

        public NodeIdentifierSnippet(NodeMetadata metadata, object item)
        {
            Add(metadata.Name);
            Add(ProxyUtils.GetEntityId(item));
        }

        public NodeMetadata Metadata => _metadata;
    }
}
