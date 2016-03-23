using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Plot.Testing;

namespace Plot.Tests.Utility
{
    public abstract class MapperFixtureBase
    {
        private readonly IDictionary<Type, object> _mappers = new ConcurrentDictionary<Type, object>();

        private IGraphSessionFactory _graphSessionFactory;

        public Mock<IMapper<T>> GetMapper<T>()
        {
            return (Mock<IMapper<T>>)_mappers[typeof(T)];
        }

        public Mock<IMapper<T>> Create<T>()
        {
            if (_mappers.ContainsKey(typeof (T)))
            {
                return GetMapper<T>();
            }
            var mapper = MockMapperFactory.Create<T>();
            _mappers.Add(typeof(T), mapper);
            return mapper;
        }

        public virtual void Setup()
        {
            _mappers.Clear();
            Configure();
            _graphSessionFactory = Configuration.CreateTestSessionFactory(_mappers.Values.Cast<Mock>().Select(x => x.Object).Cast<IMapper>().ToArray());
        }

        public IGraphSessionFactory Factory => _graphSessionFactory;

        protected abstract void Configure();
    }
}
