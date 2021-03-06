﻿using System.Collections.Generic;
using System.Linq;
using Moq;
using Plot.Attributes;
using Plot.Logging;
using Plot.Metadata;
using Plot.Proxies;
using Plot.Queries;
using Xunit;

namespace Plot.Tests
{
    public class trackable_relationship_tests
    {
        public class Address
        {
            public virtual string Id { get; set; }

            public virtual string Name { get; set; }
        }

        public class Person
        {
            public virtual string Id { get; set; }

            public virtual string Name { get; set; }

            [Relationship]
            public virtual Address Address { get; set; }
        }

        [Fact]
        public void a_proxy_is_generated_for_trackable_relationships()
        {
            var metadataFactory = new AttributeMetadataFactory(new NullLogger());
            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var stateTracker = new EntityStateCache();
            var proxyFactory = new DynamicProxyFactory(metadataFactory, new NullLogger());
            using (var session = new GraphSession(new UnitOfWork(stateTracker), new List<IListener>(), queryExecutorFactory.Object, repositoryFactory.Object, stateTracker, proxyFactory))
            {
                var person = new Person
                {
                    Id = "1",
                    Name = "Test",
                    Address = new Address
                    {
                        Id = "1",
                        Name = "first address"
                    }
                };
                var proxy = proxyFactory.Create(person, session);
                proxy.Address = null;
                proxy.Address = proxyFactory.Create(new Address {Id = "2"}, session);
                var metadata = metadataFactory.Create(proxy);
                var trackableRelationships = ProxyUtils.Flush(proxy, metadata["Address"].Relationship).ToList();
                Assert.Equal(1, trackableRelationships.Count);
                foreach (var trackableRelationship in trackableRelationships)
                {
                    var enumerable = trackableRelationship.Flush();
                    var results = enumerable.Cast<object>().ToList();
                    Assert.Equal(1, results.Count);
                    Assert.Equal("1", ((Address) results[0]).Id);
                    Assert.Equal("2", proxy.Address.Id);
                }
            }
        }
    }
}
