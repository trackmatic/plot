using Neo4jClient.Cypher;

namespace Plot.Neo4j.Cypher
{
    public class Order<TResult>
    {
        public Order(string value)
        {
            Descending = value.StartsWith("-");
            Property = value.Trim('-');
        }

        private string Property { get; set; }

        private bool Descending { get; set; }

        public ICypherFluentQuery<TResult> Start(ICypherFluentQuery<TResult> cypher)
        {
            return Descending ? cypher.OrderByDescending(Property) : cypher.OrderBy(Property);
        }

        public ICypherFluentQuery<TResult> Continue(IOrderedCypherFluentQuery<TResult> cypher)
        {
            return Descending ? cypher.ThenByDescending(Property) : cypher.ThenBy(Property);
        }
    }
}
