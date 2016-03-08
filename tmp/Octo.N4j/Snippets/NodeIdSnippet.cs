using Octo.Core;

namespace Octo.N4j.Snippets
{
    public class NodeIdSnippet<TSource> : NodeSnippet<TSource>
    {
        public NodeIdSnippet(NodeSnippet<TSource> node)
            : base(node.Data, new [] { "id" })
        {

        }

        public NodeIdSnippet(TSource model, string label)
            : base(model, new[] { "id" })
        {

        }
    }
}
