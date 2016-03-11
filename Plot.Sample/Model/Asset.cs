using System.Collections.Generic;

namespace Plot.Sample.Model
{
    public class Asset
    {
        public Asset()
        {
            Sites = Sites ?? new List<Site>();
        }

        public virtual string Id { get; set; }

        public virtual string FleetNumber { get; set; }

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
            return ProxyUtils.GetHashCode(Id);
        }

        public override bool Equals(object obj)
        {
            return Utils.Equals(this, obj);
        }
    }
}
