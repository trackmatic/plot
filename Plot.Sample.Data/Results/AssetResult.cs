using System.Collections.Generic;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;
using Plot.Sample.Model;

namespace Plot.Sample.Data.Results
{
    public class AssetResult : AbstractCypherQueryResult<Asset>
    {
        public IEnumerable<SiteNode> Sites { get; set; }

        public AssetNode Asset { get; set; }

        public override void Map(Asset aggregate)
        {
            aggregate.FleetNumber = Asset.FleetNumber;
            foreach (var site in Sites)
            {
                aggregate.Add(site.AsSite());
            }
        }

        public override Asset Create()
        {
            var contact = Asset.AsAsset();
            return contact;
        }
    }
}
