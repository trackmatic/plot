namespace Octo.Core
{
    public interface IListener<in T> : IListener
    {
        void Update(T item, IGraphSession session);

        void Delete(T item, IGraphSession session);

        void Create(T item, IGraphSession session);
    }
}
