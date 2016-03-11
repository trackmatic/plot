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

                var role = session.Create(new Role
                {
                    Id = "administrator",
                    Name = "Administrator"
                });

                var module = session.Create(new Module
                {
                    Id = "module",
                    Name = "Module"
                });

                var organisation = session.Create(new Organisation
                {
                    Id = "1",
                    Name = "Trackmatic"
                });

                var site = session.Create(new Site
                {
                    Id = "1",
                    Name = "Site"
                });
                site.Add(person);

                organisation.Add(site);


                var accessGroup = session.Create(new AccessGroup
                {
                    Id = "1",
                    Name = "Access Group"
                });

                organisation.Add(accessGroup);

                organisation.Add(person);

                var sitePermission = session.Create(new SitePermission
                {
                    Id = "1"
                });

                sitePermission.Site = organisation.Sites[0];
                sitePermission.AccessGroups.Add(organisation.AccessGroups[0]);

                var modulePermission = session.Create(new ModulePermission
                {
                    Id = "1",
                    Roles = new List<Role> {  role },
                });

                modulePermission.Add(sitePermission);
                modulePermission.Module = module;
                modulePermission.Add(role);

                var user = session.Create(new User
                {
                    Id = "1",
                    Person = person,
                    Username = "ross"
                });

                person.User = user;

                //person.Set(user);

                user.Add(modulePermission);

                session.SaveChanges();

            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}