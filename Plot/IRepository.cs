using System;

namespace Plot
{
    public interface IRepository : IDisposable
    {
        void Store(object item);

        void Delete(object item);

        IMapper Mapper { get; }
    }
}
