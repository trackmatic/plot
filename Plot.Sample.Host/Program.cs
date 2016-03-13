using System;
using System.Collections.Generic;
using System.Linq;
using Neo4jClient;
using Plot.Metadata;
using Plot.Neo4j;
using Plot.Neo4j.Queries;
using Plot.Proxies;
using Plot.Sample.Data.Mappers;
using Plot.Sample.Model;

namespace Plot.Sample.Host
{
    internal class Program
    {
        private static void Print(IEntityStateCache stateTracker)
        {
            Console.WriteLine("*****************************");
            var output = string.Join("\r\n", stateTracker.State.Select(x => x.GetIdentifier() + ":" + x.Dependencies.Sequence).ToList());
            Console.WriteLine(output);
            Console.WriteLine("*****************************");
        }

        private static void Main(string[] args)
        {
            var uri = new Uri("http://neo4j:trackmatic@localhost:7474/db/data");
            var db = new GraphClient(uri);
            db.Connect();
            var entityStateFactory = new EntityStateCacheFactory();
            var metadataFactory = new AttributeMetadataFactory();
            var proxyFactory = new DynamicProxyFactory(metadataFactory);
            var transactionFactory = new CypherTransactionFactory(db);
            var repositoryFactory = new RepositoryFactory(db, transactionFactory, proxyFactory, metadataFactory, typeof(OrganisationMapper).Assembly);
            var queryExecutorFactory = new QueryExecutorFactory(db, typeof(OrganisationMapper).Assembly);
            var factory = new GraphSessionFactory(queryExecutorFactory, repositoryFactory, entityStateFactory);

            using (var session = factory.OpenSession())
            {
                Print(session.State);
                var organisation = session.Create(new Organisation
                {
                    Id = "org",
                    Name = "Trackmatic"
                });
                Print(session.State);

                var site = session.Create(new Site
                {
                    Id = "site",
                    Name = "Site"
                });
                organisation.Add(site);
                Print(session.State);

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
                Print(session.State);

                var accessGroup = session.Create(new AccessGroup
                {
                    Id = "ag",
                    Name = "Access Group"
                });
                organisation.Add(accessGroup);
                Print(session.State);
                organisation.Add(person);
                Print(session.State);

                /*var role = session.Create(new Role
                {
                    Id = "administrator",
                    Name = "Administrator"
                });

                var module = session.Create(new Module
                {
                    Id = "module",
                    Name = "Module"
                });

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
                    Sites = new List<SitePermission> {  sitePermission},
                    //Module = module
                });

                modulePermission.Module = module;

                var user = session.Create(new User
                {
                    Id = "1",
                    Person = person,
                    Username = "ross"
                });

                person.User = user;

                person.Set(user);

                user.Add(modulePermission);*/

                session.SaveChanges();

            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}