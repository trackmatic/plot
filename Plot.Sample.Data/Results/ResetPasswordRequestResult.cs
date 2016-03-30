using Plot.Neo4j.Queries;
using Plot.Sample.Data.Nodes;

namespace Plot.Sample.Data.Results
{
    public class ResetPasswordRequestResult : AbstractCypherQueryResult<ResetPasswordRequest>
    {
        public ResetPasswordRequestNode ResetPasswordRequest { get; set; }

        public UserNode User { get; set; }

        public PasswordNode Password { get; set; }

        public UserNode RequestedBy { get; set; }

        public override void Map(ResetPasswordRequest aggregate)
        {
            aggregate.User = User.AsUser();
            aggregate.RequestedBy = RequestedBy.AsUser();
            aggregate.Password = Password?.AsPassword();
        }

        public override ResetPasswordRequest Create()
        {
            return ResetPasswordRequest.AsResetPasswordRequest();
        }
    }
}
