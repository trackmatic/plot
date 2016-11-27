using Plot.Logging;
using Plot.Metadata;
using Plot.Neo4j.Cypher;
using Plot.Tests.Model;
using Xunit;

namespace Plot.Tests.Neo4j.Cypher
{
    public class statement_factory_tests
    {
        [Fact]
        public void ShouldCreateSetStatement()
        {
            var factory = new AttributeMetadataFactory(new NullLogger());
            var person = new Person { Id = "1" };
            var node = new Node(factory.Create(person), person);
            var result = StatementFactory.Set(node, "y");
            Assert.Equal("Person_1 = {y}", result);
        }

        [Fact]
        public void ShouldCreateParameterStatement()
        {
            var factory = new AttributeMetadataFactory(new NullLogger());
            var person = new Person {Id = "1"};
            var node = new Node(factory.Create(person), person);
            var result = StatementFactory.Parameter(node);
            Assert.Equal("Person_1", result);
        }

        [Fact]
        public void ShouldCreateMergeStatement()
        {
            var factory = new AttributeMetadataFactory(new NullLogger());
            var person = new Person { Id = "1" };
            var node = new Node(factory.Create(person), person);
            var id = StatementFactory.IdParameter(node);
            var result = StatementFactory.Merge(node, id);
            Assert.Equal("(Person_1:Person { Id:{Person_1_id}})", result);
        }

        [Fact]
        public void ShouldCreateRelationshipStatement()
        {
            var metadata = new RelationshipMetadata { Name = "REL", IsReverse = false };
            var result = StatementFactory.Relationship(metadata);
            Assert.Equal("-[:REL]->", result);
        }

        [Fact]
        public void ShouldCreateReverseRelationshipStatement()
        {
            var metadata = new RelationshipMetadata {Name = "REL", IsReverse = true};
            var result = StatementFactory.Relationship(metadata);
            Assert.Equal("<-[:REL]-", result);
        }

        [Fact]
        public void ShouldCreateNamedRelationship()
        {
            var factory = new AttributeMetadataFactory(new NullLogger());
            var personA = new Person { Id = "A" };
            var nodeA = new Node(factory.Create(personA), personA);
            var personB = new Person { Id = "B" };
            var nodeB = new Node(factory.Create(personB), personB);
            var relationship = new RelationshipMetadata { Name = "REL", IsReverse = true };
            var result = StatementFactory.Relationship(nodeA, nodeB, relationship, "r");
            Assert.Equal("(Person_A)<-[r:REL]-(Person_B)", result);
        }
    }
}
