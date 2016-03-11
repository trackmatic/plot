using System.Collections.Generic;
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
            return new GetQueryExecutor(Db, MetadataFactory);
        }
        
        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<Site, GetQueryDataset>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {

            }
            
            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((site, organisation, assets) => new GetQueryDataset
                {
                    Site = site.As<SiteNode>(),
                    Organisation = organisation.As<OrganisationNode>(),
                    Assets = assets.CollectAs<AssetNode>()
                });
            }

            protected override Site Create(GetQueryDataset item)
            {
                return item.Site.AsSite();
            }

            protected override void Map(Site aggregate, GetQueryDataset dataset)
            {
                aggregate.Name = dataset.Site.Name;
                aggregate.Set(dataset.Organisation?.AsOrganisation());
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

            public OrganisationNode Organisation { get; set; }

            public IEnumerable<AssetNode> Assets { get; set; }
        }

        #endregion
    }
}
