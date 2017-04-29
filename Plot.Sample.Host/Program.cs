using System;
using System.Collections.Generic;
using System.Linq;
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

                /*var person1 = session.Create(new Person {Id = "person1"});
                var movie1 = session.Create(new Movie {Id = "movie1"});
                var movie2 = session.Create(new Movie { Id = "movie2" });
                person1.Add(movie1);
                person1.Add(movie2);

                var person2 = session.Create(new Person { Id = "person2" });
                var movie3 = session.Create(new Movie { Id = "movie3" });
                person2.Add(movie3);
                session.SaveChanges();*/


                var movie = session.Get<Person>("person2");
            }
            Console.WriteLine(DateTime.UtcNow.Subtract(start));
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}