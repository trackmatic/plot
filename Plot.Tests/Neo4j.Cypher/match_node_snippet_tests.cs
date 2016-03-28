using Plot.Metadata;
using Plot.Neo4j.Cypher;
using Xunit;

namespace Plot.Tests.Neo4j.Cypher
{
    public class match_node_snippet_tests
    {
        [Fact]
        public void it_generates_valid_cypher()
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
            var identifierNameSnippet = new NodeIdentifierSnippet(metadata, entity);
            var matchNodeSnippet = new MatchPropertySnippet(nodeSnippet, identifierNameSnippet);
            Assert.Equal("(Node_1:Node {Id:{Node_1}})", matchNodeSnippet.ToString());
        }
    }
}
