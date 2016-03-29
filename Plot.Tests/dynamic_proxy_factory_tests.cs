using System;
using System.Linq;
using System.Collections.Generic;
using Moq;
using Plot.Attributes;
using Plot.Logging;
using Plot.Metadata;
using Plot.Proxies;
using Plot.Queries;
using Plot.Testing;
using Plot.Tests.Model;
using Xunit;

namespace Plot.Tests
{
    public class dynamic_proxy_factory_tests
    {
        [Fact]
        public void all_objects_in_the_object_graph_are_proxied()
        {
            var metadataFactory = new AttributeMetadataFactory(new NullLogger());
            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var stateTracker = new EntityStateCache();
            var proxyFactory = new DynamicProxyFactory(metadataFactory, new NullLogger());
            using (var session = new GraphSession(new UnitOfWork(stateTracker), new List<IListener>(), queryExecutorFactory.Object, repositoryFactory.Object, stateTracker, proxyFactory))
            {
                var factory = new DynamicProxyFactory(metadataFactory, new NullLogger());
                var item = new Person
                {
                    Id = "1",
                    Contacts = new List<Contact>
                    {
                        new Contact {Id = "1"}
                    }
                };
                var proxy = factory.Create(item, session);
                Assert.True(ProxyUtils.IsProxy(proxy));
                Assert.True(proxy.Contacts is ITrackableCollection<Contact>);
                Assert.True(ProxyUtils.IsProxy(proxy.Contacts[0]));
            }
        }

        [Fact]
        public void properties_which_are_marked_as_ignored_are_not_proxied()
        {

            var metadataFactory = new AttributeMetadataFactory(new NullLogger());
            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var stateTracker = new EntityStateCache();
            var proxyFactory = new DynamicProxyFactory(metadataFactory, new NullLogger());
            using (var session = new GraphSession(new UnitOfWork(stateTracker), new List<IListener>(), queryExecutorFactory.Object, repositoryFactory.Object, stateTracker, proxyFactory))
            {
                var factory = new DynamicProxyFactory(metadataFactory, new NullLogger());
                var item = new EntityWithIgnoredProperty
                {
                    Id = "1",
                    Ignored = new AnotherEntity {Id = "2"},
                    Proxied = new AnotherEntity {Id = "3"}
                };
                var proxy = factory.Create(item, session);
                Assert.True(ProxyUtils.IsProxy(proxy));
                Assert.True(ProxyUtils.IsProxy(proxy.Proxied));
                Assert.False(ProxyUtils.IsProxy(proxy.Ignored));
            }
        }

        [Fact]
        public void id_property_is_set_if_null()
        {
            var metadataFactory = new AttributeMetadataFactory(new NullLogger());
            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var stateTracker = new EntityStateCache();
            var proxyFactory = new DynamicProxyFactory(metadataFactory, new NullLogger());
            using (var session = new GraphSession(new UnitOfWork(stateTracker), new List<IListener>(), queryExecutorFactory.Object, repositoryFactory.Object, stateTracker, proxyFactory))
            {
                var factory = new DynamicProxyFactory(metadataFactory, new NullLogger());
                var notSet = new AnotherEntity();
                var proxy = factory.Create(notSet, session);
                Assert.NotNull(proxy.Id);
                Assert.NotEmpty(proxy.Id);

                var set = new AnotherEntity {Id = "1"};
                proxy = factory.Create(set, session);
                Assert.Equal("1", proxy.Id);
            }
        }

        [Fact]
        public void depdendencies_are_incremented_when_property_set()
        {
            var metadataFactory = new AttributeMetadataFactory(new NullLogger());
            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var stateTracker = new EntityStateCache();
            var proxyFactory = new DynamicProxyFactory(metadataFactory, new NullLogger());
            using (var session = new GraphSession(new UnitOfWork(stateTracker), new List<IListener>(), queryExecutorFactory.Object, repositoryFactory.Object, stateTracker, proxyFactory))
            {
                var factory = new DynamicProxyFactory(metadataFactory, new NullLogger());
                var parent = factory.Create(new Parent {Id = "parent"}, session);
                var child = factory.Create(new Child {Id = "child"}, session);
                parent.Child = child;

                var parentState = stateTracker.Get(parent);
                Assert.Equal(2, parentState.Dependencies.Sequence);

                var childState = stateTracker.Get(child);
                Assert.Equal(1, childState.Dependencies.Sequence);
            }
        }

