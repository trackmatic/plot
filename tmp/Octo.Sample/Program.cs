using System;
using Neo4jClient;
using Octo.Core;
using Octo.Core.Proxies;
using Octo.N4j;
using Octo.N4j.Queries;
using Octo.Sample.Mappers;
using Octo.Sample.Model;

namespace Octo.Sample
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var db = new GraphClient(new Uri("http://neo4j:trackmatic101@n4j.trackmatic.co.za:7474/db/data"));

            db.Connect();

            var proxyFactory = new DynamicProxyFactory();
            var transactionFactory = new CypherTransactionFactory(db);
            var repositoryFactory = new RepositoryFactory(db, transactionFactory, proxyFactory, typeof (OrganisationMapper).Assembly);
            var queryExecutorFactory = new QueryExecutorFactory(db, typeof (OrganisationMapper).Assembly);
            var factory = new GraphSessionFactory(queryExecutorFactory, repositoryFactory);

            using (var session = factory.OpenSession())
            {
                var organisation = session.Get<Organisation>("80");
            }

        }
    }
}