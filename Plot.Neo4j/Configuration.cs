using System;
using System.Reflection;
using Neo4jClient;
using Plot.Logging;
using Plot.Metadata;
using Plot.Neo4j.Queries;
using Plot.Proxies;

namespace Plot.Neo4j
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

        public static IGraphSessionFactory CreateGraphSessionFactory(Uri uri, string username, string password, params Assembly[] mapperAssemblies)
        {
            var db = new GraphClient(uri, new HttpClientAuthWrapper(username, password));
            db.Connect();
            var entityStateFactory = new EntityStateCacheFactory();
            var metadataFactory = new AttributeMetadataFactory(_logger);
            var proxyFactory = new DynamicProxyFactory(metadataFactory, _logger);
            var transactionFactory = new CypherTransactionFactory(db, _logger);
            var repositoryFactory = new RepositoryFactory(db, transactionFactory, proxyFactory, metadataFactory, mapperAssemblies);
            var queryExecutorFactory = new QueryExecutorFactory(db, mapperAssemblies);
            var factory = new GraphSessionFactory(queryExecutorFactory, repositoryFactory, entityStateFactory);
            return factory;
        }


    }
}
