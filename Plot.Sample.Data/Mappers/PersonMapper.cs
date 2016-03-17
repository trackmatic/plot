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
    public class PersonMapper : Mapper<Person>
    {
        public PersonMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {
        }

        protected override object GetData(Person item)
        {
            return new PersonNode(item);
        }

        protected override IQueryExecutor<Person> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }

        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<Person, PersonResult>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
            }
            
            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((person, sites, organisations) => new PersonResult
                {
                    Person = person.As<PersonNode>(),
                    Sites = sites.CollectAs<SiteNode>(),
                    Organisations = organisations.CollectAs<OrganisationNode>()
                });
            }
        }

        #endregion
    }
}