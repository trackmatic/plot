namespace Plot.Neo4j.Cypher
{
    public class Return
    {
        public Return(string property, string alias)
        {
            Property = property;
            Alias = alias;
        }

        public string Property { get; set; }
        public string Alias { get; set; }
    }
}