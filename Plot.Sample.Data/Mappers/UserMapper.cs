using Neo4jClient;
using Neo4jClient.Cypher;
using Plot.Metadata;
using Plot.Neo4j;
using Plot.Neo4j.Queries;
using Plot.Queries;
using Plot.Sample.Data.Nodes;
using Plot.Sample.Model;

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
            var data = new UserNode
            {
                Id = item.Id,
                Username = item.Username
            };
            return data;
        }

        protected override IQueryExecutor<User> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }


        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<User, GetQueryDataset>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((user, accessGroup) => new GetQueryDataset
                {
                    User = user.As<UserNode>()
                });
            }

            protected override User Create(GetQueryDataset dataset)
            {
                return dataset.User.AsUser();
            }

            protected override void Map(User aggregate, GetQueryDataset dataset)
            {

            }
        }

        #endregion

        #region Datasets

        private class GetQueryDataset : AbstractQueryResult
        {
            public UserNode User { get; set; }
        }

        #endregion
    }
}