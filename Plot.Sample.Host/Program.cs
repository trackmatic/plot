using System;
using Plot.Neo4j;
using Plot.Sample.Data.Mappers;
using Plot.Sample.Model;

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
                var organisation = session.Create(new Organisation
                {
                    Id = "1",
                    Name = "Trackmatic"
                });

                var site = session.Create(new Site
                {
                    Id = "1",
                    Name = "Jhb"
                });

                organisation.Add(site);

                var accessGroup = session.Create(new AccessGroup
                {
                    Id = "1",
                    Name = "Developers"
                });

                organisation.Add(accessGroup);

                session.SaveChanges();
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}