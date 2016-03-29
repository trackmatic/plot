using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{
    public class TrailerResult : AbstractCypherQueryResult<Trailer>
    {
        public TrailerNode Trailer { get; set; }

        public override void Map(Trailer aggregate)
        {

        }

        public override Trailer Create()
        {
            var contact = Trailer.AsTrailer();
            return contact;
        }
    }
}
