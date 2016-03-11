using System;

namespace Plot
{
    public interface IRepository : IDisposable
    {
        object Create(object item);

        void Delete(object item);

        IMapper Mapper { get; }
    }
}
