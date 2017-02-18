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


            var id1 = new MovieId("rWJ-ibil2kOUqqcQAUs0uQ");
            var id2 = new MovieId("rWJ-ibil2kOUqqcQAUs0uQ");

            var list = new List<Identity>() {id1, id2};
            var contains = list.Contains(id1);
            contains = list.Contains(id2);

            var result = id1.Equals(id2);
            result = id1 == id2;

            var start = DateTime.UtcNow;
            using (var session = factory.OpenSession())
            {
                var movie = session.Get<Movie>("new-movie-1");

                var ids = movie.People.Select(x => (object)x.Id.Value).ToArray();

                var people = session.Get<Person>(ids);

                
                //session.SaveChanges();
            }
            Console.WriteLine(DateTime.UtcNow.Subtract(start));
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}