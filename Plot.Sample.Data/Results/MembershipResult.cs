using System.Collections.Generic;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{

    public class MembershipResult : AbstractCypherQueryResult<Membership>
    {
        public MembershipNode Membership { get; set; }

        public OrganisationNode Organisation { get; set; }
        
        public UserNode CreatedBy { get; set; }

        public UserNode User { get; set; }

        public IEnumerable<ModulePermissionNode> ModulePermissions { get; set; }

        public override void Map(Membership aggregate)
        {
            aggregate.Organisation = Organisation.AsOrganisation();
            aggregate.CreatedBy = CreatedBy?.AsUser();
            aggregate.User = User?.AsUser();
            ModulePermissions.Map(x => aggregate.Add(x.AsModulePermission()));
        }

        public override Membership Create()
        {
            var contact = Membership.AsMembership();
            return contact;
        }
    }
}
