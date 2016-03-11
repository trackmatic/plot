using System;
using Neo4jClient;
using Plot.Metadata;
using Plot.Neo4j;
using Plot.Queries;
using Plot.Sample.Model;

namespace Plot.Sample.Data.Mappers
{
    public class SitePermissionMapper : Mapper<SitePermission>
    {
        public SitePermissionMapper(GraphClient db, IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(db, session, transactionFactory, metadataFactory)
        {

        }

        protected override object GetData(SitePermission item)
        {
            var data = new
            {
                item.Id
            };
            return data;
        }

        protected override IQueryExecutor<SitePermission> CreateQueryExecutor()
        {
            throw new NotSupportedException();
        }
    }
}
