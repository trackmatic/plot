using System.Collections.Generic;

namespace Plot.Sample.Model
{
    public class ModulePermission
    {
        public ModulePermission()
        {
            Roles = Roles ?? new List<Role>();
            Sites = Sites ?? new List<SitePermission>();
        }

        public virtual string Id { get; set; }

        public virtual Module Module { get; set; }

        public virtual IList<Role> Roles { get; set; }

        public virtual IList<SitePermission> Sites { get; set; }

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
