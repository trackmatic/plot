using Plot.Sample.Model;

namespace Plot.Sample.Data.Nodes
{
    public class AssetNode
    {
        public string Id { get; set; }

        public string FleetNumber { get; set; }

        public Asset AsAsset()
        {
            var asset = new Asset
            {
                Id = Id,
                FleetNumber = FleetNumber
            };
            return asset;
        }
    }
}
