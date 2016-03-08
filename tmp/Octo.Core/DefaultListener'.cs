namespace Octo.Core
{
    public abstract class DefaultListener<T> : IListener<T>
    {
        public void Update(T item, IGraphSession session)
        {
            OnUpdated(item, session);
        }

        public void Delete(T item, IGraphSession session)
        {
            OnDeleted(item, session);
        }

        public void Create(T item, IGraphSession session)
        {
            OnCreated(item, session);
        }

        public void Update(object item, IGraphSession session)
        {
            Update((T)item, session);
        }

        public void Delete(object item, IGraphSession session)
        {
            Delete((T)item, session);
        }

        public void Create(object item, IGraphSession session)
        {
            Create((T)item, session);
        }

        protected abstract void OnUpdated(T item, IGraphSession session);

        protected abstract void OnDeleted(T item, IGraphSession session);

        protected abstract void OnCreated(T item, IGraphSession session);
    }
}
