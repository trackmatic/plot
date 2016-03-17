namespace Plot.Sample.Data.Nodes
{
    public class PersonNode
    {
        public PersonNode()
        {
            
        }

        public PersonNode(Person person)
        {
            Id = person.Id;
            FirstName = person.Names.First;
            LastName = person.Names.Last;
            Email = person.Email;
            Mobile = person.Mobile;
        }

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }


        public Person AsPerson()
        {
            return new Person
            {
                Id = Id,
                Names = new Names
                {
                    First = FirstName,
                    Last = LastName
                },
                Mobile = Mobile,
                Email = Email
            };
        }
    }
}
