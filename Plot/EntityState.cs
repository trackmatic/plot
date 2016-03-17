using System;

namespace Plot
{
    public class EntityState
    {
        private EntityStatus _status;

        private bool _readonly;

        private readonly LazyResolver<IGraphSession> _session;

        private bool _populated;

        private readonly string _id;

        public EntityState(string id)
        {
            _id = id;
            _status = EntityStatus.Clean;
            _session = new LazyResolver<IGraphSession>();
            Dependencies = new Dependencies(id);
        }

        public void Inject(IGraphSession session)
        {
            _session.Ready(session);
        }

        public LazyResolver<IGraphSession> Session => _session;

        public bool IsPopulated => _populated;

        protected void MarkClean()
        {
            if (_status == EntityStatus.New)
            {
                throw new InvalidOperationException("An item cannot be marked as clean if it has already been marked as new");
            }

            _status = EntityStatus.Clean;
        }

        protected void MarkNew()
        {
            _status = EntityStatus.New;
        }

        protected void MarkDirty()
        {
            if (_readonly)
            {
                return;
            }

            if (_status == EntityStatus.New)
            {
                return;
            }

            if (_status == EntityStatus.Deleted)
            {
                return;
            }

            _status = EntityStatus.Dirty;
        }

        protected void MarkDeleted()
        {
            _status = EntityStatus.Deleted;
        }

        public EntityStatus Status => _status;

        public Dependencies Dependencies { get; }

        public void Delete()
        {
            MarkDeleted();
        }

        public void New()
        {
            MarkNew();
        }

        public void Clean()
        {
            if (Status == EntityStatus.New)
            {
                return;
            }
            MarkClean();
        }

        public void Dirty()
        {
            MarkDirty();
        }

        public void Readonly()
        {
            MarkClean();
            _readonly = true;
        }

        public bool IsReadonly => _readonly;

        public void Populate()
        {
            _populated = true;
            _readonly = false;
            Clean();
        }

        public string GetIdentifier()
        {
            return _id;
        }

        internal bool IsLocked { get; private set; }

        internal void Lock()
        {
            IsLocked = true;
        }

        internal void Unlock()
        {
            IsLocked = false;
        }

        internal void Set(EntityStatus status)
        {
            switch (status)
            {
                case EntityStatus.Clean:
                    MarkClean();
                    break;
                case EntityStatus.Deleted:
                    Delete();
                    break;
                case EntityStatus.Dirty:
                    Dirty();
                    break;
                case EntityStatus.New:
                    New();
                    break;
            }
        }
    }
}