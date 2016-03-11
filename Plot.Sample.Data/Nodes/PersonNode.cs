using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plot.Sample.Model;

namespace Plot.Sample.Data.Nodes
{
    public class PersonNode
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PrimaryMobileNo { get; set; }


        public Person AsPerson()
        {
            return new Person
            {
                Id = Id,
                Names = new Names
                {
                    First = FirstName,
                    Last = LastName
                }
            };
        }
    }
}
