using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{
    public class InvitationResult : AbstractCypherQueryResult<Invitation>
    {
        public MembershipNode Membership { get; set; }

        public UserNode AcceptedBy { get; set; }

        public UserNode IssuedBy { get; set; }

        public InvitationNode Invitation { get; set; }

        public override void Map(Invitation aggregate)
        {
            aggregate.AcceptedBy = AcceptedBy?.AsUser();
            aggregate.Membership = Membership.AsMembership();
            aggregate.IssuedBy = IssuedBy?.AsUser();
        }

        public override Invitation Create()
        {
            return Invitation.AsInvitation();
        }
    }
}
