using Plot.Metadata;

namespace Plot.N4j.Cypher
{
    public class NodeIdSnippet : NodeSnippet
    {
        public NodeIdSnippet(NodeSnippet node)
            : base(node.Metadata, node.Data, new [] { "id" })
        {

        }

        public NodeIdSnippet(NodeMetadata metadata, object model, string label)
            : base(new ParamSnippet(metadata, model), label, new[] { "id" })
        {

        }
    }
}
