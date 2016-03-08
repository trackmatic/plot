namespace Plot.Neo4j
{
    public interface ICypherTransactionFactory
    {
        ICypherTransaction Create(IGraphSession session);
    }
}
