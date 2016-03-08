using Xunit;

namespace Plot.Tests
{
    public class dependencies_tests
    {
        [Fact]
        public void register_causes_the_caller_dependency_to_increment()
        {
            var one = new Dependencies();
            Assert.Equal(1, one.Sequence);

            var two = new Dependencies();
            one.Register(two);
            Assert.Equal(2, two.Sequence);
            Assert.Equal(1, one.Sequence);

            var another_two = new Dependencies();
            one.Register(another_two);
            Assert.Equal(2, another_two.Sequence);
            Assert.Equal(1, one.Sequence);

            var three = new Dependencies();
            two.Register(three);
            Assert.Equal(3, three.Sequence);
        }
    }
}
