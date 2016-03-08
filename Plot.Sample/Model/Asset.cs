using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample.Model
{
    public class Asset : IRequireSession
    {
        private IGraphSession _session;

        public Asset()
        {
            Sites = Sites ?? new List<Site>();
        }

        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        [Relationship(Relationships.BelongsTo)]
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

        public virtual void Set(IGraphSession session)
        {
            _session = session;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Asset;
            if (other == null)
            {
                return false;
            }
            return other.GetHashCode() == GetHashCode();
        }
    }
}
