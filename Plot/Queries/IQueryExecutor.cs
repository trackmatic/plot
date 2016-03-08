using System;

namespace Plot.Queries
{
    public interface IQueryExecutor
    {
        Type QueryType { get; }
    }
}
