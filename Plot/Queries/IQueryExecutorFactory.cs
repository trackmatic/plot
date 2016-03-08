namespace Plot.Queries
{
    public interface IQueryExecutorFactory
    {
        IQueryExecutor<TResult> Create<TResult>(IQuery<TResult> query, IGraphSession session);
    }
}
