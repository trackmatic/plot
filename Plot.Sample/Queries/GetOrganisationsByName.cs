using Plot.Queries;
using Plot.Sample.Model;

namespace Plot.Sample.Queries
{
    public class GetOrganisationsByName : AbstractQuery<Organisation>
    {
        public GetOrganisationsByName()
        {
            OrderBy = new[] {"organisation.Name"};
        }

        public string Name { get; set; }
    }
}
