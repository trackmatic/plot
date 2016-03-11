using Plot.Sample.Model;

namespace Plot.Sample.Data.Nodes
{
    public class RoleNode
    {
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
