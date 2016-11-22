using Neo4j.Driver.V1;
using Plot.Neo4j.Cypher;

namespace Plot.Sample.Data.Results
{
    public static class ResultFactory
    {
        public static MovieResult CreateMovieResult(IRecord record)
        {
            return new MovieResult(record);
        }
        public static ICypherReturn<MovieResult> CreateMovieResultReturn(ICypherReturn<MovieResult> builder)
        {
            return builder.Return("movie", "Movie").CollectDistinct("people", "People");
        }
    }
}
