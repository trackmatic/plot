using System.Collections.Generic;
using System.Linq;
using Plot.Queries;

namespace Plot.N4j.Queries
{
    public class GetAbstractQuery<TResult> : AbstractQuery<TResult>
    {
        public GetAbstractQuery(IEnumerable<object> items)
        {
            Id = items.Select(x => ProxyUtils.GetState(x).GetIdentifier()).ToArray();
        }

        public GetAbstractQuery(string[] id)
        {
            Id = id;
        }

        public string[] Id { get; set; }
    }
}