using System;
using Neo4jClient.Cypher;

namespace Plot.N4j
{
    public interface ICypherTransaction
    {
        void Commit();

        void Enlist(IMapper mapper, Func<ICypherFluentQuery, ICypherFluentQuery> callback);
    }
}
