using System;
using System.Reflection;
using Neo4jClient;
using Neo4jClient.Transactions;
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

        public static Func<ILogger> Logger = () => new NullLogger();

        public static Func<IEntityStateCacheFactory> EntityStateCacheFactory = () => new EntityStateCacheFactory();

        public static Func<ILogger, IMetadataFactory> MetadataFactory = logger => new AttributeMetadataFactory(logger);

        public static Func<IMetadataFactory, ILogger, IProxyFactory> ProxyFactory = (metadataFactory, logger) => new DynamicProxyFactory(metadataFactory, logger);

        public static Func<ITransactionalGraphClient, ILogger, ICypherTransactionFactory> CypherTransactionFactory = (db, logger) => new CypherTransactionFactory(db, logger);

        public static  Func<ITransactionalGraphClient, ICypherTransactionFactory, IProxyFactory, IMetadataFactory, Assembly[], IRepositoryFactory> RepositoryFactory = (db, transactionFactory, proxyFactory, metadataFactory, assemblies) => new RepositoryFactory(db, transactionFactory, proxyFactory, metadataFactory, assemblies);

        public static Func<ITransactionalGraphClient, IMetadataFactory, Assembly[], IQueryExecutorFactory> QueryExecutorFactory = (db, metadataFactory, assemblies) => new QueryExecutorFactory(db, metadataFactory, assemblies);

        public static Func<IQueryExecutorFactory, IRepositoryFactory, IEntityStateCacheFactory, IProxyFactory, IGraphSessionFactory> GraphSessionFactory = (queryExecutorFactory, repositoryFactory, entityStateCacheFactory, proxyFactory) => new GraphSessionFactory(queryExecutorFactory, repositoryFactory, entityStateCacheFactory, proxyFactory);

        public static IGraphSessionFactory CreateGraphSessionFactory(string app, Uri uri, string username, string password, params Assembly[] mapperAssemblies)
        {
            var db = new GraphClient(uri, new HttpClientAuthWrapper(username, password));
            db.ExecutionConfiguration.ResourceManagerId = ResourceManagerId();
            db.Connect();
            var entityStateCacheFactory = EntityStateCacheFactory();
            var logger = Logger();
            var metadataFactory = MetadataFactory(logger);
            var proxyFactory = ProxyFactory(metadataFactory, logger);
            var transactionFactory = CypherTransactionFactory(db, logger);
            var repositoryFactory = RepositoryFactory(db, transactionFactory, proxyFactory, metadataFactory, mapperAssemblies);
            var queryExecutorFactory = QueryExecutorFactory(db, metadataFactory, mapperAssemblies);
            var factory = GraphSessionFactory(queryExecutorFactory, repositoryFactory, entityStateCacheFactory, proxyFactory);
            return factory;
        }
    }
}
