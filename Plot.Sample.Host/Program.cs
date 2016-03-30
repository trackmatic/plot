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
                var person = session.Get<Person>("0bae303d-2ac1-4942-a455-4a87caa4205b");
                session.Get<User>(person.User.Id);
                session.Get<Membership>(person.User.Memberships.Select(x => x.Id).ToArray());
                session.Get<ModulePermission>(person.User.Memberships.SelectMany(x => x.ModulePermissions).Select(x => x.Id).ToArray());

                session.Get<User>(person.User.Id);
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}