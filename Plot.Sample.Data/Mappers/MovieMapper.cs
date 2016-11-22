using Plot.Metadata;
using Plot.Neo4j;
using Plot.Queries;
using Plot.Sample.Data.Nodes;
using Plot.Sample.Data.Results;

namespace Plot.Sample.Data.Mappers
{
    public class MovieMapper : Mapper<Movie>
    {
        public MovieMapper(IGraphSession session, ICypherTransactionFactory transactionFactory, IMetadataFactory metadataFactory) 
            : base(session, transactionFactory, metadataFactory)
        {

        }

        protected override object GetData(Movie item)
        {
            return new MovieNode(item);
        }

        protected override IQueryExecutor<Movie> CreateQueryExecutor()
        {
            return CreateGenericExecutor(ResultFactory.CreateMovieResult, ResultFactory.CreateMovieResultReturn);
        }
    }
}