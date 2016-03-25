using Plot.Queries;

namespace Plot.Sample.Queries
{
    public class GetSitesByOrganisation : AbstractQuery<Site>
    {
        public GetSitesByOrganisation(string organisationId, string userId)
        {
            OrganisationId = organisationId;
            UserId = userId;
            OrderBy = new[] { "site.Name" };
        }

        public string OrganisationId { get; set; }

        public string UserId { get; set; }

        public string Term { get; set; }

        public GetSitesByOrganisation With(string[] order)
        {
            if (order == null)
            {
                return this;
            }
            OrderBy = order;
            return this;
        }
    }
}
