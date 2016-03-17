using Neo4jClient;
using Neo4jClient.Cypher;
using Plot.Metadata;
using Plot.Neo4j;
using Plot.Neo4j.Queries;
using Plot.Queries;
using Plot.Sample.Data.Nodes;
using Plot.Sample.Data.Results;
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

        private class GetQueryExecutor : GenericQueryExecutor<Asset, AssetResult>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((asset, sites) => new AssetResult
                {
                    Asset = asset.As<AssetNode>(),
                    Sites = sites.CollectAs<SiteNode>()
                });
            }
        }

        #endregion
    }
}
