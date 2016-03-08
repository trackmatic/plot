using Octo.Core.Proxies;

namespace Octo.Core
{
    public class GenericAbstractRepository<T> : AbstractRepository<T> where T : class
    {
        public GenericAbstractRepository(IMapper<T> mapper, IGraphSession session, IProxyFactory proxyFactory) : base(mapper, session, proxyFactory)
        {

        }
    }
}
