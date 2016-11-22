namespace Plot.Neo4j.Cypher
{
    public interface IQueryBuilder
    {
        ICypherFluentQuery Build(ICypherFluentQuery db);
    }
}
