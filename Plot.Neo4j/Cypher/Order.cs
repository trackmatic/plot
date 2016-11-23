namespace Plot.Neo4j.Cypher
{
    public class Order<TResult>
    {
        public Order(string value)
        {
            Descending = value.StartsWith("-");
            Property = value.Trim('-');
        }

        private string Property { get; }

        private bool Descending { get; }

        public ICypherQuery<TResult> Start(ICypherQuery<TResult> cypher)
        {
            return Descending ? cypher.OrderByDescending(Property) : cypher.OrderBy(Property);
        }

        public ICypherQuery<TResult> Continue(ICypherQuery<TResult> cypher)
        {
            return Descending ? cypher.OrderByDescending(Property) : cypher.OrderBy(Property);
        }
    }
}
