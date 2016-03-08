using Plot.Sample.Model;

namespace Plot.Sample.Nodes
{
    public class OrganisationNode
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Organisation AsOrganisation()
        {
            var organisation = new Organisation
            {
                Id = Id,

                Name = Name
            };

            return organisation;
        }
    }
}
