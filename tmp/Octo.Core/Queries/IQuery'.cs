namespace Octo.Core.Queries
{

    public interface IQuery<TResult> : IQuery
    {
        IQuery<TResult> Next();
    }
}
