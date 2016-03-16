using System.Collections.Generic;
using System.Linq;
using Plot.Queries;

namespace Plot
{
    public class PagedGraphGraphCollection<T> : IPagedGraphCollection<T>
    {
        private readonly IQueryExecutor<T> _executor;
        
        private readonly IQuery<T> _query;

        private readonly IGraphSession _session;

        private readonly bool _enlist;

        public PagedGraphGraphCollection(IGraphSession session, IQueryExecutor<T> executor, IQuery<T> query, IEnumerable<T> data, int total, int page, bool enlist)
        {
            _session = session;
            _executor = executor;
            _query = query;
            Data = data;
            Total = total;
            Page = page;
        }

        public PagedGraphGraphCollection()
        {
            Data = new List<T>();
        }

        public IEnumerable<T> Data { get; set; }

        public int Total { get; set; }

        public int Page { get; set; }
        
        public IPagedGraphCollection<T> Next()
        {
            return _executor.ExecuteWithPaging(_session, _query.Next(), _enlist);
        }

        public bool IsEmpty
        {
            get
            {
                if (Data == null)
                {
                    return true;
                }
                return !Data.Any();
            }
        }

        public IEnumerable<T> Stream()
        {
            if (!IsEmpty)
            {
                foreach (var item in Data)
                {
                    yield return item;
                }

                foreach (var item in Next().Stream())
                {
                    yield return item;
                }
            }
        }


        public K Map<K>(System.Func<IPagedGraphCollection<T>, K> map)
        {
            return map(this);
        }
    }
}
