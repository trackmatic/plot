using System.Collections.Generic;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;
using Plot.Sample.Model;

namespace Plot.Sample.Data.Results
{
    public class SiteResult : AbstractCypherQueryResult<Site>
    {
        public SiteNode Site { get; set; }

        public OrganisationNode Organisation { get; set; }

        public IEnumerable<AssetNode> Assets { get; set; }

        public override void Map(Site aggregate)
        {
            aggregate.Name = Site.Name;
            aggregate.Set(Organisation?.AsOrganisation());
            foreach (var node in Assets)
            {
                aggregate.Add(node.AsAsset());
            }
        }

        public override Site Create()
        {
            return Site.AsSite();
        }
    }
}
