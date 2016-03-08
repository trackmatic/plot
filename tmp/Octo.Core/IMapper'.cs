using System.Collections.Generic;

namespace Octo.Core
{
    public interface IMapper<T> : IMapper
    {
        void Insert(T item, EntityState state);

        void Delete(T item, EntityState state);

        void Update(T item, EntityState state);
        
        IEnumerable<T> Get(string[] id);
    }
}
