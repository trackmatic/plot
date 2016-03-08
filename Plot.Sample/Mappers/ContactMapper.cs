using Neo4jClient;
using Neo4jClient.Cypher;
using Plot.Metadata;
using Plot.Neo4j;
using Plot.Neo4j.Queries;
using Plot.Queries;
using Plot.Sample.Model;
using Plot.Sample.Nodes;

namespace Plot.Sample.Mappers
{
    public class ContactMapper : Mapper<Contact>
    {
        public ContactMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {
        }

        protected override object GetData(Contact item)
        {
            var data = new
            {
                item.Id,
                item.Name
            };
            return data;
        }

        protected override IQueryExecutor<Contact> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db);
        }


        #region Queries

        private class GetQueryExecutor : GetQueryExecutorBase<Contact, GetQueryDataset>
        {
            public GetQueryExecutor(GraphClient db) : base(db)
            {
            }

            protected override ICypherFluentQuery<GetQueryDataset> GetDataset(IGraphClient db, GetAbstractQuery<Contact> abstractQuery)
            {
                var cypher = db
                    .Cypher
                    .Match("(contact:Contact)")
                    .Where("contact.Id in {id}")
                    .OptionalMatch("(contact-[:CONTACT_FOR]->organisation)")
                    .WithParam("id", abstractQuery.Id)
                    .ReturnDistinct((contact, organisation) => new GetQueryDataset
                    {
                        Organisation = organisation.As<OrganisationNode>(),
                        Contact = contact.As<ContactNode>()
                    });
                return cypher;
            }

            protected override Contact Create(GetQueryDataset dataset)
            {
                var contact = new Contact { Id = dataset.Contact.Id, Name = dataset.Contact.Name };
                return contact;
            }

            protected override void Map(Contact aggregate, GetQueryDataset dataset)
            {
                aggregate.Name = dataset.Organisation.Name;
            }
        }

        #endregion

        #region Datasets

        private class GetQueryDataset : AbstractQueryResult
        {
            public OrganisationNode Organisation { get; set; }

            public ContactNode Contact { get; set; }
        }

        #endregion
    }
}
