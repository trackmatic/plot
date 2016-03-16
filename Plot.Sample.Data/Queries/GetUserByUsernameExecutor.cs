using Neo4jClient;
using Neo4jClient.Cypher;
using Plot.Metadata;
using Plot.Neo4j.Cypher;
using Plot.Neo4j.Queries;
using Plot.Queries;
using Plot.Sample.Data.Nodes;
using Plot.Sample.Model;
using Plot.Sample.Queries;

namespace Plot.Sample.Data.Queries
{
    public class GetUserByUsernameExecutor : AbstractQueryExecutor<User, GetUserByUsernameExecutor.ResultDataset, GetUserByUsername>
    {
        private readonly IMetadataFactory _metadataFactory;

        public GetUserByUsernameExecutor(GraphClient db, IMetadataFactory metadataFactory) 
            : base(db, metadataFactory)
        {
            _metadataFactory = metadataFactory;
        }

        protected override ICypherFluentQuery<ResultDataset> GetDataset(IGraphClient db, GetUserByUsername query)
        {
            var metadata = _metadataFactory.Create(typeof(User));
            var elements = new IQueryBuilderElement[]
            {
                new Body(metadata),
                new AsCount(),
                new Body(metadata),
                new Parameters(query)
            };
            var cypher = QueryBuilder.Create(db, elements).ReturnDistinct((user, total) => new ResultDataset
            {
                User = user.As<UserNode>(),
                Total = total.As<int>()
            });
            return cypher;
        }

        protected override User Create(ResultDataset item)
        {
            return item.User.AsUser();
        }

        protected override void Map(User aggregate, ResultDataset dataset)
        {
        }

        private class AsCount : IQueryBuilderElement
        {
            public ICypherFluentQuery Append(ICypherFluentQuery cypher)
            {
                return cypher.With("count(distinct(user)) as total");
            }
        }

        private class Body : IQueryBuilderElement
        {
            private readonly NodeMetadata _metadata;

            public Body(NodeMetadata metadata)
            {
                _metadata = metadata;
            }

            public ICypherFluentQuery Append(ICypherFluentQuery cypher)
            {
                cypher = cypher.Match("(user:User { Username: {username}})");
                cypher = cypher.IncludeRelationships(_metadata);
                return cypher;
            }
        }

        private class Parameters : IQueryBuilderElement
        {
            private readonly GetUserByUsername _query;

            public Parameters(GetUserByUsername query)
            {
                _query = query;
            }

            public ICypherFluentQuery Append(ICypherFluentQuery cypher)
            {
                cypher = cypher.WithParam("username", $"{_query.Username}");
                return cypher;
            }
        }

        #region Datasets

        public class ResultDataset : AbstractQueryResult
        {
            public UserNode User { get; set; }
        }

        #endregion
    }
}
