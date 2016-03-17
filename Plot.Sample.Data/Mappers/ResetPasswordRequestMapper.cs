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
    public class ResetPasswordRequestMapper : Mapper<ResetPasswordRequest>
    {
        public ResetPasswordRequestMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {
        }

        protected override object GetData(ResetPasswordRequest item)
        {
            return new ResetPasswordRequestNode(item);
        }

        protected override IQueryExecutor<ResetPasswordRequest> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }

        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<ResetPasswordRequest, ResetPasswordRequestResult>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((resetPasswordRequest, user) => new ResetPasswordRequestResult
                {
                    ResetPasswordRequest = resetPasswordRequest.As<ResetPasswordRequestNode>(),
                    User = user.As<UserNode>()
                });
            }
        }

        #endregion
    }
}