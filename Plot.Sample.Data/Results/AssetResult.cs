using System.Collections.Generic;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{

    public class AssetResult : AbstractCypherQueryResult<Asset>
    {
        public IEnumerable<SiteNode> Sites { get; set; }

        public IEnumerable<AccessGroupNode> AccessGroups { get; set; }

        public AssetNode Asset { get; set; }

        public VehicleNode Vehicle { get; set; }

        public TrailerNode Trailer { get; set; }

        public ForkliftNode Forklift { get; set; }

        public override void Map(Asset aggregate)
        {
            aggregate.Type = GetAssetType();
            Sites?.Map(x => aggregate.Add(x.AsSite()));
            AccessGroups?.Map(x => aggregate.Add(x.AsAccessGroup()));
        }

        private AssetType GetAssetType()
        {
            if (Vehicle != null)
            {
                return Vehicle.AsVehicle();
            }

            if (Trailer != null)
            {
                return Trailer.AsTrailer();
            }

            return Forklift?.Asforklift();
        }

        public override Asset Create()
        {
            var contact = Asset.AsAsset();
            return contact;
        }
    }
}
