using System;
using Plot.Logging;
using Plot.Neo4j;
using Plot.Sample.Data.Mappers;

namespace Plot.Sample.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var uri = new Uri("bolt://n4j.tm.local:7687/db/data");
            Configuration.Logger = () => new ConsoleLogger();
            var factory = Configuration.CreateGraphSessionFactory(uri, "neo4j", "trackmatic101", typeof (MovieMapper).Assembly);
            while (true)
            {
                var start = DateTime.UtcNow;
                using (var session = factory.OpenSession())
                {
                    /*session.Create(new Movie
                    {
                        Id = "1",
                        Title = "test",
                        TagLine = "hello"
                    });
                    session.SaveChanges();*/
                    var movie = session.Get<Movie>("1");
                    
                }

                Console.WriteLine(DateTime.UtcNow.Subtract(start));
                Console.ReadLine();
            }
        }
    }
}