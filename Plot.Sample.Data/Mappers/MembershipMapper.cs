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
    public class MembershipMapper : Mapper<Membership>
    {
        public MembershipMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {
        }

        protected override object GetData(Membership item)
        {
            return new MembershipNode(item);
        }

        protected override IQueryExecutor<Membership> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }


        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<Membership, MembershipResult>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((membership, organisation, accessGroups, modulePermissions) => new MembershipResult
                {
                    Membership = membership.As<MembershipNode>(),
                    Organisation = organisation.As<OrganisationNode>(),
                    AccessGroups = accessGroups.CollectAs<AccessGroupNode>(),
                    ModulePermissions = modulePermissions.CollectAs<ModulePermissionNode>()
                });
            }
        }

        #endregion
    }
}
