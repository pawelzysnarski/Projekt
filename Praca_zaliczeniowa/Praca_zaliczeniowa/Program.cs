﻿using System;
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
                { Role.Medic, new List<string>{"Treat player","Check injury status"} },
                { Role.Boss, new List<string>{"Make important decisions","Fire staff","Hire staff"} },
                { Role.Scout, new List<string>{"Scout player","Report on players"} },
                { Role.Player, new List<string>{"Play in matches","Train"} }
                
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
            public void SetLineup()
            {
                Console.WriteLine("Enter the formation (e.g., 4-4-2, 4-3-3, etc.):");
                string formation = Console.ReadLine();  
             
                string[] positions = formation.Split('-');
                int numDefenders = int.Parse(positions[0]);
                int numMidfielders = int.Parse(positions[1]);
                int numForwards = int.Parse(positions[2]);

            }
            Console.WriteLine("Assign players to positions.");

                List<Player> availablePlayers = Members.OfType<Player>().ToList();

                
                for (int i = 0; i < numDefenders; i++)
                {
                    Console.WriteLine($"Choose a defender (LeftBack, CentreBack, RightBack):");
                    string defenderPosition = Console.ReadLine();
                    Position defenderPos = Enum.Parse<Position>(defenderPosition, true);
                    Console.WriteLine($"Choose a player for {defenderPosition}:");
                    Player selectedDefender = availablePlayers[i];
                    Lineup.Add(defenderPos, selectedDefender);
                    availablePlayers.Remove(selectedDefender);
                }

                
                for (int i = 0; i < numMidfielders; i++)
                {
                    Console.WriteLine($"Choose a midfielder (LeftMidfielder, Midfielder, RightMidfielder):");
                    string midfielderPosition = Console.ReadLine();
                    Position midfielderPos = Enum.Parse<Position>(midfielderPosition, true);
                    Console.WriteLine($"Choose a player for {midfielderPosition}:");
                    Player selectedMidfielder = availablePlayers[i];
                    Lineup.Add(midfielderPos, selectedMidfielder);
                    availablePlayers.Remove(selectedMidfielder);
                }

                
                for (int i = 0; i < numForwards; i++)
                {
                    Console.WriteLine($"Choose a forward (LeftWinger, RightWinger, Striker):");
                    string forwardPosition = Console.ReadLine();
                    Position forwardPos = Enum.Parse<Position>(forwardPosition, true);
                    Console.WriteLine($"Choose a player for {forwardPosition}:");
                    Player selectedForward = availablePlayers[i];
                    Lineup.Add(forwardPos, selectedForward);
                    availablePlayers.Remove(selectedForward);
                }

                
                Console.WriteLine("Choose a goalkeeper:");
                string goalkeeperPosition = "Goalkeeper";
                Goalkeeper selectedGoalkeeper = (Goalkeeper)availablePlayers.FirstOrDefault(p => p.Position == Position.Goalkeeper);
                Lineup.Add(Position.Goalkeeper, selectedGoalkeeper);

                Console.WriteLine("Lineup set successfully!");
            }
            public void AddPlayer(Player player)
            {
                Members.Add(player);
                Console.WriteLine($"{player.FirstName} {player.LastName} added to the club.");
            }
            public void RemovePlayer(Player player)
            {
                Members.Remove(player);
                Console.WriteLine($"{player.FirstName} {player.LastName} removed from the club.");
            }
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
