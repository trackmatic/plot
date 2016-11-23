using Plot.Queries;

namespace Plot.Sample.Queries
{
    public class GetMoviesByTitle : AbstractQuery<Movie>
    {
        public GetMoviesByTitle()
        {

            OrderBy = new[] { "movie.Title" };
        }

        public string Term { get; set; }
    }
}
