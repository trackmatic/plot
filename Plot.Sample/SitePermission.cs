using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample
{
    public class SitePermission
    {
        public SitePermission()
        {
            AccessGroups = new List<AccessGroup>();
        }

        public virtual string Id { get; set; }

        [Relationship(Relationships.GrantsAccessTo)]
        public virtual Site Site { get; set; }

        [Relationship(Relationships.GrantsAccessTo)]
        public virtual IList<AccessGroup> AccessGroups { get; set; }

        public virtual void AddRange(IEnumerable<AccessGroup> accessGroups)
        {
            foreach (var accessGroup in accessGroups)
            {
                Add(accessGroup);
            }
        }

        public virtual void Add(AccessGroup accessGroup)
        {
            Utils.Add(AccessGroups, accessGroup);
        }

        public virtual void Remove(AccessGroup accessGroup)
        {
            Utils.Remove(AccessGroups, accessGroup);
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
