namespace Plot.Sample.Data.Nodes
{
    public class DriverNode
    {
        public DriverNode()
        {

        }

        public DriverNode(Driver driver)
        {
            Id = driver.Id;
        }

        public string Id { get; set; }

        public Driver AsDriver()
        {
            return new Driver
            {
                Id = Id
            };
        }
    }
}
