using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
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
            db.ExecutionConfiguration.ResourceManagerId = GetResourceManagerIdForEndpoint(app);
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

        private static Guid GetResourceManagerIdForEndpoint(string name)
        {
            var resourceManagerId = name + "@" + Environment.MachineName + "_trackmatic_5568";
            return DeterministicGuidBuilder(resourceManagerId);
        }

        [DebuggerNonUserCode]
        private static Guid DeterministicGuidBuilder(string input)
        {
            var provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(input);
            byte[] hashBytes = provider.ComputeHash(inputBytes);
            return new Guid(hashBytes);
        }
    }
}
