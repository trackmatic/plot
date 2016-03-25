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
    public class GetSitesByOrganisationExecutor : AbstractQueryExecutor<Site, SiteResult, GetSitesByOrganisation>
    {
        public GetSitesByOrganisationExecutor(GraphClient db, IMetadataFactory metadataFactory) 
            : base(db, metadataFactory)
        {

        }

        protected override ICypherFluentQuery<SiteResult> GetDataset(IGraphClient db, GetSitesByOrganisation query)
        {
            var cypher = QueryBuilder.Create(db, new IQueryBuilderElement[]
            {
                new Body(query, Metadata),
                new AsCount(),
                new Body(query, Metadata),
                new Parameters(query)
            }).ReturnDistinct((site, organisation, assets, total) => new SiteResult
            {
                Site = site.As<SiteNode>(),
                Organisation = organisation.As<OrganisationNode>(),
                Assets = assets.CollectAs<AssetNode>(),
                Total = total.As<int>()
            });
            return cypher;
        }

        private class AsCount : IQueryBuilderElement
        {
            public ICypherFluentQuery Append(ICypherFluentQuery cypher)
            {
                return cypher.With("count(distinct(site)) as total");
            }
        }

        private class Body : IQueryBuilderElement
        {
            private readonly NodeMetadata _nodeMetadata;

            private readonly GetSitesByOrganisation _query;

            public Body(GetSitesByOrganisation query, NodeMetadata nodeMetadata)
            {
                _query = query;
                _nodeMetadata = nodeMetadata;
            }

            public ICypherFluentQuery Append(ICypherFluentQuery cypher)
            {
                cypher = cypher.Match("(organisation:Organisation { Id: {organisationId}})-[:RUNS]->(site:Site)");
                cypher = Matching(cypher);
                cypher = cypher.IncludeRelationships(_nodeMetadata);
                return cypher;
            }

            private ICypherFluentQuery Matching(ICypherFluentQuery cypher)
            {
                if (string.IsNullOrEmpty(_query.Term))
                {
                    return cypher;
                }
                return cypher.Where("site.Name =~ {term}");
            }
        }

        private class Parameters : IQueryBuilderElement
        {
            private readonly GetSitesByOrganisation _query;

            public Parameters(GetSitesByOrganisation query)
            {
                _query = query;
            }

            public ICypherFluentQuery Append(ICypherFluentQuery cypher)
            {
                cypher = OrganisationId(cypher);
                cypher = Term(cypher);
                return cypher;
            }

            private ICypherFluentQuery OrganisationId(ICypherFluentQuery cyper)
            {
                return cyper.WithParam("organisationId", _query.OrganisationId);
            }

            private ICypherFluentQuery Term(ICypherFluentQuery cypher)
            {
                if (string.IsNullOrEmpty(_query.Term))
                {
                    return cypher;
                }
                cypher = cypher.WithParam("term", $"{_query.Term}");
                return cypher;
            }
        }
    }
}