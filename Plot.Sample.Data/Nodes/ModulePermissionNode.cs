namespace Plot.Sample.Data.Nodes
{
    public class ModulePermissionNode
    {
        public ModulePermissionNode()
        {
            
        }

        public ModulePermissionNode(ModulePermission item)
        {
            Id = item.Id;
        }

        public string Id { get; set; }

        public ModulePermission AsModulePermission()
        {
            return new ModulePermission
            {
                Id = Id
            };
        }
    }
}
