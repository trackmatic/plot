namespace Plot.Proxies
{
    public interface IProxyFactory
    {
        T Create<T>(T item, IGraphSession session, IEntityStateCache entityStateCache) where T : class;
    }
}
