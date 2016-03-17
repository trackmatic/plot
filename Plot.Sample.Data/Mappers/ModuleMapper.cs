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
                item.Name
            };
            return data;
        }

        protected override IQueryExecutor<Module> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }


        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<Module, ModuleResult>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {
            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct(module => new ModuleResult
                {
                    Module = module.As<ModuleNode>()
                });
            }
        }

        #endregion
    }
}
