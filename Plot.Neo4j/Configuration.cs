using System;
using System.Reflection;
using Neo4jClient;
using Plot.Metadata;
using Plot.Neo4j.Queries;
using Plot.Proxies;

namespace Plot.Neo4j
{
    public static class Configuration
    {
        public static IGraphSessionFactory CreateGraphSessionFactory(Uri uri, params Assembly[] mapperAssemblies)
        {
            var db = new GraphClient(uri);
            db.Connect();
            var entityStateFactory = new EntityStateCacheFactory();
            var metadataFactory = new AttributeMetadataFactory();
            var proxyFactory = new DynamicProxyFactory(metadataFactory);
            var transactionFactory = new CypherTransactionFactory(db);
            var repositoryFactory = new RepositoryFactory(db, transactionFactory, proxyFactory, metadataFactory, mapperAssemblies);
            var queryExecutorFactory = new QueryExecutorFactory(db, mapperAssemblies);
            var factory = new GraphSessionFactory(queryExecutorFactory, repositoryFactory, entityStateFactory);
            return factory;
        }
    }
}
