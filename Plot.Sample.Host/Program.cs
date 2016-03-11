using System;
using System.Collections.Generic;
using Plot.Neo4j;
using Plot.Sample.Data.Mappers;
using Plot.Sample.Model;

namespace Plot.Sample.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var uri = new Uri("http://neo4j:trackmatic@localhost:7474/db/data");
            var factory = Configuration.CreateGraphSessionFactory(uri, typeof(OrganisationMapper).Assembly);
            using (var session = factory.OpenSession())
            {
                var person = session.Create(new Person
                {
                    Id = "1",
                    Names = new Names
                    {
                        First = "Ross",
                        Last = "Jones"
                    }
                });

                var role = session.Get<Role>("administrator");

                var module = session.Get<Module>("organisation");

                var organisation = session.Get<Organisation>("1");

                organisation.Add(person);

                var sitePermission = session.Create(new SitePermission
                {
                    Id = "1",
                    Site = organisation.Sites[0],
                    AccessGroups = new List<AccessGroup> {organisation.AccessGroups[0]}
                });

                var modulePermission = session.Create(new ModulePermission
                {
                    Id = "1",

                    Roles = new List<Role> {  role },
                    Module = module,
                    Sites = new List<SitePermission> { sitePermission }
                });

                var user = session.Create(new User
                {
                    Id = "1",
                    Person = person,
                    Username = "ross"
                });

                //person.User = user;

                person.Set(user);

                user.Add(modulePermission);

                session.SaveChanges();

            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}