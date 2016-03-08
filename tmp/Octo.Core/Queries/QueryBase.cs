using Octo.Core.Artifacts;

namespace Octo.Core.Queries
{
    public class QueryBase<TResult> : IQuery<TResult>
    {
        public QueryBase()
        {
            Take = 128;
        }

        public int Take { get; set; }

        public int Skip { get; set; }

        public string[] OrderBy { get; set; }

        public bool Descending { get; set; }

        public IQuery<TResult> Next()
        {
            var clone = Cloner.Clone(this);

            clone.Skip += Take;

            return clone;
        }
    }
}
