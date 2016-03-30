using Neo4jClient;
using Neo4jClient.Cypher;
using Plot.Metadata;
using Plot.Neo4j;
using Plot.Neo4j.Queries;
using Plot.Queries;
using Plot.Sample.Data.Nodes;
using Plot.Sample.Data.Results;

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
            return new AssetNode(item);
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

            protected override ICypherFluentQuery<AssetResult> GetDataset(IGraphClient db, GetAbstractQuery<Asset> abstractQuery)
            {
                var cypher = db.Cypher.MatchById(Metadata);
                cypher = cypher.IncludeRelationships(Metadata);
                cypher = IsA(cypher, "Trailer");
                cypher = IsA(cypher, "Vehicle");
                cypher = IsA(cypher, "Forklift");
                cypher = cypher.WithParam("id", abstractQuery.Id);
                return (ICypherFluentQuery<AssetResult>)OnExecute(cypher);
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((asset, sites, vehicle, trailer, forklift, accessGroups) => new AssetResult
                {
                    Asset = asset.As<AssetNode>(),
                    Sites = sites.CollectAs<SiteNode>(),
                    Vehicle = vehicle.As<VehicleNode>(),
                    Trailer = trailer.As<TrailerNode>(),
                    Forklift = forklift.As<ForkliftNode>(),
                    AccessGroups = accessGroups.CollectAs<AccessGroupNode>()
                });
            }

            private ICypherFluentQuery IsA(ICypherFluentQuery cypher, string type)
            {
                return cypher.OptionalMatch($"(asset)-[:{Relationships.IsA}]->({type.ToLower()}:{type})");
            }
        }

        #endregion
    }
}
