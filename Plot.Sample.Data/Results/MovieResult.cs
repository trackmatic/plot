using System;
using System.Collections.Generic;
using Neo4j.Driver.V1;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{

    public class MovieResult : AbstractCypherQueryResult<Movie>
    {
        public MovieResult(IRecord record)
        {
            Movie = new MovieNode(record["Movie"].As<INode>());
        }

        public MovieNode Movie { get; set; }

        public IEnumerable<PersonNode> People { get; set; }


        public override void Map(Movie aggregate)
        {

        }

        public override Movie Create()
        {
            return Movie.AsMovie();
        }
    }
}
