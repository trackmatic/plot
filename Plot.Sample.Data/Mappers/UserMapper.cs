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
    public class UserMapper : Mapper<User>
    {
        public UserMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {
        }

        protected override object GetData(User item)
        {
            return new UserNode(item);
        }

        protected override IQueryExecutor<User> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }

        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<User, UserResult>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((user, person, memberships, password) => new UserResult
                {
                    User = user.As<UserNode>(),
                    Person = person.As<PersonNode>(),
                    Memberships = memberships.CollectAs<MembershipNode>(),
                    Password = password.As<PasswordNode>()
                });
            }
        }

        #endregion
    }
}