namespace Plot.Neo4j.Cypher
{
    public interface IQueryBuilderElement
    {
        ICypherQuery Append(ICypherQuery cypher);
    }
}
