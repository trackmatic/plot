using Octo.Core;

namespace Octo.N4j
{
    public interface ICypherTransactionFactory
    {
        ICypherTransaction Create(IGraphSession session);
    }
}
