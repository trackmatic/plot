using Octo.Sample.Model;

namespace Octo.Sample.Nodes
{
    public class SiteNode
    {
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
