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
            private readonly IMetadataFactory _metadataFactory;

            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
                _metadataFactory = metadataFactory;
            }

            protected override ICypherFluentQuery<ModulePermissionResult> GetDataset(IGraphClient db, GetAbstractQuery<ModulePermission> abstractQuery)
            {

                var cypher = db.Cypher.MatchById(Metadata);
                cypher = cypher.IncludeRelationships(Metadata);
                cypher = cypher.IncludeRelationships(_metadataFactory.Create(typeof (Membership)));
                cypher = cypher.IncludeRelationships(_metadataFactory.Create(typeof(User)));
                cypher = cypher.WithParam("id", abstractQuery.Id);
                return (ICypherFluentQuery<ModulePermissionResult>)OnExecute(cypher);
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((modulePermission, roles, accessGroups, sites, membership, module, modulePermissions, user, person) => new ModulePermissionResult
                {
                    ModulePermission = modulePermission.As<ModulePermissionNode>(),
                    AccessGroups = accessGroups.CollectAs<AccessGroupNode>(),
                    Sites = sites.CollectAs<SiteNode>(),
                    Roles = roles.CollectAs<RoleNode>(),
                    Module = module.As<ModuleNode>(),
                    Membership = membership.As<MembershipNode>(),
                    ModulePermissions = modulePermissions.CollectAs<ModulePermissionNode>(),
                    User = user.As<UserNode>(),
                    Person = person.As<PersonNode>()
                });
            }
        }

        #endregion
    }
}