using System;
using Plot.Logging;
using Plot.Neo4j;
using Plot.Sample.Data.Mappers;
using Plot.Sample.Queries;

namespace Plot.Sample.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var uri = new Uri("http://n4j.tm.local:7474/db/data");
            Configuration.Logger = () => new ConsoleLogger();
            var factory = Configuration.CreateGraphSessionFactory(uri, "neo4j", "trackmatic101", typeof (UserMapper).Assembly);
            while (true)
            {
                var start = DateTime.UtcNow;
                using (var session = factory.OpenSession())
                {

                    var user = session.Query(new GetOrganisationsByName());


                    /*var membershipIds = user.Memberships.Where(x => x.IsActive).Select(x => x.Id).ToArray();
                    var memberships = session.Get<Membership>(membershipIds).ToList();*/
                }

                Console.WriteLine(DateTime.UtcNow.Subtract(start));

                Console.ReadLine();
            }

            Console.ReadLine();
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}