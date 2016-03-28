using Plot.Metadata;
using Plot.Neo4j.Cypher;
using Xunit;

namespace Plot.Tests.Neo4j.Cypher
{
    public class set_identifier_snippet_tests
    {
        [Fact]
        public void it_generates_node_data_snippet()
        {
            var metadata = new NodeMetadata
            {
                Name = "Node"
            };
            var entity = new
            {
                Id = "id"
            };
            var identifierName = new NodeIdentifierSnippet(metadata, entity);
            var snippet = new SetIdentifierSnippet(identifierName);
            var text = snippet.ToString();
            Assert.Equal("Node_id = {Node_id}", text);
        }
    }
}
