using Neo4jClient;
using Octo.Core;

namespace Octo.N4j
{
    public class CypherTransactionFactory : ICypherTransactionFactory
    {
        private readonly IGraphClient _db;

        public CypherTransactionFactory(IGraphClient db)
        {
            _db = db;
        }

        public ICypherTransaction Create(IGraphSession session)
        {
            return new CypherTransaction(_db);
        }
    }
}
