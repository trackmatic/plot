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
    public class SiteMapper : Mapper<Site>
    {
        public SiteMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {

        }
        
        protected override object GetData(Site item)
        {
            return new SiteNode(item);
        }

        protected override IQueryExecutor<Site> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }
        
        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<Site, SiteResult>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {

            }
            
            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((site, organisation, assets) => new SiteResult
                {
                    Site = site.As<SiteNode>(),
                    Organisation = organisation.As<OrganisationNode>(),
                    Assets = assets.CollectAs<AssetNode>()
                });
            }
        }

        #endregion
    }
}
