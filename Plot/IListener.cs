namespace Plot
{
    public interface IListener
    {
        void Update(object item, IGraphSession session);

        void Delete(object item, IGraphSession session);

        void Create(object item, IGraphSession session);
    }
}
