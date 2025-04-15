using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Clubs.Classes.clubMember;
using static Clubs.Classes.player;
using static Clubs.Program;
using Task = Clubs.Program.Task;
using static Clubs.IStaff;
namespace Clubs.Classes
{
    internal class staff 
    {
        public class Staff : ClubMember , IStaff
        {
            public int ID { get; set; }
            public int YearsOfExperience { get; set; }
            public DateTime? DateOfEndTask { get; set; }
            public Staff(Role role, string firstName, string lastName, int age, int yearsOfExperience, DateTime? dateOfEndTask, int iD) : base(firstName, lastName, age, role)
            {
                YearsOfExperience = yearsOfExperience;
                DateOfEndTask = dateOfEndTask;
                ID = iD;
            }
            public Staff(Role role, string firstName, string lastName, int age, int yearsOfExperience, DateTime? dateOfEndTask) : base(firstName, lastName, age, role)
            {
                YearsOfExperience = yearsOfExperience;
                DateOfEndTask = dateOfEndTask;
            }
            public void StartTask(Player player)
            {
                using (var context = new AppDbContext())
                {
                    var now = DateTime.Now;
                    now.AddHours(36);
                    DateOfEndTask = now;

                    Console.WriteLine($"{LastName} starts task. It will exired at {now.ToLongDateString()}");
                    string type;
                    if (Role == Role.Medic)
                    {
                        type = "Healing";
                    }
                    else
                    {
                        type = "Other";
                    }
                    context.tasks.Add(new Task(ID, now, type, player.Number));
                    context.SaveChanges();

                }
            }
        }
    }
}
