using Neo4jClient;
using Neo4jClient.Cypher;
using Plot.Metadata;
using Plot.Neo4j;
using Plot.Neo4j.Queries;
using Plot.Queries;
using Plot.Sample.Data.Nodes;
using Plot.Sample.Data.Results;
using Plot.Sample.Model;

namespace Plot.Sample.Data.Mappers
{
    public class OrganisationMapper : Mapper<Organisation>
    {
        public OrganisationMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {

        }

        protected override object GetData(Organisation item)
        {
            var data = new
            {
                item.Id,
                item.Name
            };
            return data;
        }

        protected override IQueryExecutor<Organisation> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }
        
        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<Organisation, OrganisationResult>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((organisation, sites, accessGroups) => new OrganisationResult
                {
                    Organisation = organisation.As<OrganisationNode>(),
                    AccessGroups = accessGroups.CollectAs<AccessGroupNode>(),
                    Sites = sites.CollectAs<SiteNode>()
                });
            }
        }

        #endregion
    }
}