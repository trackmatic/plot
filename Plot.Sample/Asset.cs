using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample
{
    public class Asset
    {
        public Asset()
        {
            Sites = new List<Site>();
            AccessGroups = new List<AccessGroup>();
        }

        public virtual string Id { get; set; }

        public virtual string FleetNumber { get; set; }

        public virtual string Reference { get; set; }
        
        [Relationship(Relationships.Operates, Reverse = true)]
        public virtual IList<Site> Sites { get; set; }

        [Relationship(Relationships.RestrictsAccessTo, Reverse = true)]
        public virtual IList<AccessGroup> AccessGroups { get; set; }

        [Relationship(Relationships.IsA, DeleteOrphan = true, Lazy = true)]
        public virtual AssetType Type { get; set; }

        public virtual void Add(Site site)
        {
            Utils.Add(Sites, site, () => site.Add(this));

        }
        public virtual void Remove(Site site)
        {
            Utils.Remove(Sites, site, () => site.Remove(this));
        }

        public virtual void Add(AccessGroup accessGroup)
        {
            Utils.Add(AccessGroups, accessGroup, () => accessGroup.Add(this));
        }

        public virtual void Remove(AccessGroup accessGroup)
        {
            Utils.Remove(AccessGroups, accessGroup, () => accessGroup.Remove(this));
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
