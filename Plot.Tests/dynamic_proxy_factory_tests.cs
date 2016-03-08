using System.Collections.Generic;
using Moq;
using Plot.Metadata;
using Plot.Proxies;
using Plot.Queries;
using Plot.Tests.Model;
using Xunit;

namespace Plot.Tests
{
    public class dynamic_proxy_factory_tests
    {
        [Fact]
        public void lists_are_proxied()
        {
            var metadataFactory = new MetadataFactory();
            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var session = new GraphSession(new UnitOfWork(), new List<IListener>(), queryExecutorFactory.Object, repositoryFactory.Object);
            var factory = new DynamicProxyFactory(metadataFactory);
            var item = new Person
            {
                Id = "1",
                Contacts = new List<Contact>
                {
                    new Contact { Id = "1" }
                }
            };
            var proxy = factory.Create(item, session);
            Assert.True(ProxyUtils.IsProxy(proxy));
            Assert.True(proxy.Contacts is ITrackableCollection<Contact>);
            Assert.True(ProxyUtils.IsProxy(proxy.Contacts[0]));
        }
    }
}
