using Plot.Metadata;

namespace Plot.Neo4j.Cypher
{
    public class CollectPropertyAsSnippet
    {
        private readonly PropertyMetadata _metadata;

        public CollectPropertyAsSnippet(PropertyMetadata metadata)
        {
            _metadata = metadata;
        }

        public override string ToString()
        {
            return $"collect({Extensions.CamelCase(_metadata.Name)}) AS {_metadata.Name}";
        }
    }
}
