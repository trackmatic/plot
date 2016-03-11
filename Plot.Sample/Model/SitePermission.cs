using System.Collections.Generic;

namespace Plot.Sample.Model
{
    public class SitePermission
    {
        public SitePermission()
        {
            AccessGroups = AccessGroups ?? new List<AccessGroup>();
        }

        public virtual string Id { get; set; }

        public virtual Site Site { get; set; }

        public virtual List<AccessGroup> AccessGroups { get; set; }

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
