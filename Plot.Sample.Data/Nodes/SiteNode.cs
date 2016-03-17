namespace Plot.Sample.Data.Nodes
{
    public class SiteNode
    {
        public SiteNode()
        {
            
        }

        public SiteNode(Site item)
        {
            Id = item.Id;
            Name = item.Name;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public Site AsSite()
        {
            var site = new Site
            {
                Id = Id,
                Name = Name
            };
            return site;

        }
    }
}
