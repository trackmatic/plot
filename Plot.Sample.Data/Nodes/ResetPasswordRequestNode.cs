using System;

namespace Plot.Sample.Data.Nodes
{
    public class ResetPasswordRequestNode
    {
        public ResetPasswordRequestNode()
        {
            
        }

        public ResetPasswordRequestNode(ResetPasswordRequest item)
        {
            Id = item.Id;
            Created = item.Created;
            ValidUntil = item.ValidUntil;
            IsComplete = item.IsComplete;
        }

        public string Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime ValidUntil { get; set; }

        public bool IsComplete { get; set; }

        public ResetPasswordRequest AsResetPasswordRequest()
        {
            return new ResetPasswordRequest
            {
                Id = Id,
                Created = Created,
                IsComplete = IsComplete,
                ValidUntil = ValidUntil
            };
        }
    }
}
