using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample
{
    public class Membership
    {
        public Membership()
        {
            AccessGroups = new List<AccessGroup>();
            ModulePermissions = new List<ModulePermission>();
        }

        public virtual string Id { get; set; }

        public virtual bool IsActive { get; set; }

        [Relationship(Relationships.MemberOf)]
        public virtual Organisation Organisation { get; set; }

        [Relationship(Relationships.GrantsAccessTo)]
        public virtual IList<AccessGroup> AccessGroups { get; set; }

        [Relationship(Relationships.GrantsAccessTo)]
        public virtual IList<ModulePermission> ModulePermissions { get; set; }

        [Relationship(Relationships.MemberOf, Reverse = true)]
        public virtual User User { get; set; }

        public virtual void Add(ModulePermission modulePermission)
        {
            Utils.Add(ModulePermissions, modulePermission, () => modulePermission.Membership = this);
        }

        public virtual void Remove(ModulePermission modulePermission)
        {
            Utils.Remove(ModulePermissions, modulePermission, () => modulePermission.Membership = null);
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
