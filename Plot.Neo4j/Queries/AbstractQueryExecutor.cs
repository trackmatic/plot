﻿using System;
using System.Collections.Generic;
using System.Linq;
using Neo4jClient;
using Neo4jClient.Cypher;
using Plot.Queries;
using Plot.Logging;
using Plot.Metadata;

namespace Plot.Neo4j.Queries
{
    public abstract class AbstractQueryExecutor<TAggregate, TDataset, TQuery> : IQueryExecutor<TAggregate>
        where TQuery : IQuery<TAggregate>
        where TDataset : IQueryResult
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
                var aggregate = Create(item);
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
                var aggregate = Create(item);
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

        protected abstract ICypherFluentQuery<TDataset> GetDataset(IGraphClient db, TQuery query);
        
        protected IGraphClient Db => _db;

        protected abstract TAggregate Create(TDataset dataset);

        protected abstract void Map(TAggregate aggregate, TDataset item);

        protected NodeMetadata Metadata
        {
            get { return _metadataFactory.Create(typeof(TAggregate)); }
        }

        private void Log(ICypherFluentQuery query)
        {

        }

        public Type QueryType => typeof (TQuery);

        private ICypherFluentQuery<TDataset> CreateCypherQuery(IQuery<TAggregate> query)
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

        private IEnumerable<TAggregate> Map(Func<TDataset, TAggregate> factory, IEnumerable<TDataset> dataset)
        {
            var results = new List<TAggregate>();
            foreach (var item in dataset)
            {
                var aggregate = factory(item);
                Map(aggregate, item);
                results.Add(aggregate);
            }
            return results;
        }
    }
}