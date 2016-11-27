using System;
using System.Collections.Generic;
using System.Linq;
using Neo4j.Driver.V1;
using Plot.Metadata;
using Plot.Neo4j.Cypher;
using Plot.Neo4j.Cypher.Commands;
using Plot.Neo4j.Queries;
using Plot.Queries;

namespace Plot.Neo4j
{
    public abstract class Mapper<T> : IMapper<T> where T : class
    {
        private readonly IGraphSession _session;
        private readonly ICypherTransactionFactory _transactionFactory;

        protected Mapper(IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory)
        {
            _session = session;
            _transactionFactory = transactionFactory;
            MetadataFactory = metadataFactory;
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

        public Type Type => typeof(T);
        
        protected abstract object GetData(T item);

        protected abstract IQueryExecutor<T> CreateQueryExecutor();

        protected IGraphSession Session => _session;

        protected ICypherTransactionFactory TransactionFactory => _transactionFactory;
        
        private void Execute(T item, Func<ICypherQuery, T, IEnumerable<ICommand>> operation)
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

        private IList<ICommand> OnUpdate(ICypherQuery query, T item)
        {
            return OnInsert(query, item);
        }

        private IList<ICommand> OnInsert(ICypherQuery query, T item)
        {
            var commandFactory = new CommandFactory<T>(MetadataFactory);
            commandFactory.Insert(item, GetData(item));
            return commandFactory.Commands;
        }

        private IList<ICommand> OnDelete(ICypherQuery query, T item)
        {
            var commandFactory = new CommandFactory<T>(MetadataFactory);
            commandFactory.Delete(item);
            return commandFactory.Commands;
        }

        private IList<T> OnGet(params string[] id)
        {
            var executor = CreateQueryExecutor();
            var item = executor.Execute(Session, new GetAbstractQuery<T>(id));
            return item.ToList();
        }

        protected IMetadataFactory MetadataFactory { get; }

        protected GenericQueryExecutor<T, TResult> CreateGenericExecutor<TResult>(Func<IRecord, TResult> map, Func<ICypherReturn<TResult>, ICypherReturn<TResult>> returnFactory)
            where TResult : ICypherQueryResult<T>
        {
            return new GetQueryExecutor<TResult>(_transactionFactory, MetadataFactory, map, returnFactory);
        }

        private class GetQueryExecutor<TResult> : GenericQueryExecutor<T, TResult> where TResult : ICypherQueryResult<T>
        {
            private readonly Func<IRecord, TResult> _map;
            private readonly Func<ICypherReturn<TResult>, ICypherReturn<TResult>> _returnFactory;

            public GetQueryExecutor(ICypherTransactionFactory transactionFactory,
                IMetadataFactory metadataFactory,
                Func<IRecord, TResult> map, Func<ICypherReturn<TResult>,
                ICypherReturn<TResult>> returnFactory) : base(transactionFactory, metadataFactory)
            {
                _map = map;
                _returnFactory = returnFactory;
            }

            protected override ICypherQuery<TResult> OnExecute(ICypherQuery<TResult> cypher)
            {
                return cypher.ReturnDistinct(_map, _returnFactory);
            }
        }
    }
}
