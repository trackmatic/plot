using System.Collections.Generic;
using Neo4jClient;
using Neo4jClient.Cypher;
using Plot.Metadata;
using Plot.Proxies;

namespace Plot.Neo4j.Queries
{
    public abstract class GenericQueryExecutor<TAggregate, TDataset> : AbstractQueryExecutor<TAggregate, TDataset, GetAbstractQuery<TAggregate>>
        where TAggregate : class
        where TDataset : ICypherQueryResult<TAggregate>
    {
        protected GenericQueryExecutor(GraphClient db, IMetadataFactory metadataFactory) : base(db, metadataFactory)
        {
        }

        protected override ICypherFluentQuery<TDataset> GetDataset(IGraphClient db, GetAbstractQuery<TAggregate> abstractQuery)
        {
            var cypher = db.Cypher.MatchById(Metadata);
            cypher = cypher.IncludeRelationships(Metadata);
            cypher = cypher.WithParam("id", abstractQuery.Id);
            return (ICypherFluentQuery<TDataset>)OnExecute(cypher);
        }

        protected abstract ICypherFluentQuery OnExecute(ICypherFluentQuery cypher);
    }
}
