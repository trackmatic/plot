using System.Collections.Generic;
using Octo.Core.Attributes;

namespace Octo.Sample.Model
{
    public class Site
    {
        public Site()
        {
            Organisations = Organisations ?? new List<Organisation>();
        }

        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        [Relationship(Reverse = false, Name = Relationships.SiteOf)]
        public virtual IList<Organisation> Organisations { get; set; }

        public virtual void Add(Organisation organisation)
        {
            if (Organisations.Contains(organisation))
            {
                return;
            }
            Organisations.Add(organisation);
            organisation.Add(this);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Site;
            if (other == null)
            {
                return false;
            }
            return other.GetHashCode() == GetHashCode();
        }
    }
}
