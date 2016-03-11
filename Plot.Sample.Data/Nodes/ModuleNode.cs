using Plot.Sample.Model;

namespace Plot.Sample.Data.Nodes
{
    public class ModuleNode
    {
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
