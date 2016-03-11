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
    public class ModuleMapper : Mapper<Module>
    {
        public ModuleMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {
        }

        protected override object GetData(Module item)
        {
            var data = new
            {
                item.Id,
                Name = item.Name
            };
            return data;
        }

        protected override IQueryExecutor<Module> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }


        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<Module, GetQueryDataset>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct(module => new GetQueryDataset
                {
                    Module = module.As<ModuleNode>()
                });
            }

            protected override Module Create(GetQueryDataset dataset)
            {
                var contact = dataset.Module.AsModule();
                return contact;
            }

            protected override void Map(Module aggregate, GetQueryDataset dataset)
            {
                aggregate.Name = dataset.Module.Name;
            }
        }

        #endregion

        #region Datasets

        private class GetQueryDataset : AbstractQueryResult
        {
            public ModuleNode Module { get; set; }
        }

        #endregion
    }
}
