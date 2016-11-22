using Neo4j.Driver.V1;

namespace Plot.Sample.Data.Nodes
{
    public class MovieNode
    {
        public MovieNode(INode node)
        {
            Id = node["Id"].As<string>();
            Title = node["Title"].As<string>();
            Tagline = node["Tagline"].As<string>();
        }

        public MovieNode(Movie item)
        {
            Id = item.Id;
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
                Id = Id,
                TagLine = Tagline,
                Title = Title
            };
            return organisation;
        }
    }
}
