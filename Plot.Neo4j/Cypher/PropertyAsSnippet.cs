using Plot.Metadata;

namespace Plot.Neo4j.Cypher
{
    public class PropertyAsSnippet
    {
        private readonly PropertyMetadata _metadata;

        public PropertyAsSnippet(PropertyMetadata metadata)
        {
            _metadata = metadata;
        }

        public override string ToString()
        {
            return $"{Extensions.CamelCase(_metadata.Name)} AS {_metadata.Name}";
        }
    }
}
