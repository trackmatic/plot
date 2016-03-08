using System;

namespace Plot
{
    public interface IRepository : IDisposable
    {
        void Create(object item);

        void Delete(object item);

        IMapper Mapper { get; }
    }
}
