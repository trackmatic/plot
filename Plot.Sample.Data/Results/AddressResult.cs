using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{

    public class AddressResult : AbstractCypherQueryResult<Address>
    {
        public AddressNode Address { get; set; }
        public override void Map(Address aggregate)
        {
        }

        public override Address Create()
        {
            return Address.AsAddress();
        }
    }
}
