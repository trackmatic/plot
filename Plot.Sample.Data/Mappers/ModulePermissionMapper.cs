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
    public class ModulePermissionMapper : Mapper<ModulePermission>
    {
        public ModulePermissionMapper(GraphClient db, IGraphSession session,
            ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory)
            : base(db, session, transactionFactory, metadataFactory)
        {

        }

        protected override object GetData(ModulePermission item)
        {
            return new ModulePermissionNode(item);
        }

        protected override IQueryExecutor<ModulePermission> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }

        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<ModulePermission, ModulePermissionResult>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((modulePermission, roles, sitePermissions, membership, module) => new ModulePermissionResult
                {
                    ModulePermission = modulePermission.As<ModulePermissionNode>(),
                    SitePermissions = sitePermissions.CollectAs<SitePermissionNode>(),
                    Roles = roles.CollectAs<RoleNode>(),
                    Module = module.As<ModuleNode>(),
                    Membership = membership.As<MembershipNode>()
                });
            }
        }

        #endregion
    }
}