using System;
using Neo4jClient;
using Plot.Metadata;
using Plot.Neo4j;
using Plot.Neo4j.Queries;
using Plot.Proxies;
using Plot.Sample.Data.Mappers;
using Plot.Sample.Queries;

namespace Plot.Sample.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var db = new GraphClient(new Uri("http://neo4j:trackmatic@localhost:7474/db/data"));

            db.Connect();

            var entityStateFactory = new EntityStateCacheFactory();
            var metadataFactory = new MetadataFactory();
            var proxyFactory = new DynamicProxyFactory(metadataFactory);
            var transactionFactory = new CypherTransactionFactory(db);
            var repositoryFactory = new RepositoryFactory(db, transactionFactory, proxyFactory, metadataFactory, typeof (OrganisationMapper).Assembly);
            var queryExecutorFactory = new QueryExecutorFactory(db, typeof (OrganisationMapper).Assembly);
            var factory = new GraphSessionFactory(queryExecutorFactory, repositoryFactory, entityStateFactory);

            using (var session = factory.OpenSession())
            {
                var results = session.Query(new GetOrganisationsByName()).Data;
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}