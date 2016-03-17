namespace Plot.Sample.Data.Nodes
{
    public class MembershipNode
    {
        public MembershipNode()
        {
            
        }

        public MembershipNode(Membership membership)
        {
            Id = membership.Id;
            IsActive = membership.IsActive;
        }

        public string Id { get; set; }

        public bool IsActive { get; set; }

        public Membership AsMembership()
        {
            return new Membership
            {
                Id = Id,
                IsActive = IsActive
            };
        }
    }
}
