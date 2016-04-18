using System;
using System.Linq;
using Plot.Logging;
using Plot.Neo4j;
using Plot.Sample.Data.Mappers;

namespace Plot.Sample.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var uri = new Uri("http://n4j.trackmatic.co.za:7474/db/data");
            Configuration.Logger = () => new ConsoleLogger();
            var factory = Configuration.CreateGraphSessionFactory(uri, "neo4j", "trackmatic101", typeof (UserMapper).Assembly);
            while (true)
            {
                var start = DateTime.UtcNow;
                using (var session = factory.OpenSession())
                {

                    var user = session.Get<User>("0b732bbf-16a3-41f5-a3f2-e3ee9eb6e635");


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