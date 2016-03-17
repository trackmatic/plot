namespace Plot.Sample.Data.Nodes
{
    public class AccessGroupNode
    {
        public AccessGroupNode()
        {
            
        }

        public AccessGroupNode(AccessGroup item)
        {
            Id = item.Id;
            Name = item.Name;
        }

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
