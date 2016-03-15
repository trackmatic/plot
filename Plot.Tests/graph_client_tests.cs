using System;
using System.Collections.Generic;
using Moq;
using Plot.Logging;
using Plot.Metadata;
using Plot.Proxies;
using Plot.Queries;
using Plot.Testing;
using Plot.Tests.Model;
using Xunit;

namespace Plot.Tests
{
    public class graph_client_tests
    {
        [Fact]
        public void dirty_tracking_should_cause_udpate()
        {
            var target = new Person
            {
                Id = "1",
                Contacts = new List<Contact>
                {
                    new Contact {Id = "1"}
                }
            };

            var stateTracker = new EntityStateCache();
            var stateFactory = new Mock<IEntityStateCacheFactory>();
            stateFactory.Setup(x => x.Create()).Returns(stateTracker);

            var metadataFactory = new AttributeMetadataFactory(new NullLogger());
            var personMapper = new Mock<IMapper<Person>>();
            personMapper.Setup(x => x.Get(It.IsAny<string[]>())).Returns(new List<Person> { target });
            personMapper.Setup(x => x.Update(It.IsAny<object>(), It.IsAny<EntityState>())).Verifiable();

            var contactMapper = new Mock<IMapper<Contact>>();
            contactMapper.Setup(x => x.Get(It.IsAny<string[]>())).Returns(new List<Contact> { new Contact { Id = "1" } });

            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            repositoryFactory.Setup(x => x.Create(It.IsAny<IGraphSession>(), It.IsAny<Type>())).Returns<IGraphSession, Type>((s, t) =>
            {
                if (t == typeof (Person))
                {
                    return new GenericAbstractRepository<Person>(personMapper.Object, s, new DynamicProxyFactory(metadataFactory, new NullLogger()));
                }

                if (t == typeof(Contact))
                {
                    return new GenericAbstractRepository<Contact>(contactMapper.Object, s, new DynamicProxyFactory(metadataFactory, new NullLogger()));
                }

                return null;
            });
            var sessionFactory = new GraphSessionFactory(queryExecutorFactory.Object, repositoryFactory.Object, stateFactory.Object);
            using (var session = sessionFactory.OpenSession())
            {
                var person = session.Get<Person>("1");
                person.Name = "Ross";
                person.Contacts[0].Name = "New Name";
                session.SaveChanges();
            }
            personMapper.Verify();
        }

        [Fact]
        public void listeners_are_called_when_registered()
        {
            var stateTracker = new EntityStateCache();
            var stateFactory = new Mock<IEntityStateCacheFactory>();
            stateFactory.Setup(x => x.Create()).Returns(stateTracker);
            var metadataFactory = new AttributeMetadataFactory(new NullLogger());
            var personMapper = new Mock<IMapper<Person>>();
            personMapper.Setup(x => x.Update(It.IsAny<object>(), It.IsAny<EntityState>())).Verifiable();
            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var proxyFactory = new DynamicProxyFactory(metadataFactory, new NullLogger());
            var repositoryFactory = new RepositoryFactory(proxyFactory);
            repositoryFactory.Register<Person>(x => personMapper.Object);

            var listener = new Mock<IListener<Person>>();
            listener.Setup(x => x.Create(It.IsAny<object>(), It.IsAny<IGraphSession>())).Verifiable();

            var sessionFactory = new GraphSessionFactory(queryExecutorFactory.Object, repositoryFactory, stateFactory.Object);
            sessionFactory.Register(listener.Object);

            using (var session = sessionFactory.OpenSession())
            {
                session.Create(new Person
                {
                    Id = "1"
                });
                session.SaveChanges();
                listener.Verify();
            }
        }
    }
}
