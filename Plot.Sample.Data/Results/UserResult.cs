using System.Collections.Generic;
using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{
    public class UserResult : AbstractCypherQueryResult<User>
    {
        public UserNode User { get; set; }

        public PersonNode Person { get; set; }

        public IEnumerable<MembershipNode> Memberships { get; set; }

        public PasswordNode Password { get; set; }

        public override void Map(User aggregate)
        {
            aggregate.Person = Person.AsPerson();
            Memberships.Map(x => aggregate.Add(x.AsMembership()));
            aggregate.Password = Password?.AsPassword();
        }

        public override User Create()
        {
            return User.AsUser();
        }
    }
}