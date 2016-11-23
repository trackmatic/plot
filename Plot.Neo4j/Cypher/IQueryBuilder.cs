namespace Plot.Neo4j.Cypher
{
    public interface IQueryBuilder
    {
        ICypherQuery Build(ICypherQuery db);
    }
}
