using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{
    public class ForkliftResult : AbstractCypherQueryResult<Forklift>
    {
        public ForkliftNode Forklift { get; set; }

        public override void Map(Forklift aggregate)
        {

        }

        public override Forklift Create()
        {
            var contact = Forklift.Asforklift();
            return contact;
        }
    }
}
