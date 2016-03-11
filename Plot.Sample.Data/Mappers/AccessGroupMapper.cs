﻿using System.Collections.Generic;
using Neo4jClient;
using Neo4jClient.Cypher;
using Plot.Metadata;
using Plot.Neo4j;
using Plot.Neo4j.Queries;
using Plot.Queries;
using Plot.Sample.Data.Nodes;
using Plot.Sample.Model;

namespace Plot.Sample.Data.Mappers
{
    public class AccessGroupMapper : Mapper<AccessGroup>
    {
        public AccessGroupMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {
        }

        protected override object GetData(AccessGroup item)
        {
            var data = new
            {
                item.Id,
                item.Name
            };
            return data;
        }

        protected override IQueryExecutor<AccessGroup> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }
        
        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<AccessGroup, GetQueryDataset>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((accessGroup, assets) => new GetQueryDataset
                {
                    AccessGroup = accessGroup.As<AccessGroupNode>(),
                    Assets = assets.CollectAs<AssetNode>()
                });
            }

            protected override AccessGroup Create(GetQueryDataset dataset)
            {
                return dataset.AccessGroup.AsAccessGroup();
            }

            protected override void Map(AccessGroup aggregate, GetQueryDataset dataset)
            {
                aggregate.Name = dataset.AccessGroup.Name;
                foreach (var node in dataset.Assets)
                {
                    aggregate.Add(node.AsAsset());
                }
            }
        }

        #endregion

        #region Datasets

        private class GetQueryDataset : AbstractQueryResult
        {
            public IEnumerable<AssetNode> Assets { get; set; }

            public AccessGroupNode AccessGroup { get; set; }
        }

        #endregion
    }
}
