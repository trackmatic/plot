using System.Collections.Generic;
using System.Linq;

namespace Octo.Core.Queries
{
    public class GetQuery<TResult> : QueryBase<TResult>
    {
        public GetQuery(IEnumerable<object> items)
        {
            Id = items.Select(x => EntityStateTracker.Get(x).GetIdentifier()).ToArray();
        }

        public GetQuery(string[] id)
        {
            Id = id;
        }

        public string[] Id { get; set; }
    }
}