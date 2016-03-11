using System.Collections.Generic;
using Plot.Attributes;

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

        [Relationship(Relationships.GrantAccessTo)]
        public virtual Module Module { get; set; }

        [Relationship(Relationships.GrantAccessTo)]
        public virtual IList<Role> Roles { get; set; }

        [Relationship(Relationships.GrantAccessTo)]
        public virtual IList<SitePermission> Sites { get; set; }

        public virtual void Add(SitePermission permission)
        {
            if (Sites.Contains(permission))
            {
                return;
            }
            Sites.Add(permission);
        }

        public virtual void Add(Role role)
        {
            if (Roles.Contains(role))
            {
                return;
            }
            Roles.Add(role);
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
