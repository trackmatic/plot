using System;

namespace Octo.Core
{
    public interface IRepositoryFactory
    {
        IRepository<T> Create<T>(IGraphSession session);

        IRepository Create(IGraphSession session, Type type);
    }
}
