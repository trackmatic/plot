using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample.Model
{
    public class Site
    {
        public Site()
        {
            Organisations = Organisations ?? new List<Organisation>();
            Assets = Assets ?? new List<Asset>();
        }

        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        [Relationship(Relationships.SiteOf)]
        public virtual IList<Organisation> Organisations { get; set; }

        [Relationship(Relationships.BelongsTo, true)]
        public virtual IList<Asset> Assets { get; set; }

        public virtual void Add(Organisation organisation)
        {
            if (Organisations.Contains(organisation))
            {
                return;
            }
            Organisations.Add(organisation);
            organisation.Add(this);
        }

        public virtual void Add(Asset asset)
        {
            if (Assets.Contains(asset))
            {
                return;
            }
            Assets.Add(asset);
            asset.Add(this);
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
