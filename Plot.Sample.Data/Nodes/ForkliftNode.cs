namespace Plot.Sample.Data.Nodes
{
    public class ForkliftNode
    {
        public ForkliftNode()
        {

        }

        public ForkliftNode(Forklift vehicle)
        {
            Id = vehicle.Id;
        }

        public string Id { get; set; }

        public Forklift Asforklift()
        {
            return new Forklift
            {
                Id = Id,
            };
        }
    }
}
