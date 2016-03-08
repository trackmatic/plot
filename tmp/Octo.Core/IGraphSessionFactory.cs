namespace Octo.Core
{
    public interface IGraphSessionFactory
    {
        IGraphSession OpenSession();
        
        void Register(IListener listener);
    }
}
