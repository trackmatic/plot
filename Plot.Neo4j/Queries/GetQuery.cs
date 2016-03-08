using Plot.Queries;

namespace Plot.Neo4j.Queries
{
    public class GetAbstractQuery<TResult> : AbstractQuery<TResult>
    {
        public GetAbstractQuery(string[] id)
        {
            Id = id;
        }

        public string[] Id { get; set; }
    }
}