using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{
    public class PasswordResult : AbstractCypherQueryResult<Password>
    {
        public UserNode User { get; set; }

        public PasswordNode Password { get; set; }
        
        public override void Map(Password aggregate)
        {
            aggregate.User = User.AsUser();
        }

        public override Password Create()
        {
            return Password.AsPassword();
        }
    }
}