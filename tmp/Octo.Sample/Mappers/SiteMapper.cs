using System.Collections.Generic;
using System.Linq;
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
    public class SiteMapper : MapperBase<Site>
    {
        public SiteMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory) : base(db, session, transactionFactory)
        {

        }

        protected override IEnumerable<ICommand> OnUpdate(ICypherFluentQuery query, Site item)
        {
            return OnInsert(query, item);
        }

        protected override IEnumerable<ICommand> OnDelete(ICypherFluentQuery query, Site item)
        {
            return new ICommand[] { new DeleteSiteCommand(item) };
        }

        protected override IEnumerable<ICommand> OnInsert(ICypherFluentQuery query, Site item)
        {
            var commands = new List<ICommand> { new PersistSiteCommand(item) };
            commands.AddRange(EntityUtils.Flush(item.Organisations).Select(x => new RemoveOrganisationCommand(item, x)));
            commands.AddRange(item.Organisations.Select(x => new AddOrganisationCommand(item, x)));
            return commands;
        }

        protected override IEnumerable<Site> OnGet(params string[] id)
        {
            return new AbstractGetQueryExecutor(Db).Execute(Session.Uow, new GetQuery<Site>(id));
        }
        
        #region Commands

        private class DeleteSiteCommand : DeleteCommandBase<Site>
        {
            public DeleteSiteCommand(Site site)
                : base(new NodeSnippet<Site>(site))
            {

            }
        }

        private class PersistSiteCommand : CreateCommandBase<Site>
        {
            private readonly Site _site;

            public PersistSiteCommand(Site site)
                : base(new NodeSnippet<Site>(site))
            {
                _site = site;
            }

            protected override object Data()
            {
                var data = new
                {
                    _site.Id,
                    _site.Name
                };
                return data;
            }
        }

        private class AddOrganisationCommand : CreateReverseAssociationCommandBase<Site, Organisation>
        {
            public AddOrganisationCommand(Site site, Organisation organisation)
                : base(new ParamSnippet<Site>(site), new NodeSnippet<Organisation>(organisation), "SITE_OF")
            {

            }
        }

        private class RemoveOrganisationCommand : DeleteReverseAssociationCommandBase<Site, Organisation>
        {
            public RemoveOrganisationCommand(Site site, Organisation organisation)
                : base(new ParamSnippet<Site>(site), new NodeSnippet<Organisation>(organisation), "SITE_OF")
            {

            }
        }

        #endregion

        #region Queries

        private class AbstractGetQueryExecutor : AbstractGetQueryExecutor<Site, GetQueryDataset>
        {
            public AbstractGetQueryExecutor(GraphClient db) : base(db)
            {

            }

            protected override ICypherFluentQuery<GetQueryDataset> GetDataset(IGraphClient db, GetQuery<Site> query)
            {
                var dataset = db
                    .Cypher
                    .Match("(site:Site)")
                    .Where("site.Id in {id}")
                    .OptionalMatch("(site-[:SITE_OF]->organisation)")
                    .OptionalMatch("((assetTag:AssetTag)-[:BELONGS_TO]->site)")
                    .OptionalMatch("(site-[:SITE_OF]->(organisation:Organisation))")
                    .With("organisation, site, assetTag")
                    .WithParam("id", query.Id)
                    .ReturnDistinct((site, organisation, assetTag, assets, driverTags, assetTags, people) => new GetQueryDataset
                    {
                        Site = site.As<SiteNode>(),
                        Organisations = organisation.CollectAs<OrganisationNode>()
                    });
                return dataset;
            }

            protected override Site Create(GetQueryDataset item)
            {
                return item.Site.AsSite();
            }

            protected override void Map(Site aggregate, GetQueryDataset dataset)
            {
                aggregate.Name = dataset.Site.Name;
                foreach (var node in dataset.Organisations)
                {
                    aggregate.Add(node.AsOrganisation());
                }
            }
        }

        #endregion

        #region Datasets

        private class GetQueryDataset : QueryResultBase
        {
            public SiteNode Site { get; set; }

            public IEnumerable<OrganisationNode> Organisations { get; set; }
        }

        #endregion
    }
}
