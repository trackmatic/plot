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
    public class VehicleMapper : Mapper<Vehicle>
    {
        public VehicleMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {

        }

        protected override object GetData(Vehicle item)
        {
            return new VehicleNode(item);
        }

        protected override IQueryExecutor<Vehicle> CreateQueryExecutor()
        {
            return new GetQueryExecutor(Db, MetadataFactory);
        }

        #region Queries

        private class GetQueryExecutor : GenericQueryExecutor<Vehicle, VehicleResult>
        {
            public GetQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
            {

            }

            protected override ICypherFluentQuery OnExecute(ICypherFluentQuery cypher)
            {
                return cypher.ReturnDistinct(vehicle => new VehicleResult
                {
                    Vehicle= vehicle.As<VehicleNode>()
                });
            }
        }

        #endregion
    }
}