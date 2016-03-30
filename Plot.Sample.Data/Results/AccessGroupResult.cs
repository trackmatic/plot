using System.Collections.Generic;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{

    public class AccessGroupResult : AbstractCypherQueryResult<AccessGroup>
    {
        public IEnumerable<AssetNode> Assets { get; set; }

        public AccessGroupNode AccessGroup { get; set; }

        public IEnumerable<ModulePermissionNode> ModulePermissions { get; set; }

        public override void Map(AccessGroup aggregate)
        {
            aggregate.Name = AccessGroup.Name;
            foreach (var node in Assets)
            {
                aggregate.Add(node.AsAsset());
            }
            Assets.Map(x => aggregate.Add(x.AsAsset()));
            ModulePermissions?.Map(x => aggregate.Add(x.AsModulePermission()));
        }

        public override AccessGroup Create()
        {
            return AccessGroup.AsAccessGroup();
        }
    }
}
