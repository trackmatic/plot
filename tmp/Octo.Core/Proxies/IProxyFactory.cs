namespace Octo.Core.Proxies
{
    public interface IProxyFactory
    {
        T Create<T>(T item, IGraphSession session) where T : class;
    }
}
