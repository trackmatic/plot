namespace Plot
{
    public interface IGraphSessionFactory
    {
        IGraphSession OpenSession();
        
        void Register(IListener listener);
    }
}
