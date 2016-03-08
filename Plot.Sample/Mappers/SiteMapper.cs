using System.Collections.Generic;
using Neo4jClient;
using Neo4jClient.Cypher;
using Plot.Metadata;
using Plot.N4j;
using Plot.N4j.Queries;
using Plot.Queries;
using Plot.Sample.Model;
using Plot.Sample.Nodes;

namespace Plot.Sample.Mappers
{
    public class SiteMapper : Mapper<Site>
    {
        public SiteMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {

        }
        
        protected override object GetData(Site item)
        {
            var data = new
            {
                item.Id,
                item.Name
            };
            return data;
        }

        protected override IQueryExecutor<Site> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db);
        }
        
        #region Queries

        private class GetQueryExecutor : GetQueryExecutorBase<Site, GetQueryDataset>
        {
            public GetQueryExecutor(GraphClient db) : base(db)
            {

            }

            protected override ICypherFluentQuery<GetQueryDataset> GetDataset(IGraphClient db, GetAbstractQuery<Site> abstractQuery)
            {
                var dataset = db
                    .Cypher
                    .Match("(site:Site)")
                    .Where("site.Id in {id}")
                    .OptionalMatch("(site-[:SITE_OF]->organisation)")
                    .OptionalMatch("(site-[:SITE_OF]->(organisation:Organisation))")
                    .OptionalMatch("((asset:Asset)-[:BELONGS_TO]->site)")
                    .With("organisation, site, asset")
                    .WithParam("id", abstractQuery.Id)
                    .ReturnDistinct((site, organisation, asset) => new GetQueryDataset
                    {
                        Site = site.As<SiteNode>(),
                        Organisations = organisation.CollectAs<OrganisationNode>(),
                        Assets = asset.CollectAs<AssetNode>()
                    });
                return dataset;
            }

            protected override Site Create(GetQueryDataset item)
            {
                return item.Site.AsSite();
            }

            protected override void Map(Site aggregate, GetQueryDataset dataset)
            {
                aggregate.Name = dataset.Site.Name;
                foreach (var node in dataset.Organisations)
                {
                    aggregate.Add(node.AsOrganisation());
                }
                foreach ( var node in dataset.Assets)
                {
                    aggregate.Add(node.AsAsset());
                }
            }
        }

        #endregion

        #region Datasets

        private class GetQueryDataset : AbstractQueryResult
        {
            public SiteNode Site { get; set; }

            public IEnumerable<OrganisationNode> Organisations { get; set; }

            public IEnumerable<AssetNode> Assets { get; set; }
        }

        #endregion
    }
}
