using System;
using Neo4jClient;
using Plot.Metadata;
using Plot.N4j;
using Plot.N4j.Queries;
using Plot.Proxies;
using Plot.Sample.Mappers;
using Plot.Sample.Model;

namespace Plot.Sample
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
                var organisation = session.Get<Organisation>("80");

                var site = session.Get<Site>(organisation.Sites[0].Id);

                var asset = site.Assets[0];
                
                
                session.SaveChanges();
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}