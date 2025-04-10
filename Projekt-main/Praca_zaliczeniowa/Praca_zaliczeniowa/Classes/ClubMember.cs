using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Clubs.Program;


namespace Clubs.Classes
{
    internal class clubMember
    {
        public class ClubMember
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
            public Role Role { get; set; }
            public ClubMember(string firstName, string lastName, int age, Role role)
            {
                FirstName = firstName;
                LastName = lastName;
                Age = age;
                Role = role;
            }
        }
    }
}
