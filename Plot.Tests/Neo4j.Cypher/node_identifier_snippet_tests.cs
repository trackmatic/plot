using Plot.Metadata;
using Plot.Neo4j.Cypher;
using Xunit;

namespace Plot.Tests.Neo4j.Cypher
{
    public class node_identifier_snippet_tests
    {
        [Fact]
        public void it_generates_a_valid_identifier_name()
        {
            var metadata = new NodeMetadata
            {
                Name = "Node"
            };
            var entity = new
            {
                Id = "1"
            };
            var snippet = new NodeIdentifierSnippet(metadata, entity);
            var parameterName = snippet.ToString();
            Assert.Equal("Node_1", parameterName);
        }
    }
}