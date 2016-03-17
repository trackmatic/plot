using System.Collections.Generic;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{

    public class ModulePermissionResult : AbstractCypherQueryResult<ModulePermission>
    {
        public ModulePermissionNode ModulePermission { get; set; }

        public IEnumerable<SitePermissionNode> SitePermissions { get; set; }

        public IEnumerable<RoleNode> Roles { get; set; }

        public ModuleNode Module { get; set; }

        public MembershipNode Membership { get; set; }
        public override void Map(ModulePermission aggregate)
        {
            SitePermissions.Map(x => aggregate.Add(x.AsSitePermission()));
            Roles.Map(x => aggregate.Add(x.AsRole()));
            aggregate.Membership = Membership.AsMembership();
            aggregate.Module = Module.AsModule();
        }

        public override ModulePermission Create()
        {
            var contact = ModulePermission.AsModulePermission();
            return contact;
        }
    }
}
