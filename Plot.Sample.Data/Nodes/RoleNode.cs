namespace Plot.Sample.Data.Nodes
{
    public class RoleNode
    {
        public RoleNode()
        {
            
        }

        public RoleNode(Role role)
        {
            Id = role.Id;
            Name = role.Name;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public Role AsRole()
        {
            return new Role
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
