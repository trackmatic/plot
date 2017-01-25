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
            var uri = new Uri("bolt://n4j.tm.local:7687/db/data");
            Configuration.Logger = () => new ConsoleLogger();
            var factory = Configuration.CreateGraphSessionFactory(uri, "neo4j", "trackmatic101", typeof (MovieMapper).Assembly);


            var start = DateTime.UtcNow;
            using (var session = factory.OpenSession())
            {
                var person = session.Get<Person>("test_person");

                person.Remove(person.Movies[0]);
                session.SaveChanges();
            }
            Console.WriteLine(DateTime.UtcNow.Subtract(start));
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}