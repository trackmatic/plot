using System.Collections.Generic;
using Neo4jClient;
using Neo4jClient.Cypher;
using Octo.Core;
using Octo.Core.Queries;
using Octo.N4j;
using Octo.N4j.Commands;
using Octo.N4j.Queries;
using Octo.N4j.Snippets;
using Octo.Sample.Model;
using Octo.Sample.Nodes;

namespace Octo.Sample.Mappers
{
    public class OrganisationMapper : MapperBase<Organisation>
    {
        public OrganisationMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory) 
            : base(db, session, transactionFactory)
        {

        }

        protected override IEnumerable<ICommand> OnUpdate(ICypherFluentQuery query, Organisation item)
        {
            return OnInsert(query, item);
        }

        protected override IEnumerable<ICommand> OnDelete(ICypherFluentQuery query, Organisation item)
        {
            return new List<ICommand> { new DeleteOrganisationCommand(item) };
        }

        protected override IEnumerable<ICommand> OnInsert(ICypherFluentQuery query, Organisation item)
        {
            var commands = new List<ICommand> { new PersistOrganisationCommand(item) };
            return commands;
        }

        protected override IEnumerable<Organisation> OnGet(params string[] id)
        {
            var executor = new AbstractGetQueryExecutor(Db);
            var item = executor.Execute(Session.Uow, new GetQuery<Organisation>(id));
            return item;
        }
        
        #region Commands

        private class PersistOrganisationCommand : CreateCommandBase<Organisation>
        {
            private readonly Organisation _organisation;

            public PersistOrganisationCommand(Organisation organisation)
                : base(new NodeSnippet<Organisation>(organisation))
            {
                _organisation = organisation;
            }

            protected override object Data()
            {
                var data = new
                {
                    _organisation.Id,
                    _organisation.Name
                };
                return data;
            }
        }

        private class DeleteOrganisationCommand : DeleteCommandBase<Organisation>
        {
            public DeleteOrganisationCommand(Organisation organisation)
                : base(new NodeSnippet<Organisation>(organisation))
            {

            }
        }

        #endregion

        #region Queries

        private class AbstractGetQueryExecutor : AbstractGetQueryExecutor<Organisation, GetQueryDataset>
        {
            public AbstractGetQueryExecutor(GraphClient db) : base(db)
            {
            }

            protected override ICypherFluentQuery<GetQueryDataset> GetDataset(IGraphClient db, GetQuery<Organisation> query)
            {
                var cypher = db
                    .Cypher
                    .Match("(organisation:Organisation)")
                    .Where("organisation.Id in {id}")
                    .OptionalMatch("(site-[:SITE_OF]->organisation)")
                    .WithParam("id", query.Id)
                    .ReturnDistinct((organisation, site, driverTag, person, asset) => new GetQueryDataset
                    {
                        Organisation = organisation.As<OrganisationNode>(),
                        Sites = site.CollectAs<SiteNode>()
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
                foreach (var node in dataset.Sites)
                {
                    aggregate.Add(node.AsSite());
                }
            }
        }

        #endregion

        #region Datasets

        private class GetQueryDataset : QueryResultBase
        {
            public OrganisationNode Organisation { get; set; }

            public IEnumerable<SiteNode> Sites { get; set; }
        }

        #endregion
    }
}