namespace Plot.Neo4j.Cypher
{
    public class PropertyIdentifierSnippet : NodeIdentifierSnippet
    {
        public PropertyIdentifierSnippet(NodeSnippet node, string propertyName = "id")
            : base(node.Metadata, node.Data, propertyName)
        {

        }
    }
}
