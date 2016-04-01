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
            var uri = new Uri("http://localhost:7474/db/data");
            Configuration.Logger = () => new ConsoleLogger();
            var factory = Configuration.CreateGraphSessionFactory(uri, "neo4j", "trackmatic", typeof (UserMapper).Assembly);
            using (var session = factory.OpenSession())
            {
                var id = Guid.NewGuid().ToString();
                var person = session.Create(new Person
                {
                    Id = id,
                    Names = new Names(),
                    Numbers = new Numbers(),
                    Email = "",
                    Title = "",
                    Gender = "",
                    Position = "",
                    Department = "",
                    IdentityNumber = ""
                });
                var user = session.Create(new User
                {
                    Id = id,
                    Username = "ross"
                });
                var request = session.Create(new ResetPasswordRequest());
                request.RequestedBy = user;
                user.Add(request);
                person.User = user;
                session.SaveChanges();
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}