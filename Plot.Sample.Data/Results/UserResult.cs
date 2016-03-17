using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;
using Plot.Sample.Model;

namespace Plot.Sample.Data.Results
{
    public class UserResult : AbstractCypherQueryResult<User>
    {
        public UserNode User { get; set; }

        public override void Map(User aggregate)
        {

        }

        public override User Create()
        {
            return User.AsUser();
        }
    }
}
