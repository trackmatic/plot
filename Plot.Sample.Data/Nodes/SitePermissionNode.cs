using System.Collections.Generic;
using Plot.Sample.Model;

namespace Plot.Sample.Data.Nodes
{
    public class SitePermissionNode
    {
        public string Id { get; set; }

        public List<SiteNode> Sites { get; set; }

        public List<AccessGroupNode> AccessGroups { get; set; }

        public SitePermission AsSitePermission()
        {
            return new SitePermission
            {
                Id = Id
            };
        }
    }
}
