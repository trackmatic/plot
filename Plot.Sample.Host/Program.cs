using System;
using System.Collections.Generic;
using Plot.Neo4j;
using Plot.Sample.Data.Mappers;
using Plot.Sample.Model;
using Plot.Sample.Queries;

namespace Plot.Sample.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var uri = new Uri("http://localhost:7474/db/data");
            var factory = Configuration.CreateGraphSessionFactory(uri, "neo4j", "trackmatic", typeof(UserMapper).Assembly);
            //try
            //{
                using (var session = factory.OpenSession())
                {
                    var data = session.Get(new GetUserByUsername {Username = "ross"}, false);

                    var organisation = session.Create(new Organisation
                    {
                        Id = "org",
                        Name = "Trackmatic"
                    });

                    session.SaveChanges();

                    var site = session.Create(new Site
                    {
                        Id = "site",
                        Name = "Site"
                    });
                    organisation.Add(site);
                    session.SaveChanges();

                    var person = session.Create(new Person
                    {
                        Id = "person",
                        Names = new Names
                        {
                            First = "Ross",
                            Last = "Jones"
                        }
                    });
                    site.Add(person);
                    session.SaveChanges();

                    var accessGroup = session.Create(new AccessGroup
                    {
                        Id = "accessGroup",
                        Name = "Access Group"
                    });
                    organisation.Add(accessGroup);
                    organisation.Add(person);
                    session.SaveChanges();

                    var role = session.Create(new Role
                    {
                        Id = "administrator",
                        Name = "Administrator"
                    });
                    session.SaveChanges();

                    var module = session.Create(new Module
                    {
                        Id = "module",
                        Name = "Module"
                    });
                    session.SaveChanges();

                    var sitePermission = session.Create(new SitePermission
                    {
                        Id = "sitePermission"
                    });
                    sitePermission.Site = organisation.Sites[0];
                    sitePermission.AccessGroups.Add(organisation.AccessGroups[0]);
                    session.SaveChanges();

                    var modulePermission = session.Create(new ModulePermission
                    {
                        Id = "modulePermission",
                        Roles = new List<Role> {role},
                        Sites = new List<SitePermission> {sitePermission},
                        //Module = module
                    });
                    modulePermission.Module = module;
                    session.SaveChanges();

                    var user = session.Create(new User
                    {
                        Id = "user",
                        Person = person,
                        Username = "ross"
                    });
                    person.User = user;
                    person.Set(user);
                    user.Add(modulePermission);
                    session.SaveChanges();
                }

            /*}
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }*/

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}