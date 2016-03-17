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
                var request = session.Get<ResetPasswordRequest>("bdb03431-a315-4fc3-97c5-785afe9833eb");
                var password = session.Create(Password.Create("test"));
                var user = session.Get<User>(request.User.Id);
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}