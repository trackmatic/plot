using Neo4j.Driver.V1;
using Plot.Neo4j;

namespace Plot.Sample.Data.Nodes
{
    public class MovieNode
    {
        public MovieNode(INode node)
        {
            Id = node.Read<string>(Keys.Id);
            Title = node.Read<string>(Keys.Title);
            Tagline = node.Read<string>(Keys.Tagline);
        }

        public MovieNode(Movie item)
        {
            Id = item.Id.Value;
            Tagline = item.TagLine;
            Title = item.Title;
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Tagline { get; set; }

        public Movie AsMovie()
        {
            var organisation = new Movie
            {
                Id = new MovieId(Id),
                TagLine = Tagline,
                Title = Title
            };
            return organisation;
        }

        private static class Keys
        {
            public const string Id = "Id";
            public const string Tagline = "Tagline";
            public const string Title = "Title";
        }
    }
}
