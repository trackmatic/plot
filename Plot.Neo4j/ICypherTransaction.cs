using System;
using System.Collections.Generic;
using Plot.Neo4j.Cypher;

namespace Plot.Neo4j
{
    public interface ICypherTransaction : IDisposable
    {
        void Commit();
        void Enlist(IMapper mapper, Func<ICypherFluentQuery, ICypherFluentQuery> callback);
    }
}
