using System.Collections.Generic;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{

    public class ModulePermissionResult : AbstractCypherQueryResult<ModulePermission>
    {
        public ModulePermissionNode ModulePermission { get; set; }

        public IEnumerable<SiteNode> Sites { get; set; }

        public IEnumerable<RoleNode> Roles { get; set; }

        public IEnumerable<AccessGroupNode> AccessGroups { get; set; }

        public ModuleNode Module { get; set; }

        public MembershipNode Membership { get; set; }

        public IEnumerable<ModulePermissionNode> ModulePermissions { get; set; }

        public UserNode User { get; set; }

        public PersonNode Person { get; set; }

        public override void Map(ModulePermission aggregate)
        {
            Sites.Map(x => aggregate.Add(x.AsSite()));
            AccessGroups.Map(x => aggregate.Add(x.AsAccessGroup()));
            Roles.Map(x => aggregate.Add(x.AsRole()));
            aggregate.Membership = Membership.AsMembership();
            aggregate.Module = Module.AsModule();
            ModulePermissions?.Map(x => aggregate.Membership.Add(x.AsModulePermission()));
            aggregate.Membership.User = User?.AsUser();
            if (aggregate.Membership.User != null)
            {
                aggregate.Membership.User.Person = Person?.AsPerson();
            }
        }

        public override ModulePermission Create()
        {
            var contact = ModulePermission.AsModulePermission();
            return contact;
        }
    }
}
