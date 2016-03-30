using System.Collections.Generic;
using Plot.Attributes;

namespace Plot.Sample
{
    public class ModulePermission
    {
        public ModulePermission()
        {
            Roles = new List<Role>();
            Sites = new List<Site>();
            AccessGroups = new List<AccessGroup>();
        }

        public virtual string Id { get; set; }

        [Relationship(Relationships.GrantsAccessTo)]
        public virtual Module Module { get; set; }

        [Relationship(Relationships.AsRole)]
        public virtual IList<Role> Roles { get; set; }

        [Relationship(Relationships.GrantsAccessTo, DeleteOrphan = true)]
        public virtual IList<Site> Sites { get; set; }
        
        [Relationship(Relationships.GrantsAccessTo, Reverse = true)]
        public virtual Membership Membership { get; set; }

        [Relationship(Relationships.GrantsAccessTo)]
        public virtual IList<AccessGroup> AccessGroups { get; set; }

        public virtual void AddRange(IEnumerable<AccessGroup> accessGroups)
        {
            foreach (var accessGroup in accessGroups)
            {
                Add(accessGroup);
            }
        }

        public virtual void Add(Site site)
        {
            Utils.Add(Sites, site);
        }

        public virtual void Add(Role role)
        {
            Utils.Add(Roles, role);
        }

        public virtual void Add(AccessGroup accessGroup)
        {
            Utils.Add(AccessGroups, accessGroup);
        }

        public virtual void Remove(AccessGroup accessGroup)
        {
            Utils.Remove(AccessGroups, accessGroup);
        }

        public virtual void Remove(Site site)
        {
            Utils.Remove(Sites, site);
        }

        public virtual void Remove(Role role)
        {
            Utils.Remove(Roles, role);
        }
        
        public virtual void Clear()
        {
            foreach (var site in Sites)
            {
                Remove(site);
            }

            foreach (var role in Roles)
            {
                Remove(role);
            }
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
