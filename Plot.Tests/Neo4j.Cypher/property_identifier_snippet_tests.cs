using Plot.Metadata;
using Plot.Neo4j.Cypher;
using Xunit;

namespace Plot.Tests.Neo4j.Cypher
{
    public class property_identifier_snippet_tests
    {
        [Fact]
        public void it_generates_a_node_with_variable_and_label()
        {
            var metadata = new NodeMetadata
            {
                Name = "Node"
            };
            var entity = new
            {
                Id = "1"
            };
            var nodeSnippet = new NodeSnippet(metadata, entity);
            var snippet = new PropertyIdentifierSnippet(nodeSnippet);
            var parameterName = snippet.ToString();
            Assert.Equal("Node_1_id", parameterName);
        }

        [Fact]
        public void it_generates_a_node_with_variable_and_label_when_property_name_supplied()
        {
            var metadata = new NodeMetadata
            {
                Name = "Node"
            };
            var entity = new
            {
                Id = "1"
            };
            var nodeSnippet = new NodeSnippet(metadata, entity);
            var snippet = new PropertyIdentifierSnippet(nodeSnippet, "name");
            var parameterName = snippet.ToString();
            Assert.Equal("Node_1_name", parameterName);
        }
    }
}
