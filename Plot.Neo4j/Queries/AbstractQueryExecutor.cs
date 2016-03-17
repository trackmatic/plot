using System;
using System.Collections.Generic;
using System.Linq;
using Neo4jClient;
using Neo4jClient.Cypher;
using Plot.Queries;
using Plot.Metadata;

namespace Plot.Neo4j.Queries
{
    public abstract class AbstractQueryExecutor<TAggregate, TResult, TQuery> : IQueryExecutor<TAggregate>
        where TQuery : IQuery<TAggregate>
        where TResult : ICypherQueryResult<TAggregate>
    {
        private readonly GraphClient _db;

        private readonly IMetadataFactory _metadataFactory;

        protected AbstractQueryExecutor(GraphClient db, IMetadataFactory metadataFactory)
        {
            _db = db;
            _metadataFactory = metadataFactory;
        }

        public IEnumerable<TAggregate> Execute(IGraphSession session, IQuery<TAggregate> query)
        {
            var cypher = CreateCypherQuery(query);
            var dataset = cypher.Results.ToList();
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
            var cypher = CreateCypherQuery(query);
            var dataset = cypher.Results.ToList();
            if (!dataset.Any())
            {
                return new PagedGraphGraphCollection<TAggregate>();
            }
            var total = dataset.First().Total;
            var page = total == 0 ? 1 : (query.Skip/total)+1;
            if (!dataset.Any())
            {
                return new PagedGraphGraphCollection<TAggregate>(session, this, query, new List<TAggregate>(), 0, (int)page, enlist);
            }
            var results = Map(item =>
            {
                var aggregate = item.Create();
                if (session.Uow.Contains(aggregate))
                {
                    aggregate = session.Uow.Get<TAggregate>(ProxyUtils.GetEntityId(aggregate));
                }
                else if (enlist)
                {
                    session.Register(aggregate);
                }
                return aggregate;
            }, dataset);
            return new PagedGraphGraphCollection<TAggregate>(session, this, query, results, (int)total, (int)page, enlist);
        }

        protected abstract ICypherFluentQuery<TResult> GetDataset(IGraphClient db, TQuery query);
        
        protected IGraphClient Db => _db;
        
        protected NodeMetadata Metadata => _metadataFactory.Create(typeof(TAggregate));

        private void Log(ICypherFluentQuery query)
        {

        }

        public Type QueryType => typeof (TQuery);

        private ICypherFluentQuery<TResult> CreateCypherQuery(IQuery<TAggregate> query)
        {
            var cypher = GetDataset(_db, (TQuery)query);
            if (query.OrderBy != null && query.OrderBy.Any())
            {
                cypher = query.Descending ? cypher.OrderByDescending(query.OrderBy) : cypher.OrderBy(query.OrderBy);
            }
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
    }
}