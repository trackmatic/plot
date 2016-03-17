namespace Plot.Sample.Data.Nodes
{
    public class AssetNode
    {
        public AssetNode()
        {
            
        }

        public AssetNode(Asset item)
        {
            Id = item.Id;
            FleetNumber = item.FleetNumber;
        }

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
