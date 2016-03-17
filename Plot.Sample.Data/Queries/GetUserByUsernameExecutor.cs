using Neo4jClient;
using Neo4jClient.Cypher;
using Plot.Metadata;
using Plot.Neo4j;
using Plot.Neo4j.Cypher;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;
using Plot.Sample.Data.Results;
using Plot.Sample.Queries;

namespace Plot.Sample.Data.Queries
{
    public class GetUserByUsernameExecutor : AbstractQueryExecutor<User, UserResult, GetUserByUsername>
    {
        public GetUserByUsernameExecutor(GraphClient db, IMetadataFactory metadataFactory) 
            : base(db, metadataFactory)
        {

        }

        protected override ICypherFluentQuery<UserResult> GetDataset(IGraphClient db, GetUserByUsername query)
        {
            var cypher = QueryBuilder.Create(db, new IQueryBuilderElement[]
            {
                new Body(Metadata),
                new AsCount(),
                new Body(Metadata),
                new Parameters(query)
            }).ReturnDistinct((user, person, memberships, password, total) => new UserResult
            {
                User = user.As<UserNode>(),
                Person = person.As<PersonNode>(),
                Memberships = memberships.CollectAs<MembershipNode>(),
                Password = password.As<PasswordNode>(),
                Total = total.As<int>()
            });
            return cypher;
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
            private readonly NodeMetadata _nodeMetadata;

            public Body(NodeMetadata nodeMetadata)
            {
                _nodeMetadata = nodeMetadata;
            }

            public ICypherFluentQuery Append(ICypherFluentQuery cypher)
            {
                cypher = cypher.Match("(user:User { Username: {username}})");
                cypher = cypher.IncludeRelationships(_nodeMetadata);
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
    }
}
