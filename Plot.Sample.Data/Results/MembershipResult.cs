using System.Collections.Generic;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{

    public class MembershipResult : AbstractCypherQueryResult<Membership>
    {
        public MembershipNode Membership { get; set; }

        public OrganisationNode Organisation { get; set; }

        public IEnumerable<AccessGroupNode> AccessGroups { get; set; }

        public IEnumerable<ModulePermissionNode> ModulePermissions { get; set; }
        public override void Map(Membership aggregate)
        {
            aggregate.Organisation = Organisation.AsOrganisation();
            ModulePermissions.Map(x => aggregate.Add(x.AsModulePermission()));
            AccessGroups.Map(x => aggregate.Add(x.AsAccessGroup()));
        }

        public override Membership Create()
        {
            var contact = Membership.AsMembership();
            return contact;
        }
    }
}
