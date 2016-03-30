using System.Collections.Generic;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{

    public class PersonResult : AbstractCypherQueryResult<Person>
    {
        public PersonNode Person { get; set; }

        public UserNode User { get; set; }

        public CrewNode Crew { get; set; }

        public DriverNode Driver { get; set; }

        public IEnumerable<SiteNode> Sites { get; set; }

        public IEnumerable<OrganisationNode> Organisations { get; set; }

        public override void Map(Person aggregate)
        {
            Organisations.Map(x => aggregate.Add(x.AsOrganisation()));
            Sites.Map(x => aggregate.Add(x.AsSite()));
            aggregate.User = User?.AsUser();
            aggregate.Driver = Driver?.AsDriver();
            aggregate.Crew = Crew?.AsCrew();
        }

        public override Person Create()
        {
            return Person.AsPerson();
        }
    }
}
