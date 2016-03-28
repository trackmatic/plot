using Plot.Metadata;

namespace Plot.Neo4j.Cypher
{
    public class AsSnippet
    {
        private readonly NodeMetadata _metadata;

        public AsSnippet(NodeMetadata metadata)
        {
            _metadata = metadata;
        }

        public override string ToString()
        {
            return $"{Extensions.CamelCase(_metadata.Name)} AS {_metadata.Name}";
        }
    }
}
