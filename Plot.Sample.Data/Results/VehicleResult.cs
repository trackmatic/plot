using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{
    public class VehicleResult : AbstractCypherQueryResult<Vehicle>
    {
        public VehicleNode Vehicle { get; set; }

        public override void Map(Vehicle aggregate)
        {

        }

        public override Vehicle Create()
        {
            var contact = Vehicle.AsVehicle();
            return contact;
        }
    }
}
