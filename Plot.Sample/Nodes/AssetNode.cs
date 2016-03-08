using Plot.Sample.Model;

namespace Plot.Sample.Nodes
{
    public class AssetNode
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Asset AsAsset()
        {
            var asset = new Asset
            {
                Id = Id,
                Name = Name
            };
            return asset;
        }
    }
}
