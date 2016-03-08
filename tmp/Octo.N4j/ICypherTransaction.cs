using System;
using Neo4jClient.Cypher;
using Octo.Core;

namespace Octo.N4j
{
    public interface ICypherTransaction
    {
        void Commit();

        void Enlist(IMapper mapper, Func<ICypherFluentQuery, ICypherFluentQuery> callback);
    }
}
