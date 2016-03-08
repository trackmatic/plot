namespace Octo.Core.Queries
{
    public interface IQueryExecutorFactory
    {
        IQueryExecutor<TResult> Create<TResult>(IQuery<TResult> query, IGraphSession session);
    }
}
