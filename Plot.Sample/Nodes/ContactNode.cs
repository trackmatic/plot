using Plot.Sample.Model;

namespace Plot.Sample.Nodes
{
    public class ContactNode
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public Contact AsContact()
        {
            var contact = new Contact
            {
                Id = Id,
                Name = Name
            };
            return contact;
        }
    }
}
