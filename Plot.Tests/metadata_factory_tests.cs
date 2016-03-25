using System.Collections.Generic;
using System.Linq;
using Moq;
using Plot.Attributes;
using Plot.Logging;
using Plot.Metadata;
using Plot.Proxies;
using Plot.Queries;
using Plot.Tests.Model;
using Xunit;

namespace Plot.Tests
{
    public class metadata_factory_tests
    {
        [Fact]
        public void factory_creates_meta_data()
        {
            var factory = new AttributeMetadataFactory(new NullLogger());
            var node = factory.Create(typeof(Person));
            Assert.Equal("Person", node.Name);
            Assert.Equal(3, node.Properties.Count());

            Assert.Equal("Id", node["Id"].Name);
            Assert.True(node["Id"].IsPrimitive);

            Assert.Equal("Name", node["Name"].Name);
            Assert.True(node["Name"].IsPrimitive);

            Assert.Equal("Contacts", node["Contacts"].Name);
            Assert.True(node["Contacts"].IsList);
            Assert.False(node["Contacts"].IsPrimitive);
            Assert.NotNull(node["Contacts"].Relationship);
            Assert.Equal("LINKED_TO", node["Contacts"].Relationship.Name);
        }

        [Fact]
        public void factory_creates_meta_data_correctly_from_a_proxy_object()
        {
            var metadataFactory = new AttributeMetadataFactory(new NullLogger());
            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var stateTracker = new EntityStateCache();
            var proxyFactory = new DynamicProxyFactory(metadataFactory, new NullLogger());
            using (var session = new GraphSession(new UnitOfWork(stateTracker), new List<IListener>(), queryExecutorFactory.Object, repositoryFactory.Object, stateTracker, proxyFactory))
            {
                var factory = new DynamicProxyFactory(metadataFactory, new NullLogger());
                var item = new Parent
                {
                    Id = "1",
                    Child = new Child { Id = "2"},
                    Children = new List<Child> {  new Child {  Id = "3"} }
                };
                var proxy = factory.Create(item, session);
                var parentMetadata = metadataFactory.Create(proxy);
                Assert.Equal("Parent", parentMetadata.Name);

                var childMetadta = metadataFactory.Create(proxy.Child);
                Assert.Equal("Child", childMetadta.Name);
            }
        }

        [Fact]
        public void reverse_property_is_set_correctly_on_one_to_one_relationships()
        {
            var metadataFactory = new AttributeMetadataFactory(new NullLogger());
            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var stateTracker = new EntityStateCache();
            var proxyFactory = new DynamicProxyFactory(metadataFactory, new NullLogger());
            using (var session = new GraphSession(new UnitOfWork(stateTracker), new List<IListener>(), queryExecutorFactory.Object, repositoryFactory.Object, stateTracker, proxyFactory))
            {
                var factory = new DynamicProxyFactory(metadataFactory, new NullLogger());
                var alpha = factory.Create(new ClassAlpha
                {
                    Id = "1"
                }, session);
                var beta = factory.Create(new ClassBeta
                {
                    Id = "2"
                }, session);
                alpha.Beta = beta;
                var metadata = metadataFactory.Create(beta);
                Assert.True(metadata["Alpha"].Relationship.IsReverse);
            }
        }

        public class Parent
        {
            public Parent()
            {
                Children = new List<Child>();
            }

            public virtual string Id { get; set; }

            [Relationship("HAS_A")]
            public virtual Child Child { get; set; }

            [Relationship("HAS_MANY")]
            public virtual IList<Child> Children { get; set; }
        }

        public class Child
        {
            public virtual string Id { get; set; }
        }

        public class ClassAlpha
        {
            public virtual string Id { get; set; }

            [Relationship("HAS")]
            public virtual ClassBeta Beta { get; set; }
            
        }

        public class ClassBeta
        {
            public virtual string Id { get; set; }

            [Relationship("HAS", Reverse = true)]
            public virtual  ClassAlpha Alpha { get; set; }
        }
    }
}