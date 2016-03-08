using System;
using System.Collections.Generic;
using System.Linq;
using Neo4jClient;
using Neo4jClient.Cypher;
using Octo.Core;

namespace Octo.N4j
{
    public abstract class MapperBase<T> : IMapper<T>
    {
        private readonly IGraphSession _session;

        private readonly ICypherTransactionFactory _transactionFactory;

        protected MapperBase(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory)
        {
            Db = db;
            _session = session;
            _transactionFactory = transactionFactory;
        }

        public void Insert(T item, EntityState state)
        {
            Execute(item, OnInsert);
        }

        public void Delete(T item, EntityState state)
        {
            Execute(item, OnDelete);
        }

        public void Update(T item, EntityState state)
        {
            Execute(item, OnUpdate);
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
        
        protected GraphClient Db { get; }

        private void Execute(T item, Func<ICypherFluentQuery, T, IEnumerable<ICommand>> operation)
        {
            var transaction = _transactionFactory.Create(_session);
            transaction.Enlist(this, query =>
            {
                foreach (var command in operation(query, item))
                {
                    query = command.Execute(query);
                }
                return query;
            });
        }

        protected abstract IEnumerable<ICommand> OnUpdate(ICypherFluentQuery query, T item);

        protected abstract IEnumerable<ICommand> OnInsert(ICypherFluentQuery query, T item);
        
        protected abstract IEnumerable<ICommand> OnDelete(ICypherFluentQuery query, T item);
        
        protected abstract IEnumerable<T> OnGet(params string[] id);

        public Type Type => typeof(T);

        protected IGraphSession Session => _session;
    }
}