        [Fact]
        public void depdendencies_are_incremented_when_item_added_to_a_list()
        {
            var metadataFactory = new AttributeMetadataFactory(new NullLogger());
            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var stateTracker = new EntityStateCache();
            var proxyFactory = new DynamicProxyFactory(metadataFactory, new NullLogger());
            using (var session = new GraphSession(new UnitOfWork(stateTracker), new List<IListener>(), queryExecutorFactory.Object, repositoryFactory.Object, stateTracker, proxyFactory))
            {
                var factory = new DynamicProxyFactory(metadataFactory, new NullLogger());
                var parent = factory.Create(new Parent { Id = "parent" }, session);
                var child = factory.Create(new Child { Id = "child" }, session);
                parent.Children.Add(child);

                var parentState = stateTracker.Get(parent);
                Assert.Equal(2, parentState.Dependencies.Sequence);

                var childState = stateTracker.Get(child);
                Assert.Equal(1, childState.Dependencies.Sequence);
            }
        }

        [Fact]
        public void properties_set_during_construction_are_restored_after_proxy_created()
        {
            var metadataFactory = new AttributeMetadataFactory(new NullLogger());
            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var stateTracker = new EntityStateCache();
            var proxyFactory = new DynamicProxyFactory(metadataFactory, new NullLogger());
            using (var session = new GraphSession(new UnitOfWork(stateTracker), new List<IListener>(), queryExecutorFactory.Object, repositoryFactory.Object, stateTracker, proxyFactory))
            {
                var factory = new DynamicProxyFactory(metadataFactory, new NullLogger());
                var item = new EntityWithPropertiesSetInConstructor("1", "Test");
                var proxy = factory.Create(item, session);
                Assert.Equal("1", proxy.Id);
                Assert.Equal("Test", proxy.Name);
            }
        }

        [Fact]
        public void change_tracking_is_applied_when_entity_set_in_constructor()
        {
            var metadataFactory = new AttributeMetadataFactory(new NullLogger());
            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();

            var mapper = new Mock<IMapper<Parent>>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            repositoryFactory.Setup(x => x.Create(It.IsAny<IGraphSession>(), It.IsAny<Type>())).Returns<IGraphSession, Type>((s, t) => new GenericAbstractRepository<Parent>(mapper.Object, s, new DynamicProxyFactory(metadataFactory, new NullLogger())));
            var stateTracker = new EntityStateCache();
            var proxyFactory = new DynamicProxyFactory(metadataFactory, new NullLogger());
            using (var session = new GraphSession(new UnitOfWork(stateTracker), new List<IListener>(), queryExecutorFactory.Object, repositoryFactory.Object, stateTracker, proxyFactory))
            {
                var proxy = session.Create(new Parent
                {
                    Id = "1",
                    Child = new Child {Id = "2"}
                });
                var state = stateTracker.Get(proxy.Child);
                Assert.Equal(EntityStatus.New, state.Status);
            }
        }

        [Fact]
        public void interceptors_are_not_created_for_re_entrant_proxies()
        { 
            var metadataFactory = new AttributeMetadataFactory(new NullLogger());
            var proxyFactory = new DynamicProxyFactory(metadataFactory, new NullLogger());
            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var repositoryFactory = new RepositoryFactory(proxyFactory);
            var parentMapper = new Mock<IMapper<Parent>>();
            repositoryFactory.Register<Parent>(x => parentMapper.Object);
            var childMapper = new Mock<IMapper<Child>>();
            repositoryFactory.Register<Child>(x => childMapper.Object);

            var stateTracker = new EntityStateCache();
            using (var session = new GraphSession(new UnitOfWork(stateTracker), new List<IListener>(), queryExecutorFactory.Object, repositoryFactory, stateTracker, proxyFactory))
            {
                var proxy = session.Create(new Parent
                {
                    Id = "1"
                });
                proxy.Child = session.Create(new Child
                {
                    Id = "1"
                });

                var metadata = metadataFactory.Create(proxy);
                var deleted = ProxyUtils.Flush(proxy, metadata["Child"].Relationship);
                
                Assert.Equal(0, deleted.SelectMany(x => x.Flush().Cast<object>()).Count());

                var reentrant = session.Create(proxy);
                Assert.Equal(proxy, reentrant);

                metadata = metadataFactory.Create(reentrant);
                deleted = ProxyUtils.Flush(reentrant, metadata["Child"].Relationship);
                Assert.Equal(0, deleted.SelectMany(x => x.Flush().Cast<object>()).Count());

                proxy.Child = session.Create(new Child
                {
                    Id = "2"
                });
                metadata = metadataFactory.Create(reentrant);
                deleted = ProxyUtils.Flush(reentrant, metadata["Child"].Relationship);
                Assert.Equal(1, deleted.SelectMany(x => x.Flush().Cast<object>()).Count());
            }
        }

