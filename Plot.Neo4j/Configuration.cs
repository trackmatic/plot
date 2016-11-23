using System;
using System.Reflection;
using Neo4j.Driver.V1;
using Plot.Logging;
using Plot.Metadata;
using Plot.Neo4j.Queries;
using Plot.Proxies;
using Plot.Queries;

namespace Plot.Neo4j
{
    public static class Configuration
    {
        public static Func<Guid> ResourceManagerId = () => Guid.Parse("5c62baaa-2c28-4a2d-b55f-36b96503ffc3");

        public static Func<Logging.ILogger> Logger = () => new NullLogger();

        public static Func<IEntityStateCacheFactory> EntityStateCacheFactory = () => new EntityStateCacheFactory();

        public static Func<Logging.ILogger, IMetadataFactory> MetadataFactory = logger => new AttributeMetadataFactory(logger);

        public static Func<IMetadataFactory, Logging.ILogger, IProxyFactory> ProxyFactory = (metadataFactory, logger) => new DynamicProxyFactory(metadataFactory, logger);

        public static Func<IDriver, Logging.ILogger, ICypherTransactionFactory> CypherTransactionFactory = (db, logger) => new CypherTransactionFactory(db, logger);

        public static  Func<IDriver, ICypherTransactionFactory, IProxyFactory, IMetadataFactory, Assembly[], IRepositoryFactory> RepositoryFactory = (db, transactionFactory, proxyFactory, metadataFactory, assemblies) => new RepositoryFactory(db, transactionFactory, proxyFactory, metadataFactory, assemblies);

        public static Func<ICypherTransactionFactory, IMetadataFactory, Assembly[], IQueryExecutorFactory> QueryExecutorFactory = (transactionFactory, metadataFactory, assemblies) => new QueryExecutorFactory(transactionFactory, metadataFactory, assemblies);

        public static Func<IQueryExecutorFactory, IRepositoryFactory, IEntityStateCacheFactory, IProxyFactory, IGraphSessionFactory> GraphSessionFactory = (queryExecutorFactory, repositoryFactory, entityStateCacheFactory, proxyFactory) => new GraphSessionFactory(queryExecutorFactory, repositoryFactory, entityStateCacheFactory, proxyFactory);

        public static IGraphSessionFactory CreateGraphSessionFactory(Uri uri, string username, string password, params Assembly[] mapperAssemblies)
        {
            var token = AuthTokens.Basic(username, password);
            var db = GraphDatabase.Driver(uri, token);
            var entityStateCacheFactory = EntityStateCacheFactory();
            var logger = Logger();
            var metadataFactory = MetadataFactory(logger);
            var proxyFactory = ProxyFactory(metadataFactory, logger);
            var transactionFactory = CypherTransactionFactory(db, logger);
            var repositoryFactory = RepositoryFactory(db, transactionFactory, proxyFactory, metadataFactory, mapperAssemblies);
            var queryExecutorFactory = QueryExecutorFactory(transactionFactory, metadataFactory, mapperAssemblies);
            var factory = GraphSessionFactory(queryExecutorFactory, repositoryFactory, entityStateCacheFactory, proxyFactory);
            return factory;
        }
    }
}
