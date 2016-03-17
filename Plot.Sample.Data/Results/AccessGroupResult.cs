using System.Collections.Generic;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{

    public class AccessGroupResult : AbstractCypherQueryResult<AccessGroup>
    {
        public IEnumerable<AssetNode> Assets { get; set; }

        public AccessGroupNode AccessGroup { get; set; }

        public override void Map(AccessGroup aggregate)
        {
            aggregate.Name = AccessGroup.Name;
            foreach (var node in Assets)
            {
                aggregate.Add(node.AsAsset());
            }
        }

        public override AccessGroup Create()
        {
            return AccessGroup.AsAccessGroup();
        }
    }
}
