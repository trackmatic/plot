using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample.Model
{
    public class SitePermission
    {
        public SitePermission()
        {
            AccessGroups = new List<AccessGroup>();
        }

        public virtual string Id { get; set; }

        [Relationship(Relationships.GrantAccessTo)]
        public virtual Site Site { get; set; }

        [Relationship(Relationships.GrantAccessTo)]
        public virtual IList<AccessGroup> AccessGroups { get; set; }

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
