using System;
using Plot.Metadata;
using Plot.Neo4j;
using Plot.Neo4j.Cypher;
using Plot.Neo4j.Queries;
using Plot.Queries;
using Plot.Sample.Data.Nodes;
using Plot.Sample.Data.Results;

namespace Plot.Sample.Data.Mappers
{
    public class SiteMapper : Mapper<Person>
    {
        public SiteMapper(IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(session, transactionFactory, metadataFactory)
        {

        }
        
        protected override object GetData(Person item)
        {
            return new PersonNode(item);
        }

        protected override IQueryExecutor<Person> CreateQueryExecutor()
        {
            return new GetQueryExecutor(TransactionFactory, MetadataFactory);
        }
        
        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<Person, PersonResult>
        {
            public GetQueryExecutor(ICypherTransactionFactory db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {

            }
            
            protected override ICypherFluentQuery<PersonResult> OnExecute(ICypherFluentQuery<PersonResult> cypher)
            {
                /*return cypher.ReturnDistinct((site, organisation, assets) => new SiteResult
                {
                    Site = site.As<SiteNode>(),
                    Organisation = organisation.As<OrganisationNode>(),
                    Assets = assets.CollectAs<AssetNode>()
                });*/
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
