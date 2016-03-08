using System;

namespace Octo.Core
{
    public interface IRepository : IDisposable
    {
        void Store(object item);

        void Delete(object item);

        IMapper Mapper { get; }
    }
}
