using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Clubs
{
    internal class Program
    {
        public enum Role
        {
            Coach,
            Medic,
            Boss,
            Scout,
            Player
        }
        public enum Position
        {
            Goalkeeper,
            LeftBack,
            RightBack,
            CentreBack,
            DefensiveMidfielder,
            Midfielder,
            OfensiveMidfielder,
            LeftMidfielder,
            RightMidfielder,
            LeftWinger,
            RightWinger,
            Striker
        }
        public class Club
        {
            public string Name { get; set; }
            public List<ClubMember> Members { get; set; }
            public Dictionary<Position,ClubMember> Lineup { get; set; }
            public readonly Dictionary<Role, List<string>> Permissions = new Dictionary<Role, List<string>>
            {
                { Role.Coach,new List<string>{"Make Lineup","Blame player","praise player","Ask for new player","Chat" } },
                //Wymyśl kilka uprawnień dla pozostałych ról
            };
            public bool HasThatPermission(ClubMember clubmember,string permission)
            {
                if (Permissions.ContainsKey(clubmember.Role) && Permissions[clubmember.Role].Contains(permission))
                {
                    return true;
                }
                return false;
            }
            public Club(string name,List<ClubMember> members) 
            {
                Name = name;
                Members = members;
            }
            /*
                Dodaj tutaj jakąś metodę do ustawienia składu
                Najpierw pyta o formację a potem po kolei o zawodników
            */
            //Zrób też dodawanie i usuwanie zawodników
        }
        public class ClubMember
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
            public Role Role { get; set; }
            public ClubMember(string firstName,string lastName,int age,Role role) 
            {
                FirstName = firstName;
                LastName = lastName;
                Age = age;
                Role = role;
            }
        }
        public class Staff : ClubMember
        {
            public Staff(Role role, string firstName, string lastName, int age) : base(firstName, lastName, age,role) {
                
            }
        }
        public class Player : ClubMember
        {
            public Role Role { get; set; } = Role.Player;
            public Position Position { get; set; }
            public int Pace { get; set; }
            public int Shooting { get; set; }
            public int Passing { get; set; }
            public int Dribling { get; set; }
            public int Defense { get; set; }
            public int Physical { get; set; }
            public bool IsInjured { get; set; }
            public Player(Position position,int pace,int shooting,int passing,int dribling,int defense,int physical,bool isInjured, string firstName, string lastName, int age) : base(firstName, lastName, age,Role.Player)
            {
                Position = position;
                Pace = pace;
                Shooting = shooting;
                Passing = passing;
                Dribling = dribling;
                Defense = defense;
                Physical = physical;
                IsInjured = isInjured;
            }
            public virtual int OverallStats()
            {
                return (int)Math.Round((double)(Pace + Shooting + Passing + Dribling + Defense + Physical) / 6);
            }
        }
        public class Goalkeeper : Player
        {
            public int GoalkeeperStats { get; set; }
            public Goalkeeper(int goalkeeperStats,Position position, int pace, int shooting, int passing, int dribling, int defense, int physical, bool isInjured, string firstName, string lastName, int age) : base(position,pace,shooting,passing,dribling,defense,physical,isInjured,firstName,lastName,age)
            {
                GoalkeeperStats = goalkeeperStats;
            }
            public override int OverallStats()
            {
                return GoalkeeperStats;
            }
        }
        static void Main(string[] args)
        {

        }
    }
}