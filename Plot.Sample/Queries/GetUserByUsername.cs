using Plot.Queries;
using Plot.Sample.Model;

namespace Plot.Sample.Queries
{
    public class GetUserByUsername : AbstractQuery<User>
    {
        public GetUserByUsername()
        {
            OrderBy = new[] { "user.Username" };
        }

        public string Username { get; set; }
    }
}
