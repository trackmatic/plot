using System;

namespace Plot.Sample.Data.Nodes
{
    public class InvitationNode
    {
        public InvitationNode()
        {
            
        }

        public InvitationNode(Invitation item)
        {
            Id = item.Id;
            Email = item.Email;
            Created = item.Created;
            ValidUntil = item.ValidUntil;
            Accepted = item.Accepted;
            AcceptedOn = item.AcceptedOn;
        }

        public string Id { get; set; }

        public string Email { get; set; }

        public DateTime Created { get; set; }

        public DateTime ValidUntil { get; set; }

        public bool Accepted { get; set; }

        public DateTime? AcceptedOn { get; set; }

        public Invitation AsInvitation()
        {
            return new Invitation
            {
                Id = Id,
                Email = Email,
                Created = Created,
                ValidUntil = ValidUntil,
                AcceptedOn = AcceptedOn,
                Accepted = Accepted
            };
        }
    }
}
