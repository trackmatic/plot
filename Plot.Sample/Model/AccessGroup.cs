using System;
using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample.Model
{
    [Serializable]
    public class AccessGroup
    {
        public AccessGroup()
        {
            Assets = Assets ?? new List<Asset>();
        }

        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        [Relationship(Relationships.RestrictsAccessTo)]
        public virtual IList<Asset> Assets { get; set; }

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
