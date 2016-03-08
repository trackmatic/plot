using System.Collections.Generic;
using Neo4jClient;
using Neo4jClient.Cypher;
using Plot.Metadata;
using Plot.Neo4j;
using Plot.Neo4j.Queries;
using Plot.Queries;
using Plot.Sample.Model;
using Plot.Sample.Nodes;

namespace Plot.Sample.Mappers
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
                item.Name
            };
            return data;
        }

        protected override IQueryExecutor<Asset> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db);
        }


        #region Queries

        private class GetQueryExecutor : GetQueryExecutorBase<Asset, GetQueryDataset>
        {
            public GetQueryExecutor(GraphClient db) : base(db)
            {
            }

            protected override ICypherFluentQuery<GetQueryDataset> GetDataset(IGraphClient db, GetAbstractQuery<Asset> abstractQuery)
            {
                var cypher = db
                    .Cypher
                    .Match("(asset:Asset)")
                    .Where("asset.Id in {id}")
                    .OptionalMatch("(asset-[:BELONGS_TO]->(site:Site))")
                    .WithParam("id", abstractQuery.Id)
                    .ReturnDistinct((asset, site) => new GetQueryDataset
                    {
                        Sites = site.CollectAs<SiteNode>(),
                        Asset = site.As<AssetNode>()
                    });
                return cypher;
            }

            protected override Asset Create(GetQueryDataset dataset)
            {
                var contact = dataset.Asset.AsAsset();
                return contact;
            }

            protected override void Map(Asset aggregate, GetQueryDataset dataset)
            {
                aggregate.Name = dataset.Asset.Name;
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
