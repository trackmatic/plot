using System.Reflection;
using Plot.Metadata;
using Plot.Proxies;

namespace Plot.Testing
{
    public static class Configuration
    {
        public static IGraphSessionFactory CreateTestSessionFactory(params IMapper[] mappers)
        {
            var metadataFactory = new AttributeMetadataFactory();
            var proxyFactory = new DynamicProxyFactory(metadataFactory);
            var repositoryFactory = new RepositoryFactory(proxyFactory);
            foreach (var mapper in mappers)
            {
                repositoryFactory.Register(session => mapper, mapper.Type);
            }
            return CreateTestSessionFactory(repositoryFactory);
        }

        public static IGraphSessionFactory CreateTestSessionFactory(IRepositoryFactory repositoryFactory)
        {
            var entityStateFactory = new EntityStateCacheFactory();
            var queryExecutorFactory = new QueryExecutorFactory();
            var factory = new GraphSessionFactory(queryExecutorFactory, repositoryFactory, entityStateFactory);
            return factory;
        }
    }
}
