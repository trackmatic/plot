using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plot.Metadata;

namespace Plot.Neo4j.Cypher
{
    public class NodeSnippet
    {
        private readonly IdentifierNameSnippet _identifierName;

        public NodeSnippet(NodeMetadata metadata, object data) 
            : this(metadata, data, new List<string>())
        {

        }

        protected NodeSnippet(NodeMetadata metadata, object data, IEnumerable<string> segments)
        {
            _identifierName = new NodeIdentifierSnippet(metadata, data, segments.ToArray());
            Data = data;
            Metadata = metadata;
            Data = data;
        }
        
        public override string ToString()
        {
            return new StringBuilder()
                .Append(_identifierName)
                .Append(":")
                .Append(Metadata.Name)
                .ToString();
        }

        public IdentifierNameSnippet IdentifierName => _identifierName;
        
        public object Data { get; }

        public NodeMetadata Metadata { get; }
    }
}
