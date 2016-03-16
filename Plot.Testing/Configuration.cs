using Plot.Logging;
using Plot.Metadata;
using Plot.Proxies;

namespace Plot.Testing
{
    public static class Configuration
    {
        private static ILogger _logger = new ConsoleLogger();

        public static void With(ILogger logger)
        {
            _logger = logger;
        }

        public static void WithNoLogging()
        {
            _logger = new NullLogger();
        }

        public static IGraphSessionFactory CreateTestSessionFactory(params IMapper[] mappers)
        {
            var metadataFactory = new AttributeMetadataFactory(_logger);
            var proxyFactory = new DynamicProxyFactory(metadataFactory, _logger);
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
