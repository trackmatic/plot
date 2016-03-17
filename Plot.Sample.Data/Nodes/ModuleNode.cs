namespace Plot.Sample.Data.Nodes
{
    public class ModuleNode
    {
        public ModuleNode()
        {
            
        }

        public ModuleNode(Module item)
        {
            Id = item.Id;
            Name = item.Name;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public Module AsModule()
        {
            return new Module
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
