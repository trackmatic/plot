namespace Plot.Proxies
{
    public interface IProxyFactory
    {
        T Create<T>(T item, IGraphSession session, EntityStatus status = EntityStatus.Clean) where T : class;
    }
}
