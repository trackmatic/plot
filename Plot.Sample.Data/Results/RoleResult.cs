using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{

    public class RoleResult : AbstractCypherQueryResult<Role>
    {
        public RoleNode Role { get; set; }

        public override void Map(Role aggregate)
        {
            aggregate.Name = Role.Name;
        }

        public override Role Create()
        {
            return Role.AsRole();
        }
    }
}
