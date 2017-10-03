using System.Collections.Generic;
using Plot.Attributes;
using Plot.Metadata;
using Plot.Neo4j.Cypher;
using Plot.Tests.Model;
using Plot.Tests.Utility;
using Xunit;

namespace Plot.Tests.Neo4j.Cypher
{
    public class cypher_query_tests
    {
        [Fact]
        public void DuplicateParametersAreOverwritten()
        {
            var query = new CypherQuery<Person>();
            var result = query.WithParam("param1", "value 1");
            Assert.Equal("value 1", result.Parameters["param1"]);
            result = query.WithParam("param1", "value 2");
            Assert.Equal(1, result.Parameters.Count);
            Assert.Equal("value 2", result.Parameters["param1"]);
        }

        [Fact]
        public void MatchShouldGenerateValidSyntax()
        {
            var factory = new AttributeMetadataFactory(null);
            var person = new Person { Id = "1" };
            var node = new Node(factory.Create(person), person);
            var statement = StatementFactory.Match(node, StatementFactory.Parameter(node));
            var query = new CypherQuery<Person>();
            var response = query.Match(statement);
            Assert.Equal("MATCH (Person_1:Person { Id:{Person_1}})", response.Statement);
        }

        [Fact]
        public void InculdeRelationshipsGeneratesValidSyntax()
        {
            var factory = new AttributeMetadataFactory(null);
            var person = new PersonWithNotNullAttributes { Id = "1" };
            var metadata = factory.Create(person);
            var query = new CypherQuery<Person>();
            var response = query.IncludeRelationships(metadata);
            var expected = @"MATCH ((personWithNotNullAttributes)-[:HAS_A]->(person1:Person))
MATCH ((personWithNotNullAttributes)-[:HAS_A]->(person2:Person))
OPTIONAL MATCH ((personWithNotNullAttributes)-[:LINKED_TO]->(contacts:Contact))";
            Assert.Equal(expected , response.Statement);
        }


        public class PersonWithNotNullAttributes
        {
            public virtual string Id { get; set; }

            public virtual string Name { get; set; }

            [Relationship(Name = "HAS_A", NotNull = true)]
            public virtual Person Person1 { get; set; }

            [Relationship(Name = "LINKED_TO")]
            public virtual IList<Contact> Contacts { get; set; }

            [Relationship(Name = "HAS_A", NotNull = true)]
            public virtual Person Person2 { get; set; }

            public override int GetHashCode()
            {
                return Utils.GetHashCode(Id);
            }

            public override bool Equals(object obj)
            {
                return Utils.Equals(this, obj);
            }
        }
    }
}
