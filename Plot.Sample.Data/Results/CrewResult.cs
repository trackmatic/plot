using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{
    public class CrewResult : AbstractCypherQueryResult<Crew>
    {
        public CrewNode Crew { get; set; }

        public override void Map(Crew aggregate)
        {

        }

        public override Crew Create()
        {
            var item = Crew.AsCrew();
            return item;
        }
    }
}
