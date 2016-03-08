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
            return new GetQueryExecutor(Db);
        }
        
        #region Queries

        private class GetQueryExecutor : GetQueryExecutorBase<Organisation, GetQueryDataset>
        {
            public GetQueryExecutor(GraphClient db) : base(db)
            {
            }

            protected override ICypherFluentQuery<GetQueryDataset> GetDataset(IGraphClient db, GetAbstractQuery<Organisation> abstractQuery)
            {
                var cypher = db
                    .Cypher
                    .Match("(organisation:Organisation)")
                    .Where("organisation.Id in {id}")
                    .OptionalMatch("(site-[:SITE_OF]->organisation)")
                    .OptionalMatch("(organisation-[:CONTACT_FOR]->contact)")
                    .WithParam("id", abstractQuery.Id)
                    .ReturnDistinct((organisation, site, contact) => new GetQueryDataset
                    {
                        Organisation = organisation.As<OrganisationNode>(),
                        Sites = site.CollectAs<SiteNode>(),
                        Contact = contact.As<ContactNode>()
                    });
                return cypher;
            }

            protected override Organisation Create(GetQueryDataset item)
            {
                return item.Organisation.AsOrganisation();
            }

            protected override void Map(Organisation aggregate, GetQueryDataset dataset)
            {
                aggregate.Name = dataset.Organisation.Name;
                aggregate.Contact = dataset.Contact?.AsContact();
                foreach (var node in dataset.Sites)
                {
                    aggregate.Add(node.AsSite());
                }
            }
        }

        #endregion

        #region Datasets

        private class GetQueryDataset : AbstractQueryResult
        {
            public OrganisationNode Organisation { get; set; }

            public IEnumerable<SiteNode> Sites { get; set; }

            public ContactNode Contact { get; set; }
        }

        #endregion
    }
}