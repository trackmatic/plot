using System.Collections.Generic;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{

    public class OrganisationResult : AbstractCypherQueryResult<Organisation>
    {
        public OrganisationNode Organisation { get; set; }

        public IEnumerable<SiteNode> Sites { get; set; }

        public AddressNode Address { get; set; }

        public IEnumerable<AccessGroupNode> AccessGroups { get; set; }


        public override void Map(Organisation aggregate)
        {
            Sites.Map(x => aggregate.Add(x.AsSite()));
            AccessGroups.Map(x => aggregate.Add(x.AsAccessGroup()));
            aggregate.Address = Address?.AsAddress();
        }

        public override Organisation Create()
        {
            return Organisation.AsOrganisation();
        }
    }
}
