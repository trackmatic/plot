using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample
{
    public class ModulePermission
    {
        public ModulePermission()
        {
            Roles = new List<Role>();
            SitePermissions = new List<SitePermission>();
        }

        public virtual string Id { get; set; }

        [Relationship(Relationships.GrantsAccessTo)]
        public virtual Module Module { get; set; }

        [Relationship(Relationships.AsRole)]
        public virtual IList<Role> Roles { get; set; }

        [Relationship(Relationships.GrantsAccessTo, DeleteOrphan = true)]
        public virtual IList<SitePermission> SitePermissions { get; set; }
        
        [Relationship(Relationships.GrantsAccessTo, Reverse = true)]
        public virtual Membership Membership { get; set; }

        public virtual void Clear()
        {
            foreach (var sitePermission in SitePermissions)
            {
                Remove(sitePermission);
            }

            foreach (var role in Roles)
            {
                Remove(role);
            }
        }

        public virtual void Add(SitePermission sitePermission)
        {
            Utils.Add(SitePermissions, sitePermission);
        }

        public virtual void Add(Role role)
        {
            Utils.Add(Roles, role);
        }

        public virtual void Remove(SitePermission sitePermission)
        {
            Utils.Remove(SitePermissions, sitePermission);
        }

        public virtual void Remove(Role role)
        {
            Utils.Remove(Roles, role);
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
