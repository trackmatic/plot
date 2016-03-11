using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample.Model
{
    public class Organisation
    {
        public Organisation()
        {
            Sites = Sites ?? new List<Site>();
            AccessGroups = AccessGroups ?? new List<AccessGroup>();
        }

        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        [Relationship(Relationships.Runs)]
        public virtual IList<Site> Sites { get; set; }

        [Relationship(Relationships.Maintains)]
        public virtual IList<AccessGroup> AccessGroups { get; set; }

        public virtual void Add(AccessGroup accessGroup)
        {
            if (AccessGroups.Contains(accessGroup))
            {
                return;
            }
            AccessGroups.Add(accessGroup);
        }

        public virtual void Add(Site site)
        {
            if (Sites.Contains(site))
            {
                return;
            }
            Sites.Add(site);
            site?.Set(this);
        } 

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Utils.Equals(this, obj);
        }
    }
}
