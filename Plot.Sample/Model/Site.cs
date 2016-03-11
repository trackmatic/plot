using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample.Model
{
    public class Site
    {
        public Site()
        {
            Assets = Assets ?? new List<Asset>();
        }

        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        [Relationship(Relationships.Operates)]
        public virtual IList<Asset> Assets { get; set; }

        [Relationship(Relationships.Runs, Reverse = true)]
        public virtual Organisation Organisation { get; set; }

        public virtual void Set(Organisation organisation)
        {
            if (Organisation == organisation)
            {
                return;
            }
            Organisation = organisation;
            organisation?.Add(this);
        }

        public virtual void Add(Asset asset)
        {
            if (Assets.Contains(asset))
            {
                return;
            }
            Assets.Add(asset);
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