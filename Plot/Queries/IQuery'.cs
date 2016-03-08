namespace Plot.Queries
{

    public interface IQuery<TResult> : IQuery
    {
        IQuery<TResult> Next();
    }
}
