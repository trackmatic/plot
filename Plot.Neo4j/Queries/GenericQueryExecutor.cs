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

        protected override ICypherQuery<TResult> GetDataset(ICypherQuery<TResult> query, GetAbstractQuery<TAggregate> abstractQuery)
        {
            var cypher = MatchById(query, Metadata);
            cypher = cypher.IncludeRelationships(Metadata);
            cypher = cypher.WithParam("id", abstractQuery.Id);
            return OnExecute((ICypherQuery<TResult>)cypher);
        }


        public static ICypherQuery MatchById(ICypherQuery cypher, NodeMetadata metadata)
        {
            return cypher.Match($"({Conventions.NamedParameterCase(metadata.Name)}:{metadata.Name})").Where($"{Conventions.NamedParameterCase(metadata.Name)}.Id in {{id}}");
        }

        protected abstract ICypherQuery<TResult> OnExecute(ICypherQuery<TResult> cypher);
    }
}
