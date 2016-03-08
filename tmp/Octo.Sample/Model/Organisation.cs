using System.Collections.Generic;
using Octo.Core.Attributes;

namespace Octo.Sample.Model
{
    public class Organisation
    {
        public Organisation()
        {
            Sites = Sites ?? new List<Site>();
        }

        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        [Relationship(Reverse = true, Name = Relationships.SiteOf)]
        public virtual IList<Site> Sites { get; set; }

        public virtual void Add(Site site)
        {
            if (Sites.Contains(site))
            {
                return;
            }
            Sites.Add(site);
            site.Add(this);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Organisation;
            if (other == null)
            {
                return false;
            }
            return other.GetHashCode() == GetHashCode();
        }
    }
}
