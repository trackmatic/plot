using System.Collections.Generic;
using Moq;
using Octo.Core;
using Octo.Core.Proxies;
using Octo.Core.Queries;
using Octo.Tests.Model;
using Xunit;

namespace Octo.Tests
{
    public class dynamic_proxy_factory_tests
    {
        [Fact]
        public void lists_are_proxied()
        {
            var queryExecutorFactory = new Mock<IQueryExecutorFactory>();
            var repositoryFactory = new Mock<IRepositoryFactory>();
            var session = new GraphSession(new UnitOfWork(), new List<IListener>(), queryExecutorFactory.Object, repositoryFactory.Object);
            var factory = new DynamicProxyFactory();
            var item = new Person
            {
                Id = "1",
                Contacts = new List<Contact>
                {
                    new Contact { Id = "1" }
                }
            };
            var proxy = factory.Create(item, session);
            Assert.True(EntityUtils.IsProxy(proxy));
            Assert.True(proxy.Contacts is ITrackableCollection<Contact>);
            Assert.True(EntityUtils.IsProxy(proxy.Contacts[0]));
        }
    }
}
