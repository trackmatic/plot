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
    public class InvitationMapper : Mapper<Invitation>
    {
        public InvitationMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {
        }

        protected override object GetData(Invitation item)
        {
            return new InvitationNode(item);
        }

        protected override IQueryExecutor<Invitation> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }


        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<Invitation, InvitationResult>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((invitation, membership, issuedBy, acceptedBy) => new InvitationResult
                {
                    Invitation = invitation.As<InvitationNode>(),
                    Membership = membership.As<MembershipNode>(),
                    IssuedBy = issuedBy.As<UserNode>(),
                    AcceptedBy = acceptedBy.As<UserNode>()
                });
            }
        }

        #endregion
    }
}
