namespace Plot.Sample.Data.Nodes
{
    public class CrewNode
    {
        public CrewNode()
        {
            
        }

        public CrewNode(Crew crew)
        {
            Id = crew.Id;
        }

        public string Id { get; set; }

        public Crew AsCrew()
        {
            return new Crew
            {
                Id = Id
            };
        }
    }
}
