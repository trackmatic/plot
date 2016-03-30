using System;
using System.Collections.Generic;
using System.Linq;
using Plot.Attributes;

namespace Plot.Sample
{
    [Serializable]
    public class AccessGroup : IRequireSession
    {
        private IGraphSession _session;

        public AccessGroup()
        {
            Assets = new List<Asset>();
            ModulePermissions = new List<ModulePermission>();
        }

        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        [Relationship(Relationships.RestrictsAccessTo)]
        public virtual IList<Asset> Assets { get; set; }

        [Relationship(Relationships.GrantsAccessTo, Reverse = true)]
        public virtual IList<ModulePermission> ModulePermissions { get; set; }

        public virtual void Add(Asset asset)
        {
            Utils.Add(Assets, asset, () => asset.Add(this));
        }

        public virtual void Remove(Asset asset)
        {
            Utils.Remove(Assets, asset, () => asset.Remove(this));
        }

        public virtual void Add(ModulePermission modulePermission)
        {
            Utils.Add(ModulePermissions, modulePermission, () => modulePermission.Add(this));
        }

        public virtual void Remove(ModulePermission modulePermission)
        {
            Utils.Remove(ModulePermissions, modulePermission, () => modulePermission.Remove(this));
        }
        public virtual IEnumerable<ModulePermission> GetModulePermissions()
        {
            var ids = ModulePermissions.Select(x => x.Id).ToArray();
            return _session.Get<ModulePermission>(ids);
        }

        public override int GetHashCode()
        {
            return Utils.GetHashCode(Id);
        }

        public void Set(IGraphSession session)
        {
            _session = session;
        }

        public override bool Equals(object obj)
        {
            return Utils.Equals(this, obj);
        }
    }
}
