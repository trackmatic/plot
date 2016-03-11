using System.Collections.Generic;
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
    public class AssetMapper : Mapper<Asset>
    {
        public AssetMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {
        }

        protected override object GetData(Asset item)
        {
            var data = new
            {
                item.Id,
                item.FleetNumber
            };
            return data;
        }

        protected override IQueryExecutor<Asset> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }


        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<Asset, GetQueryDataset>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((asset, sites) => new GetQueryDataset
                {
                    Asset = asset.As<AssetNode>(),
                    Sites = sites.CollectAs<SiteNode>()
                });
            }

            protected override Asset Create(GetQueryDataset dataset)
            {
                var contact = dataset.Asset.AsAsset();
                return contact;
            }

            protected override void Map(Asset aggregate, GetQueryDataset dataset)
            {
                aggregate.FleetNumber = dataset.Asset.FleetNumber;
                foreach (var site in dataset.Sites)
                {
                    aggregate.Add(site.AsSite());
                }
            }
        }

        #endregion

        #region Datasets

        private class GetQueryDataset : AbstractQueryResult
        {
            public IEnumerable<SiteNode> Sites { get; set; }

            public AssetNode Asset { get; set; }
        }

        #endregion
    }
}
