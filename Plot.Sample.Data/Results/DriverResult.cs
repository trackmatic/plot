using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{
    public class DriverResult : AbstractCypherQueryResult<Driver>
    {
        public DriverNode Driver { get; set; }

        public override void Map(Driver aggregate)
        {

        }

        public override Driver Create()
        {
            var item = Driver.AsDriver();
            return item;
        }
    }
}
