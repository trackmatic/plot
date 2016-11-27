using System.Collections.Generic;
using Neo4j.Driver.V1;
using Plot.Neo4j;
using Plot.Neo4j.Cypher;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{

    public class MovieResult : AbstractCypherQueryResult<Movie>
    {
        public MovieResult(IRecord record)
        {
            Movie = record.Read(Keys.Movie, node => new MovieNode(node));
            People = record.ReadList(Keys.People, node => new PersonNode(node));
            Total = record.Read<long>(Keys.Total);
        }

        public MovieNode Movie { get; set; }

        public IEnumerable<PersonNode> People { get; set; }
        
        public override void Map(Movie aggregate)
        {
            People.Map(x => aggregate.Add(x.AsPerson()));
        }

        public override Movie Create()
        {
            return Movie.AsMovie();
        }

        public static MovieResult Map(IRecord record)
        {
            return new MovieResult(record);
        }

        public static ICypherReturn<MovieResult> Return(ICypherReturn<MovieResult> builder)
        {
            return builder.Return("Movie").CollectDistinct("People");
        }

        public static ICypherReturn<MovieResult> ReturnWithTotal(ICypherReturn<MovieResult> builder)
        {
            return Return(builder).Return("Total");
        }

        public static class Keys
        {
            public const string Movie = "Movie";
            public const string People = "People";
            public const string Total = "Total";
        }
    }
}
