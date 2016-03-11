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
    public class PersonMapper : Mapper<Person>
    {
        public PersonMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {
        }

        protected override object GetData(Person item)
        {
            var data = new PersonNode
            {
                Id = item.Id,
                FirstName = item.Names.First,
                LastName = item.Names.Last
            };
            return data;
        }

        protected override IQueryExecutor<Person> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }

        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<Person, GetQueryDataset>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
            }

            protected override Person Create(GetQueryDataset dataset)
            {
                return dataset.Person.AsPerson();
            }

            protected override void Map(Person aggregate, GetQueryDataset item)
            {
                item.Organisations.Map(x => aggregate.Add(x.AsOrganisation()));
                item.Sites.Map(x => aggregate.Add(x.AsSite()));
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((person, sites, organisations) => new GetQueryDataset
                {
                    Person = person.As<PersonNode>(),
                    Sites = sites.CollectAs<SiteNode>(),
                    Organisations = organisations.CollectAs<OrganisationNode>()
                });
            }
        }

        #endregion

        #region Datasets

        private class GetQueryDataset : AbstractQueryResult
        {
            public PersonNode Person { get; set; }

            public IEnumerable<SiteNode> Sites { get; set; }

            public IEnumerable<OrganisationNode> Organisations { get; set; }
        }

        #endregion
    }
}