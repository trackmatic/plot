namespace Plot.N4j
{
    public interface ICypherTransactionFactory
    {
        ICypherTransaction Create(IGraphSession session);
    }
}