        [Fact]
        public void nullable_types_are_ignored()
        {
            var metadataFactory = new AttributeMetadataFactory(new NullLogger());
            var proxyFactory = new DynamicProxyFactory(metadataFactory, new NullLogger());
            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var repositoryFactory = new RepositoryFactory(proxyFactory);
            var parentMapper = new Mock<IMapper<Parent>>();
            repositoryFactory.Register<Parent>(x => parentMapper.Object);
            var childMapper = new Mock<IMapper<Child>>();
            repositoryFactory.Register<Child>(x => childMapper.Object);

            var stateTracker = new EntityStateCache();
            using (var session = new GraphSession(new UnitOfWork(stateTracker), new List<IListener>(), queryExecutorFactory.Object, repositoryFactory, stateTracker, proxyFactory))
            {
                var entity = new EntityWithNullableType
                {
                    Date = DateTime.Now
                };
                var proxy = proxyFactory.Create(entity, session);
                Assert.NotNull(proxy.Date);
            }
        }

        [Fact]
        public void objects_with_inheritance_chain_are_proxied_correctly()
        {
            var metadataFactory = new AttributeMetadataFactory(new NullLogger());
            var proxyFactory = new DynamicProxyFactory(metadataFactory, new NullLogger());
            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var repositoryFactory = new RepositoryFactory(proxyFactory);
            var parentMapper = new Mock<IMapper<Parent>>();
            repositoryFactory.Register<Parent>(x => parentMapper.Object);
            var childMapper = new Mock<IMapper<Child>>();
            repositoryFactory.Register<Child>(x => childMapper.Object);

            var stateTracker = new EntityStateCache();
            using (var session = new GraphSession(new UnitOfWork(stateTracker), new List<IListener>(), queryExecutorFactory.Object, repositoryFactory, stateTracker, proxyFactory))
            {
                var entity = new ParentWithInheritedChild
                {
                    Id = "1",
                    Type = new SuperTypeA { Id = "1"}
                };
                var proxy = proxyFactory.Create(entity, session);
                Assert.True(proxy.Type is SuperTypeA);
            }
        }

        public class EntityWithIgnoredProperty
        {
            public virtual string Id { get; set; }

            [Ignore]
            public virtual AnotherEntity Ignored { get; set; }

            public virtual AnotherEntity Proxied { get; set; }
        }

        public class AnotherEntity
        {
            public virtual string Id { get; set; }
        }

        public class Parent
        {
            public Parent()
            {
                Children = Children ?? new List<Child>();
            }

            public virtual string Id { get; set; }

            [Relationship("HAS_A")]
            public virtual Child Child { get; set; }

            [Relationship("HAS_A")]
            public virtual IList<Child> Children { get; set; }
        }

        public class Child
        {
            public virtual string Id { get; set; }
        }

        public class EntityWithPropertiesSetInConstructor
        {
            public EntityWithPropertiesSetInConstructor()
            {
                
            }

            public EntityWithPropertiesSetInConstructor(string id, string name)
            {
                Id = id;
                Name = name;
            }

            public virtual string Id { get; set; }

            public virtual string Name { get; set; }
        }

        public class EntityWithNullableType
        {
            public virtual string Id { get; set; }

            public virtual DateTime? Date { get; set; }
        }

        public class ParentWithInheritedChild
        {
            public virtual string Id { get; set; }

            public virtual BaseType Type { get; set; }
        }

        public class BaseType
        {
            public virtual string Id { get; set; }
        }

        public class SuperTypeA : BaseType
        {
            
        }
    }
}
