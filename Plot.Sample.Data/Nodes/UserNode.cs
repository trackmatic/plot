namespace Plot.Sample.Data.Nodes
{
    public class UserNode
    {
        public UserNode()
        {
            
        }

        public UserNode(User user)
        {
            Id = user.Id;
            Username = user.Username;
        }

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
