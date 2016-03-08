using System.Collections.Generic;
using Neo4jClient;
using Neo4jClient.Cypher;
using Plot.Neo4j.Cypher;
using Plot.Neo4j.Queries;
using Plot.Queries;
using Plot.Sample.Data.Nodes;
using Plot.Sample.Model;
using Plot.Sample.Queries;

namespace Plot.Sample.Data.Queries
{
    public class GetOrganisationsByNameExecutor : AbstractQueryExecutor<Organisation, GetOrganisationsByNameExecutor.ResultDataset, GetOrganisationsByName>
    {
        public GetOrganisationsByNameExecutor(GraphClient db) 
            : base(db)
        {

        }

        protected override ICypherFluentQuery<ResultDataset> GetDataset(IGraphClient db, GetOrganisationsByName query)
        {
            var elements = new IQueryBuilderElement[]
                {
                    new Body(query),
                    new AsCount(),
                    new Body(query),
                    new Parameters(query)
                };
            var cypher = QueryBuilder
                .Create(db, elements)
                .ReturnDistinct((organisation, site, total) => new ResultDataset
                {
                    Organisation = organisation.As<OrganisationNode>(),
                    Sites = site.CollectAs<SiteNode>(),
                    Total = total.As<int>()
                });
            return cypher;
        }

        protected override Organisation Create(ResultDataset item)
        {
            return item.Organisation.AsOrganisation();
        }

        protected override void Map(Organisation aggregate, ResultDataset dataset)
        {
            foreach (var siteNode in dataset.Sites)
            {
                aggregate.Add(siteNode.AsSite());
            }
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

            public Body(GetOrganisationsByName query)
            {
                _query = query;
            }

            public ICypherFluentQuery Append(ICypherFluentQuery cypher)
            {
                //cypher = cypher.Match("(user:User { Id: {id}})");
                cypher = cypher.OptionalMatch("(site-[:SITE_OF]->organisation)");
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

        #region Datasets

        public class ResultDataset : AbstractQueryResult
        {
            public OrganisationNode Organisation { get; set; }

            public IEnumerable<SiteNode> Sites { get; set; }
        }
        
        #endregion
    }
}
