using System.Collections.Generic;
using System.Linq;
using Neo4j.Driver.V1;
using Plot.Neo4j.Cypher;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{

    public class PersonResult : AbstractCypherQueryResult<Person>
    {
        public PersonResult(IRecord record)
        {
            Person = new PersonNode(record[Keys.Person].As<INode>());
            Movies = record[Keys.Movies].As<List<INode>>().Select(x => new MovieNode(x));
        }

        public PersonNode Person { get; set; }
        public IEnumerable<MovieNode> Movies { get; set; }
        
        public override void Map(Person aggregate)
        {
            Movies.Map(x => aggregate.Add(x.AsMovie()));
        }

        public override Person Create()
        {
            return Person.AsPerson();
        }

        public static PersonResult Map(IRecord record)
        {
            return new PersonResult(record);
        }

        public static ICypherReturn<PersonResult> Return(ICypherReturn<PersonResult> builder)
        {
            return builder.Return("person", "Person").CollectDistinct("movies", "Movies");
        }

        private static class Keys
        {
            public const string Person = "Person";
            public const string Movies = "Movies";
        }
    }
}
