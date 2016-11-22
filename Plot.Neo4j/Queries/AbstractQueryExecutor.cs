using System;
using System.Collections.Generic;
using System.Linq;
using Neo4j.Driver.V1;
using Plot.Queries;
using Plot.Metadata;
using Plot.Neo4j.Cypher;

namespace Plot.Neo4j.Queries
{
    public abstract class AbstractQueryExecutor<TAggregate, TResult, TQuery> : IQueryExecutor<TAggregate>
        where TAggregate : class
        where TQuery : IQuery<TAggregate>
        where TResult : ICypherQueryResult<TAggregate>
    {
        private readonly ICypherTransactionFactory _transactionFactory;
        private readonly IMetadataFactory _metadataFactory;

        protected AbstractQueryExecutor(ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory)
        {
            _transactionFactory = transactionFactory;
            _metadataFactory = metadataFactory;
        }

        public IEnumerable<TAggregate> Execute(IGraphSession session, IQuery<TAggregate> query)
        {
            var dataset = Execute(query);
            var results = Map(item =>
            {
                var aggregate = item.Create();
                if (session.Uow.Contains(aggregate))
                {
                    aggregate = session.Uow.Get<TAggregate>(ProxyUtils.GetEntityId(aggregate));
                }
                return aggregate;
            }, dataset);
            return results;
        }

        public IPagedGraphCollection<TAggregate> ExecuteWithPaging(IGraphSession session, IQuery<TAggregate> query, bool enlist)
        {
            var dataset = Execute(query);
            if (!dataset.Any())
            {
                return new PagedGraphGraphCollection<TAggregate>();
            }
            var total = dataset.First().Total;
            var page = total == 0 ? 1 : query.Skip/total+1;
            var results = Map(item =>
            {
                var aggregate = item.Create();
                if (session.Uow.Contains(aggregate))
                {
                    aggregate = session.Uow.Get<TAggregate>(ProxyUtils.GetEntityId(aggregate));
                }
                else if (enlist)
                {
                    aggregate = session.ProxyFactory.Create(aggregate, session);
                }
                return aggregate;

            }, dataset);
            return new PagedGraphGraphCollection<TAggregate>(session, this, query, results, (int)total, (int)page, enlist);
        }

        protected abstract ICypherFluentQuery<TResult> GetDataset(ICypherFluentQuery<TResult> db, TQuery query);
        
        protected NodeMetadata Metadata => _metadataFactory.Create(typeof(TAggregate));

        private void Log(ICypherFluentQuery query)
        {

        }

        public Type QueryType => typeof (TQuery);

        private ICypherFluentQuery<TResult> CreateCypherQuery(IQuery<TAggregate> query)
        {
            var session = CreateFluentQuery();
            var cypher = GetDataset(session, (TQuery)query);
            cypher = OrderByHelper<TResult, TAggregate>.OrderBy(cypher, query);
            cypher = cypher.Skip(query.Skip).Limit(query.Take);
            Log(cypher);
            return cypher;
        }

        private IEnumerable<TAggregate> Map(Func<TResult, TAggregate> factory, IEnumerable<TResult> dataset)
        {
            var results = new List<TAggregate>();
            foreach (var item in dataset)
            {
                var aggregate = factory(item);
                item.Map(aggregate);
                results.Add(aggregate);
            }
            return results;
        }
        
        private IList<TResult> Execute(IQuery<TAggregate> query)
        {
            var cypher = CreateCypherQuery(query);
            var dataset = _transactionFactory.Run<TResult>(cypher);
            return dataset;
        }

        private ICypherFluentQuery<TResult> CreateFluentQuery()
        {
            return new CypherFluentQuery<TResult>();
        }
    }
}