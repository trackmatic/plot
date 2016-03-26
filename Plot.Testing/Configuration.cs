using System;
using Plot.Logging;
using Plot.Metadata;
using Plot.Proxies;
using Plot.Queries;

namespace Plot.Testing
{
    public static class Configuration
    {
        public static Func<ILogger> Logger = () => new NullLogger();

        public static Func<IQueryExecutorFactory> QueryExecutorFactory = () => new QueryExecutorFactory();

        public static Func<IEntityStateCacheFactory> EntityStateCacheFactory = () => new EntityStateCacheFactory();

        public static Func<ILogger, IMetadataFactory> MetadataFactory = logger => new AttributeMetadataFactory(logger);

        public static Func<IMetadataFactory, ILogger, IProxyFactory> ProxyFactory = (metadataFactory, logger) => new DynamicProxyFactory(metadataFactory, logger);

        public static IGraphSessionFactory CreateTestSessionFactory(params IMapper[] mappers)
        {
            var logger = Logger();
            var metadataFactory = MetadataFactory(logger);
            var proxyFactory = ProxyFactory(metadataFactory, logger);
            var entityStateCacheFactory = EntityStateCacheFactory();
            var queryExecutorFactory = QueryExecutorFactory();
            var repositoryFactory = new RepositoryFactory(proxyFactory);
            foreach (var mapper in mappers)
            {
                repositoryFactory.Register(session => mapper, mapper.Type);
            }
            var factory = new GraphSessionFactory(queryExecutorFactory, repositoryFactory, entityStateCacheFactory, proxyFactory);
            return factory;
        }

        public static IGraphSessionFactory CreateTestSessionFactory(IRepositoryFactory repositoryFactory)
        {
            var logger = Logger();
            var metadataFactory = MetadataFactory(logger);
            var proxyFactory = ProxyFactory(metadataFactory, logger);
            var entityStateCacheFactory = EntityStateCacheFactory();
            var queryExecutorFactory = QueryExecutorFactory();
            var factory = new GraphSessionFactory(queryExecutorFactory, repositoryFactory, entityStateCacheFactory, proxyFactory);
            return factory;
        }
    }
}
