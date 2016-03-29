namespace Plot.Sample.Data.Nodes
{
    public class TrailerNode
    {
        public TrailerNode()
        {

        }

        public TrailerNode(Trailer vehicle)
        {
            Id = vehicle.Id;
        }

        public string Id { get; set; }

        public Trailer AsTrailer()
        {
            return new Trailer
            {
                Id = Id,
            };
        }
    }
}
