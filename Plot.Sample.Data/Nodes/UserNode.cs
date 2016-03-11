using Plot.Sample.Model;

namespace Plot.Sample.Data.Nodes
{
    public class UserNode
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public User AsUser()
        {
            return new User
            {
                Id = Id,
                Username = Username
            };
        }
    }
}
