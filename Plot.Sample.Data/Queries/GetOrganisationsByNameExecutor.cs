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
    public class GetOrganisationsByNameExecutor : AbstractQueryExecutor<Organisation, OrganisationResult, GetOrganisationsByName>
    {
        public GetOrganisationsByNameExecutor(GraphClient db, IMetadataFactory metadataFactory) 
            : base(db, metadataFactory)
        {

        }

        protected override ICypherFluentQuery<OrganisationResult> GetDataset(IGraphClient db, GetOrganisationsByName query)
        {
            var cypher = QueryBuilder.Create(db, new IQueryBuilderElement[]
            {
                new Body(query, Metadata),
                new AsCount(),
                new Body(query, Metadata),
                new Parameters(query)
            }).ReturnDistinct((organisation, total) => new OrganisationResult
            {
                Organisation = organisation.As<OrganisationNode>(),
                //Sites = site.CollectAs<SiteNode>(),
                Total = total.As<int>()
            });
            return cypher;
        }
       
        private class AsCount : IQueryBuilderElement
        {
            public ICypherFluentQuery Append(ICypherFluentQuery cypher)
            {
                return cypher.With("count(distinct(organisation)) as total");
            }
        }

        private class Body : IQueryBuilderElement
        {
            private readonly GetOrganisationsByName _query;

            private readonly NodeMetadata _nodeMetadata;

            public Body(GetOrganisationsByName query, NodeMetadata nodeMetadata)
            {
                _query = query;
                _nodeMetadata = nodeMetadata;
            }

            public ICypherFluentQuery Append(ICypherFluentQuery cypher)
            {
                cypher = cypher.Match("(organisation:Organisation)");
                //cypher = cypher.Match("(user:User { Id: {id}})");
                //cypher = cypher.OptionalMatch("(site-[:SITE_OF]->organisation)");
                //cypher = cypher.IncludeRelationships(_nodeMetadata);
                return cypher;
            }

            private ICypherFluentQuery Matching(ICypherFluentQuery cypher)
            {
                if (string.IsNullOrEmpty(_query.Name))
                {
                    return cypher;
                }
                cypher = cypher.Where("organisation.Name =~ {name}");
                return cypher;
            }
        }

        private class Parameters : IQueryBuilderElement
        {
            private readonly GetOrganisationsByName _query;

            public Parameters(GetOrganisationsByName query)
            {
                _query = query;
            }

            public ICypherFluentQuery Append(ICypherFluentQuery cypher)
            {
                if(string.IsNullOrEmpty(_query.Name))
                {
                    return cypher;
                }

                cypher = cypher.WithParam("name", $"(?i){_query.Name}.*");

                return cypher;
            }
        }
    }
}
