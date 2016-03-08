using System;
using System.Collections.Generic;
using Moq;
using Plot.Metadata;
using Plot.Proxies;
using Plot.Queries;
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

            var metadataFactory = new MetadataFactory();
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
                    return new GenericAbstractRepository<Person>(personMapper.Object, s, new DynamicProxyFactory(metadataFactory));
                }

                if (t == typeof(Contact))
                {
                    return new GenericAbstractRepository<Contact>(contactMapper.Object, s, new DynamicProxyFactory(metadataFactory));
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
    }
}
