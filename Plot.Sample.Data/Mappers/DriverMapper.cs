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
    public class DriverMapper : Mapper<Driver>
    {
        public DriverMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {

        }

        protected override object GetData(Driver item)
        {
            return new DriverNode(item);
        }

        protected override IQueryExecutor<Driver> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }

        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<Driver, DriverResult>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {

            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct(driver => new DriverResult
                {
                    Driver = driver.As<DriverNode>()
                });
            }
        }

        #endregion
    }
}