using System;

namespace Plot
{
    public interface IMapper
    {
        Type Type { get; }

        void Insert(object item, EntityState state);

        void Delete(object item, EntityState state);

        void Update(object item, EntityState state);
    }
}
