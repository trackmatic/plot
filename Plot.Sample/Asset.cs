using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample
{
    public class Asset
    {
        public Asset()
        {
            Sites = new List<Site>();
        }

        public virtual string Id { get; set; }

        public virtual string FleetNumber { get; set; }

        [Relationship(Relationships.Maintains)]
        public virtual IList<Site> Sites { get; set; }

        [Relationship(Relationships.IsA)]
        public virtual AssetType Type { get; set; }

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
            return Utils.GetHashCode(Id);
        }

        public override bool Equals(object obj)
        {
            return Utils.Equals(this, obj);
        }
    }
}
