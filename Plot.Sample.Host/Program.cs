using System;
using Plot.Neo4j;
using Plot.Sample.Data.Mappers;
using Plot.Sample.Queries;

namespace Plot.Sample.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var uri = new Uri("http://neo4j:trackmatic@localhost:7474/db/data");
            var factory = Configuration.CreateGraphSessionFactory(uri, typeof(OrganisationMapper).Assembly);
            using (var session = factory.OpenSession())
            {
                var results = session.Query(new GetOrganisationsByName()).Data;
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}