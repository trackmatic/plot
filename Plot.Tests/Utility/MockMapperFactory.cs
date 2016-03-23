using Moq;

namespace Plot.Tests.Utility
{
    public static class MockMapperFactory
    {
        public static Mock<IMapper<T>> Create<T>()
        {
            var mapper = new Mock<IMapper<T>>();
            mapper.Setup(x => x.Type).Returns(typeof(T));
            return mapper;
        }
    }
}
