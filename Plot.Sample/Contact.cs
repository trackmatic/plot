using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample
{
    public class Contact
    {
        public Contact()
        {
            Sites = new List<Site>();
        }

        public virtual string Id { get; set; }

        public virtual Names Names { get; set; }

        public virtual string Email { get; set; }

        public virtual string WorkNo { get; set; }

        public virtual string MobileNo { get; set; }

        public virtual string Type { get; set; }

        [Relationship(Relationships.InterestedIn)]
        public virtual IList<Site> Sites { get; set; }
    }
}
