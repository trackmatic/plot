using System.Collections.Generic;
using Plot.Sample.Model;

namespace Plot.Sample.Data.Nodes
{
    class ModulePermissionNode
    {
        public string Id { get; set; }

        public ModuleNode Module { get; set; }

        public List<RoleNode> Roles { get; set; }

        public List<SitePermissionNode> Sites { get; set; }

        public Module AsModulePermission()
        {
            return new Module
            {
                Id = Id
            };
        }
    }
}
