using System;
using System.Linq;
using System.Collections.Generic;
using Moq;
using Plot.Attributes;
using Plot.Metadata;
using Plot.Proxies;
using Plot.Queries;
using Xunit;

namespace Plot.Tests
{
    public class dependencies_tests
    {
        [Fact]
        public void register_causes_the_caller_dependency_to_increment()
        {
            var one = new Dependencies("1");
            Assert.Equal(1, one.Sequence);

            var two = new Dependencies("2");
            one.Register(two);
            Assert.Equal(2, two.Sequence);
            Assert.Equal(1, one.Sequence);

            var another_two = new Dependencies("3");
            one.Register(another_two);
            Assert.Equal(2, another_two.Sequence);
            Assert.Equal(1, one.Sequence);

            var three = new Dependencies("4");
            two.Register(three);
            Assert.Equal(3, three.Sequence);
        }

        [Fact]
        public void dependencies_are_incremented_as_relationships_are_created()
        {
            var stateTracker = new EntityStateCache();
            var stateFactory = new Mock<IEntityStateCacheFactory>();
            stateFactory.Setup(x => x.Create()).Returns(stateTracker);

            var metadataFactory = new AttributeMetadataFactory();
            var proxyFactory = new DynamicProxyFactory(metadataFactory);

            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var repositoryFactory = new RepositoryFactory(proxyFactory);
            repositoryFactory.Register<Organisation>(session => new Mapper<Organisation>(session, metadataFactory));
            repositoryFactory.Register<AccessGroup>(session => new Mapper<AccessGroup>(session, metadataFactory));
            repositoryFactory.Register<Site>(session => new Mapper<Site>(session, metadataFactory));
            repositoryFactory.Register<Person>(session => new Mapper<Person>(session, metadataFactory));
            var sessionFactory = new GraphSessionFactory(queryExecutorFactory.Object, repositoryFactory, stateFactory.Object);

            using (var session = sessionFactory.OpenSession())
            {
                var organisation = session.Create(new Organisation
                {
                    Id = "organisation"
                });

                var site = session.Create(new Site
                {
                    Id = "site"
                });

                var accessGroup = session.Create(new AccessGroup
                {
                    Id = "accessGroup"
                });

                var person = session.Create(new Person
                {
                    Id = "person"
                });

                organisation.Add(site);
                organisation.Add(accessGroup);
                site.Add(accessGroup);
                person.Add(organisation);
                person.Add(site);

                var organisationMetadata = stateTracker.Get(organisation);
                var siteMetadata = stateTracker.Get(site);
                var accessGroupMetadata = stateTracker.Get(accessGroup);
                var personMetadata = stateTracker.Get(person);

                Assert.Equal(1, accessGroupMetadata.Dependencies.Sequence);
                Assert.Equal(2, siteMetadata.Dependencies.Sequence);
                Assert.Equal(3, organisationMetadata.Dependencies.Sequence);
                Assert.Equal(4, personMetadata.Dependencies.Sequence);
            }
        }

        public class Organisation
        {
            public Organisation()
            {
                Sites = new List<Site>();
                AccessGroups = new List<AccessGroup>();
            }

            public virtual string Id { get; set; }

            [Relationship("HAS_SITE")]
            public virtual IList<Site> Sites { get; set; }

            [Relationship("HAS_ACCESS_GROUPS")]
            public virtual IList<AccessGroup> AccessGroups { get; set; }

            public virtual void Add(Site site)
            {
                if (Sites.Contains(site))
                {
                    return;
                }
                Sites.Add(site);
            }

            public virtual void Add(AccessGroup accessGroup)
            {
                if (AccessGroups.Contains(accessGroup))
                {
                    return;
                }
                AccessGroups.Add(accessGroup);
            }
        }

        public class Site
        {
            public Site()
            {
                AccessGroups = new List<AccessGroup>();
            }

            public virtual string Id { get; set; }

            [Relationship("MAINTAINS")]
            public virtual IList<AccessGroup> AccessGroups { get; set; }

            public virtual void Add(AccessGroup accessGroup)
            {
                if (AccessGroups.Contains(accessGroup))
                {
                    return;
                }
                AccessGroups.Add(accessGroup);
            }
        }

        public class AccessGroup
        {
            public virtual string Id { get; set; }

        }

        public class Person
        {
            public Person()
            {
                Sites = new List<Site>();
                Organisations = new List<Organisation>();
            }

            public virtual string Id { get; set; }

            [Relationship("MEMBER_OF")]
            public virtual IList<Site> Sites { get; set; }

            [Relationship("MEMBER_OF")]
            public virtual IList<Organisation> Organisations { get; set; }

            public virtual void Add(Organisation organisation)
            {
                if (Organisations.Contains(organisation))
                {
                    return;
                }
                Organisations.Add(organisation);
            }

            public virtual void Add(Site site)
            {
                if (Sites.Contains(site))
                {
                    return;
                }
                Sites.Add(site);
            }

        }
    }
}
