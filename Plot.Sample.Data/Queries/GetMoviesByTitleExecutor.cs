using Plot.Metadata;
using Plot.Neo4j;
using Plot.Neo4j.Cypher;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Params;
using Plot.Sample.Data.Results;
using Plot.Sample.Queries;

namespace Plot.Sample.Data.Queries
{
    public class GetMoviesByTitleExecutor : AbstractQueryExecutor<Movie, MovieResult, GetMoviesByTitle>
    {
        public GetMoviesByTitleExecutor(ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(transactionFactory, metadataFactory)
        {
        }

        protected override ICypherQuery<MovieResult> GetDataset(ICypherQuery<MovieResult> db, GetMoviesByTitle query)
        {
            var elements = new IQueryBuilderElement[]
            {
                new Body(query, Metadata),
                new Parameters(new Term(() => query.Term))
            };
            return QueryBuilder.Create(db, elements).AsTypedQuery<MovieResult>().ReturnDistinct(MovieResult.Map, MovieResult.Return);
        }

        private class Body : IQueryBuilderElement
        {
            private readonly NodeMetadata _nodeMetadata;
            private readonly GetMoviesByTitle _query;

            public Body(GetMoviesByTitle query, NodeMetadata nodeMetadata)
            {
                _query = query;
                _nodeMetadata = nodeMetadata;
            }

            public ICypherQuery Append(ICypherQuery cypher)
            {
                cypher = cypher.Match("(movie:Movie)");
                cypher = Matching(cypher);
                cypher = cypher.IncludeRelationships(_nodeMetadata);
                return cypher;
            }

            private ICypherQuery Matching(ICypherQuery cypher)
            {
                if (string.IsNullOrEmpty(_query.Term))
                {
                    return cypher;
                }
                return cypher.Where($"movie.Title =~ {Term.Key}");
            }
        }
    }
}
