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
                var request = session.Get<ResetPasswordRequest>("f15fab5a-f398-41f1-a46f-21e721d01891");
                var password = session.Create(Password.Create("a test"));
                var user = session.Get<User>(request.User.Id);
                user.Set(password);
                request.Complete(password);
                session.SaveChanges();
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}