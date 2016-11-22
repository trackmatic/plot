using System.Collections.Generic;
using Plot.Neo4j.Cypher;

namespace Plot.Neo4j
{
    public interface ICypherTransactionFactory
    {
        ICypherTransaction Create(IGraphSession session);
        IList<T> Run<T>(ICypherFluentQuery<T> query);
    }
}
