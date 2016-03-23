using System.Collections.Generic;
using Moq;
using Plot.Tests.Model;
using Plot.Tests.Utility;
using Xunit;

namespace Plot.Tests
{
    public class graph_session_tests : IClassFixture<GraphSessionTestsFixture>
    {
        private readonly GraphSessionTestsFixture _fixture;

        public graph_session_tests(GraphSessionTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void dirty_tracking_should_cause_udpate()
        {
            _fixture.Setup();
            using (var session = _fixture.Factory.OpenSession())
            {
                var person = session.Get<Person>("1");
                person.Name = "Ross";
                person.Contacts[0].Name = "New Name";
                session.SaveChanges();
            }
            _fixture.GetMapper<Person>().Verify();
        }

        [Fact]
        public void listeners_are_called_when_registered()
        {
            _fixture.Setup();
            var listener = new Mock<IListener<Person>>();
            listener.Setup(x => x.Create(It.IsAny<object>(), It.IsAny<IGraphSession>())).Verifiable();
            _fixture.Factory.Register(listener.Object);
            using (var session = _fixture.Factory.OpenSession())
            {
                session.Create(new Person
                {
                    Id = "1"
                });
                session.SaveChanges();
                listener.Verify();
            }
        }
    }

    public class GraphSessionTestsFixture : MapperFixtureBase
    {
        protected override void Configure()
        {
            Create<Person>().Setup(x => x.Get(It.IsAny<string[]>())).Returns(new List<Person> { Target });
            Create<Person>().Setup(x => x.Update(It.IsAny<object>(), It.IsAny<EntityState>())).Verifiable();
            Create<Contact>().Setup(x => x.Get(It.IsAny<string[]>())).Returns(new List<Contact> { new Contact { Id = "1" } });
        }

        public Person Target => new Person
        {
            Id = "1",
            Contacts = new List<Contact>
            {
                new Contact {Id = "1"}
            }
        };
    }
}
