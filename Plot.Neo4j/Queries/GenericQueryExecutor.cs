using Plot.Metadata;
using Plot.Neo4j.Cypher;

namespace Plot.Neo4j.Queries
{
    public abstract class GenericQueryExecutor<TAggregate, TResult> : AbstractQueryExecutor<TAggregate, TResult, GetAbstractQuery<TAggregate>>
        where TAggregate : class
        where TResult : ICypherQueryResult<TAggregate>
    {
        protected GenericQueryExecutor(ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(transactionFactory, metadataFactory)
        {
        }

        protected override ICypherFluentQuery<TResult> GetDataset(ICypherFluentQuery<TResult> query, GetAbstractQuery<TAggregate> abstractQuery)
        {
            var cypher = query.MatchById(Metadata);
            cypher = cypher.IncludeRelationships(Metadata);
            cypher = cypher.WithParam("id", abstractQuery.Id);
            return OnExecute((ICypherFluentQuery<TResult>)cypher);
        }

        protected abstract ICypherFluentQuery<TResult> OnExecute(ICypherFluentQuery<TResult> cypher);
    }
}
