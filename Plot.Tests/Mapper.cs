using System;
using System.Collections.Generic;
using System.Linq;
using Plot.Metadata;
using Plot.Queries;

namespace Plot.Tests
{
    public class Mapper<T> : IMapper<T>
    {
        private readonly IGraphSession _session;
        
        public Mapper(IGraphSession session, IMetadataFactory metadataFactory)
        {
            _session = session;
            MetadataFactory = metadataFactory;
        }

        public void Insert(T item, EntityState state)
        {
        }

        public void Delete(T item, EntityState state)
        {
        }

        public void Update(T item, EntityState state)
        {
        }

        public void Insert(object item, EntityState state)
        {
            Insert((T) item, state);
        }

        public void Delete(object item, EntityState state)
        {
            Delete((T) item, state);
        }

        public void Update(object item, EntityState state)
        {
            Update((T) item, state);
        }

        public IEnumerable<T> Get(string[] id)
        {
            var items = OnGet(id).ToList();
            return items;
        }

        public Type Type => typeof(T);

        protected object GetData(T item)
        {
            return null;
        }

        protected IQueryExecutor<T> CreateQueryExecutor()
        {
            return null;
        }

        protected IGraphSession Session => _session;
        
        private IList<T> OnGet(params string[] id)
        {
            return null;
        }

        protected IMetadataFactory MetadataFactory { get; }
    }
}
