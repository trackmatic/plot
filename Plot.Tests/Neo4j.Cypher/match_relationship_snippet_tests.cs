using Plot.Metadata;
using Plot.Neo4j.Cypher;
using Xunit;

namespace Plot.Tests.Neo4j.Cypher
{
    public class match_relationship_snippet_tests
    {
        [Fact]
        public void it_generates_valid_cypher_when_identifier_not_supplied()
        {
            var source = CreateSource();
            var destination = CreateDestination();
            var relationship = new RelationshipSnippet(new RelationshipMetadata {Name = "BELONGS_TO"});
            var snippet = new MatchRelationshipSnippet(source, destination, relationship);
            Assert.Equal("(Parent_1-[:BELONGS_TO]->Child_2)", snippet.ToString());
        }

        [Fact]
        public void it_generates_valid_cypher_when_identifier_supplied()
        {
            var source = CreateSource();
            var destination = CreateDestination();
            var relationship = new RelationshipSnippet(new IdentifierNameSnippet("rel"), new RelationshipMetadata { Name = "BELONGS_TO" });
            var snippet = new MatchRelationshipSnippet(source, destination, relationship);
            Assert.Equal("(Parent_1-[rel:BELONGS_TO]->Child_2)", snippet.ToString());
        }

        private IdentifierNameSnippet CreateSource()
        {
            return new NodeIdentifierSnippet(new NodeMetadata { Name = "Parent" }, new { Id = "1" });
        }

        private IdentifierNameSnippet CreateDestination()
        {
            return new NodeIdentifierSnippet(new NodeMetadata { Name = "Child" }, new { Id = "2" });
        }

        private IdentifierNameSnippet CreateRelationshipIdentifier()
        {
            return new IdentifierNameSnippet("rel");
        }
    }
}
