using System;
using System.Collections.Generic;
using Plot.Neo4j;
using Plot.Sample.Data.Mappers;
using Plot.Sample.Queries;

namespace Plot.Sample.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var uri = new Uri("http://localhost:7474/db/data");
            var factory = Configuration.CreateGraphSessionFactory(uri, "neo4j", "trackmatic", typeof (UserMapper).Assembly);
            using (var session = factory.OpenSession())
            {
                var organisation = session.Query(new GetSitesByOrganisation("e08811bf-775f-432c-ba5f-b50150ef8964", "")
                {
                    OrderBy = new[] {"site.Name", "-site.Reference"}
                }, false);
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}