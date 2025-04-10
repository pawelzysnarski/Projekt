using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clubs;
using static Clubs.Program;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static Clubs.AppDbContext;
using static Clubs.Classes.player;

namespace Clubs.Classes
{
    internal class goalkeeper
    {
        public class Goalkeeper : Player
        {
            public int GoalkeeperStats { get; set; }
            public Goalkeeper(int number, int goalkeeperStats, Position position, int pace, int shooting, int passing, int dribling, int defense, int physical, bool isInjured, string firstName, string lastName, int age) : base(number, position, pace, shooting, passing, dribling, defense, physical, isInjured, firstName, lastName, age)
            {
                GoalkeeperStats = goalkeeperStats;
            }
            public override int OverallStats()
            {
                return GoalkeeperStats;
            }
            public override void Train()
            {
                Random random = new Random();
                int injurystatus = random.Next(100);
                if (injurystatus == 0)
                {
                    IsInjured = true;
                    Console.WriteLine($"Player {FirstName} {LastName} got injury during training");
                }
                else if (injurystatus == 1)
                {
                    GoalkeeperStats += 1;
                    using (var context = new AppDbContext())
                    {
                        var player = context.goalkeepers.Find(Number);
                        if (player != null)
                        {
                            player.GoalkeeperStats += 1;
                            context.SaveChanges();
                        }
                    }
                    Console.WriteLine($"Player {FirstName} {LastName} has improved");
                }
                else
                {
                    Console.WriteLine($"Player {FirstName} {LastName} trained");
                }
            }


        }
    }
}
