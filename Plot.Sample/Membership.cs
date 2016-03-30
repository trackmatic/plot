using System.Collections.Generic;
using System.Linq;
using Plot.Attributes;

namespace Plot.Sample
{
    public class Membership : IRequireSession
    {
        private IGraphSession _session;

        public Membership()
        {
            ModulePermissions = new List<ModulePermission>();
        }

        public virtual string Id { get; set; }

        public virtual bool IsActive { get; set; }

        [Relationship(Relationships.MemberOf)]
        public virtual Organisation Organisation { get; set; }

        [Relationship(Relationships.GrantsAccessTo)]
        public virtual IList<ModulePermission> ModulePermissions { get; set; }

        [Relationship(Relationships.MemberOf, Reverse = true)]
        public virtual User User { get; set; }
        
        [Relationship(Relationships.CreatedBy)]
        public virtual User CreatedBy { get; set; }
        
        public virtual void Add(ModulePermission modulePermission)
        {
            Utils.Add(ModulePermissions, modulePermission, () => modulePermission.Membership = this);
        }

        public virtual void Remove(ModulePermission modulePermission)
        {
            Utils.Remove(ModulePermissions, modulePermission, () => modulePermission.Membership = null);
        }

        public override int GetHashCode()
        {
            return Utils.GetHashCode(Id);
        }

        public virtual void Set(IGraphSession session)
        {
            _session = session;
        }

        public virtual IEnumerable<ModulePermission> GetModulePermissions()
        {
            var ids = ModulePermissions.Select(x => x.Id).ToArray();
            return _session.Get<ModulePermission>(ids);
        }

        public override bool Equals(object obj)
        {
            return Utils.Equals(this, obj);
        }
    }
}
