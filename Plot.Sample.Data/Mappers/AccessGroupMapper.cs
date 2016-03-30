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
    public class AccessGroupMapper : Mapper<AccessGroup>
    {
        public AccessGroupMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {
        }

        protected override object GetData(AccessGroup item)
        {
            return new AccessGroupNode(item);
        }

        protected override IQueryExecutor<AccessGroup> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }
        
        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<AccessGroup, AccessGroupResult>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((accessGroup, assets, modulePermissions) => new AccessGroupResult
                {
                    AccessGroup = accessGroup.As<AccessGroupNode>(),
                    Assets = assets.CollectAs<AssetNode>(),
                    ModulePermissions = modulePermissions.CollectAs<ModulePermissionNode>()
                });
            }
        }

        #endregion
    }
}
