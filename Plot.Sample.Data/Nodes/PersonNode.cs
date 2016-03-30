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
            MobileNumber = person.Numbers?.Mobile;
            WorkNumber = person.Numbers?.Work;
            HomeNumber = person.Numbers?.Home;
            Position = person.Position;
            Department = person.Department;
            Gender = person.Gender;
            Title = person.Title;
            IdentityNumber = person.IdentityNumber;
        }

        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Title { get; set; }

        public string Position { get; set; }

        public string Department { get; set; }

        public string Gender { get; set; }

        public string HomeNumber { get; set; }

        public string WorkNumber { get; set; }

        public string MobileNumber { get; set; }

        public string IdentityNumber { get; set; }

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
                Numbers = new Numbers
                {
                    Home = HomeNumber,
                    Mobile = MobileNumber,
                    Work = WorkNumber
                },
                Email = Email,
                Title = Title,
                Gender = Gender,
                Position = Position,
                Department = Department,
                IdentityNumber = IdentityNumber
            };
        }
    }
}
