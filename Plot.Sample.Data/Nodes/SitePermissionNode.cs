namespace Plot.Sample.Data.Nodes
{
    public class SitePermissionNode
    {
        public SitePermissionNode()
        {
            
        }

        public SitePermissionNode(SitePermission item)
        {
            Id = item.Id;
        }

        public string Id { get; set; }

        public SitePermission AsSitePermission()
        {
            return new SitePermission
            {
                Id = Id
            };
        }
    }
}
