using Plot.Sample.Model;

namespace Plot.Sample.Data.Nodes
{
    public class AccessGroupNode
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public AccessGroup AsAccessGroup()
        {
            return new AccessGroup
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
