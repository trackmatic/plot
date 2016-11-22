using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{

    public class PersonResult : AbstractCypherQueryResult<Person>
    {
        public PersonNode Person { get; set; }
        
        public override void Map(Person aggregate)
        {
        }

        public override Person Create()
        {
            return Person.AsPerson();
        }
    }
}
