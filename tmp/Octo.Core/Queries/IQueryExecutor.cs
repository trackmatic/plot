using System;
using System.Collections.Generic;

namespace Octo.Core.Queries
{
    public interface IQueryExecutor
    {
        Type QueryType { get; }
    }
}
