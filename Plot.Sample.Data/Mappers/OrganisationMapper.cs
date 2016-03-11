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
            return new GetQueryExecutor(Db, MetadataFactory);
        }
        
        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<Organisation, GetQueryDataset>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
            }

            protected override Organisation Create(GetQueryDataset item)
            {
                return item.Organisation.AsOrganisation();
            }

            protected override void Map(Organisation aggregate, GetQueryDataset dataset)
            {
                aggregate.Name = dataset.Organisation.Name;
                dataset.Sites.Map(x => aggregate.Add(x.AsSite()));
                dataset.AccessGroups.Map(x => aggregate.Add(x.AsAccessGroup()));
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct((organisation, sites, accessGroups) => new GetQueryDataset
                {
                    Organisation = organisation.As<OrganisationNode>(),
                    AccessGroups = accessGroups.CollectAs<AccessGroupNode>(),
                    Sites = sites.CollectAs<SiteNode>()
                });
            }
        }

        #endregion

        #region Datasets

        private class GetQueryDataset : AbstractQueryResult
        {
            public OrganisationNode Organisation { get; set; }

            public IEnumerable<SiteNode> Sites { get; set; }

            public IEnumerable<AccessGroupNode> AccessGroups { get; set; }
        }

        #endregion
    }
}