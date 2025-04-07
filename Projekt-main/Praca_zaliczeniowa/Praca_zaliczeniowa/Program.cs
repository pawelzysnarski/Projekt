using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
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
            OffensiveMidfielder,
            LeftMidfielder,
            RightMidfielder,
            LeftWinger,
            RightWinger,
            Striker
        }
        public class Club
        {
            public List<Message> Messages { get; set; }
            public string Name { get; set; }
            public List<ClubMember> Members { get; set; }
            public Dictionary<Position, ClubMember> Lineup { get; set; }
            public readonly Dictionary<Role, List<string>> Permissions = new Dictionary<Role, List<string>>
            {
                { Role.Coach,new List<string>{"Make Lineup","Blame player","praise player","Ask for new player","Chat" } },
                { Role.Medic, new List<string>{"Treat player","Check injury status"} },
                { Role.Boss, new List<string>{"Make important decisions","Sack staff","Hire staff"} },
                { Role.Scout, new List<string>{"Scout player","Report on players"} },
                { Role.Player, new List<string>{"Play in matches","Train"} }

            };
            public bool HasThatPermission(ClubMember clubmember, string permission)
            {
                if (Permissions.ContainsKey(clubmember.Role) && Permissions[clubmember.Role].Contains(permission))
                {
                    return true;
                }
                return false;
            }

            public Club(string name, List<ClubMember> members, List<Message> messages)
            {
                Name = name;
                Members = members;
                Messages = messages;
            }
            public void SetLineup()
            {
                Console.WriteLine("Enter the formation (e.g., 4-4-2, 4-3-3, etc.):");
                string formation = Console.ReadLine();

                string[] positions = formation.Split('-');
                int numDefenders = int.Parse(positions[0]);
                int numMidfielders = int.Parse(positions[1]);
                int numForwards = int.Parse(positions[2]);

                Console.WriteLine("Assign players to positions.");

                List<Player> availablePlayers = Members.OfType<Player>().ToList();
                Lineup = new Dictionary<Position, ClubMember>();

                // Helper method to display players with position highlighting
                void DisplayPlayersWithHighlight(Position targetPosition)
                {
                    Console.WriteLine($"Available players for {targetPosition}:");
                    Console.WriteLine("(Players matching position are shown in green)");

                    int index = 1;
                    foreach (var player in availablePlayers.OrderByDescending(p => p.Position == targetPosition))
                    {
                        if (player.Position == targetPosition)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{index++}. {player.FirstName} {player.LastName} ({player.Position}) - Overall: {player.OverallStats()}");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine($"{index++}. {player.FirstName} {player.LastName} ({player.Position}) - Overall: {player.OverallStats()}");
                        }
                    }
                }

                // Assign goalkeeper (only one)
                Console.WriteLine("\nAvailable goalkeepers:");
                var goalkeepers = availablePlayers.Where(p => p.Position == Position.Goalkeeper).ToList();
                for (int j = 0; j < goalkeepers.Count; j++)
                {
                    Console.WriteLine($"{j + 1}. {goalkeepers[j].FirstName} {goalkeepers[j].LastName}");
                }

                Console.WriteLine("Select goalkeeper number:");
                int gkNumber = int.Parse(Console.ReadLine()) - 1;
                Player selectedGoalkeeper = goalkeepers[gkNumber];
                Lineup.Add(Position.Goalkeeper, selectedGoalkeeper);
                availablePlayers.Remove(selectedGoalkeeper);

                // Assign defenders with multiple players per position
                Console.WriteLine("\n=== DEFENDERS ===");
                for (int i = 0; i < numDefenders; i++)
                {
                    Console.WriteLine($"\nChoosing defender {i + 1} of {numDefenders}:");
                    Console.WriteLine("Available positions: LeftBack, CentreBack, RightBack");
                    Console.Write("Enter position: ");
                    Position defenderPos = Enum.Parse<Position>(Console.ReadLine(), true);

                    DisplayPlayersWithHighlight(defenderPos);

                    Console.WriteLine("Select player number:");
                    int playerNumber = int.Parse(Console.ReadLine()) - 1;

                    // Get the selected player
                    var orderedPlayers = availablePlayers.OrderByDescending(p => p.Position == defenderPos).ToList();
                    Player selectedDefender = orderedPlayers[playerNumber];

                    // Create unique key if position already exists
                    Position lineupKey = defenderPos;
                    if (Lineup.ContainsKey(defenderPos))
                    {
                        lineupKey = (Position)((int)defenderPos + i + 1); // Create unique key
                    }

                    Lineup.Add(lineupKey, selectedDefender);
                    availablePlayers.Remove(selectedDefender);
                }

                // Assign midfielders with multiple players per position
                Console.WriteLine("\n=== MIDFIELDERS ===");
                for (int i = 0; i < numMidfielders; i++)
                {
                    Console.WriteLine($"\nChoosing midfielder {i + 1} of {numMidfielders}:");
                    Console.WriteLine("Available positions: LeftMidfielder, Midfielder, RightMidfielder, DefensiveMidfielder, OffensiveMidfielder");
                    Console.Write("Enter position: ");
                    Position midfielderPos = Enum.Parse<Position>(Console.ReadLine(), true);

                    DisplayPlayersWithHighlight(midfielderPos);

                    Console.WriteLine("Select player number:");
                    int playerNumber = int.Parse(Console.ReadLine()) - 1;

                    var orderedPlayers = availablePlayers.OrderByDescending(p => p.Position == midfielderPos).ToList();
                    Player selectedMidfielder = orderedPlayers[playerNumber];

                    Position lineupKey = midfielderPos;
                    if (Lineup.ContainsKey(midfielderPos))
                    {
                        lineupKey = (Position)((int)midfielderPos + i + 1);
                    }

                    Lineup.Add(lineupKey, selectedMidfielder);
                    availablePlayers.Remove(selectedMidfielder);
                }

                // Assign forwards with multiple players per position
                Console.WriteLine("\n=== FORWARDS ===");
                for (int i = 0; i < numForwards; i++)
                {
                    Console.WriteLine($"\nChoosing forward {i + 1} of {numForwards}:");
                    Console.WriteLine("Available positions: LeftWinger, RightWinger, Striker");
                    Console.Write("Enter position: ");
                    Position forwardPos = Enum.Parse<Position>(Console.ReadLine(), true);

                    DisplayPlayersWithHighlight(forwardPos);

                    Console.WriteLine("Select player number:");
                    int playerNumber = int.Parse(Console.ReadLine()) - 1;

                    var orderedPlayers = availablePlayers.OrderByDescending(p => p.Position == forwardPos).ToList();
                    Player selectedForward = orderedPlayers[playerNumber];

                    Position lineupKey = forwardPos;
                    if (Lineup.ContainsKey(forwardPos))
                    {
                        lineupKey = (Position)((int)forwardPos + i + 1);
                    }

                    Lineup.Add(lineupKey, selectedForward);
                    availablePlayers.Remove(selectedForward);
                }

                // Display final lineup
                Console.WriteLine("\nLineup set successfully!");
                Console.WriteLine("\n=== SELECTED LINEUP ===");

                // Group by base position (without the +i modification)
                var groupedLineup = Lineup.GroupBy(kvp =>
                    Enum.IsDefined(typeof(Position), kvp.Key) ? kvp.Key : (Position)((int)kvp.Key / 10 * 10));

                foreach (var group in groupedLineup)
                {
                    if (group.Count() > 1)
                    {
                        Console.WriteLine($"{group.Key}s:");
                        foreach (var player in group)
                        {
                            Console.WriteLine($"- {player.Value.FirstName} {player.Value.LastName}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{group.Key}: {group.First().Value.FirstName} {group.First().Value.LastName}");
                    }
                }
            }
            public void HireClubMember(ClubMember clubMember)
            {
                Members.Add(clubMember);
                Console.WriteLine($"{clubMember.FirstName} {clubMember.LastName} hired to the club.");
            }
            public void SackClubMember(ClubMember clubMember)
            {
                Members.Remove(clubMember);
                Console.WriteLine($"{clubMember.FirstName} {clubMember.LastName} sacked from the club.");
            }
            public void MakeTeamTraining()
            {
                List<Player> players = Members.OfType<Player>().ToList();
                foreach (Player player in players)
                {
                    if (!player.IsInjured)
                    {
                        if (player is Goalkeeper goalkeeper)
                        {
                            goalkeeper.Train();
                        }
                        else
                        {
                            player.Train();
                        }
                    }
                }
                Console.WriteLine("Training ended");
            }

            public void DisplayMembersList()
            {
                Console.WriteLine("Choose which list to display:");
                Console.WriteLine("1. Players");
                Console.WriteLine("2. Staff");
                Console.WriteLine("Enter your choice (1 or 2):");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayPlayersList();
                        break;
                    case "2":
                        DisplayStaffList();
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }

            private void DisplayPlayersList()
            {
                var players = Members.OfType<Player>().OrderBy(p => p.Position).ThenBy(p => p.LastName).ToList();

                Console.WriteLine("\nList of Players:");
                Console.WriteLine("------------------------------------------------------------------");
                Console.WriteLine("| #  | Name                      | Position            | Overall |");
                Console.WriteLine("------------------------------------------------------------------");

                foreach (var player in players)
                {
                    if (player is Goalkeeper goalkeeper)
                    {
                        Console.WriteLine($"| {goalkeeper.Number,2} | {goalkeeper.FirstName + " " + goalkeeper.LastName,-25} | {goalkeeper.Position,-19} | {goalkeeper.OverallStats(),7} |");
                    }
                    else
                    {
                        Console.WriteLine($"| {player.Number,2} | {player.FirstName + " " + player.LastName,-25} | {player.Position,-19} | {player.OverallStats(),7} |");
                    }
                }

                Console.WriteLine("------------------------------------------------------------------");

            }

            private void DisplayStaffList()
            {
                var staffMembers = Members.OfType<Staff>().OrderBy(s => s.Role).ThenBy(s => s.LastName).ToList();

                Console.WriteLine("\nList of Staff:");
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine("| Role      | Name                 | Age | Experience (yrs) |");
                Console.WriteLine("-------------------------------------------------------------");

                foreach (var staff in staffMembers)
                {
                    Console.WriteLine($"| {staff.Role,-9} | {staff.FirstName + " " + staff.LastName,-20} | {staff.Age,3} | {staff.YearsOfExperience,16} |");
                }

                Console.WriteLine("-------------------------------------------------------------");
            }
            public class AppDbContext : DbContext
            {
                public DbSet<Player> players { get; set; }
                public DbSet<Staff> staff { get; set; }
                public DbSet<Goalkeeper> goalkeepers { get; set; }
                public DbSet<Message> messages { get; set; }
                protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                {
                    optionsBuilder.UseMySql("Server=localhost;Port=3306;Database=club;User=root;Password='';",
                        new MySqlServerVersion(new Version(11, 6, 0)));
                }
                protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
                    modelBuilder.Entity<Player>().HasKey(p => p.Number);
                    modelBuilder.Entity<Staff>().HasKey(s => s.ID);
                    modelBuilder.Entity<Message>().HasKey(m => m.ID);
                    modelBuilder.Entity<Player>().ToTable("players");
                    modelBuilder.Entity<Staff>().ToTable("staff");
                    modelBuilder.Entity<Goalkeeper>().ToTable("goalkeepers");
                    modelBuilder.Entity<Message>().ToTable("messages");
                    modelBuilder.Entity<Player>().Ignore(p => p.Role);
                    modelBuilder.Entity<Player>()
           .Property(p => p.FirstName).HasColumnOrder(1);

                    modelBuilder.Entity<Player>()
                        .Property(p => p.LastName).HasColumnOrder(2);

                    modelBuilder.Entity<Player>()
                        .Property(p => p.Position).HasColumnOrder(3);

                    modelBuilder.Entity<Player>()
                        .Property(p => p.Pace).HasColumnOrder(4);

                    modelBuilder.Entity<Player>()
                        .Property(p => p.Shooting).HasColumnOrder(5);

                    modelBuilder.Entity<Player>()
                        .Property(p => p.Passing).HasColumnOrder(6);

                    modelBuilder.Entity<Player>()
                        .Property(p => p.Dribling).HasColumnOrder(7);

                    modelBuilder.Entity<Player>()
                        .Property(p => p.Defense).HasColumnOrder(8);

                    modelBuilder.Entity<Player>()
                        .Property(p => p.Physical).HasColumnOrder(9);

                    modelBuilder.Entity<Player>()
                        .Property(p => p.Number).HasColumnOrder(10);

                    modelBuilder.Entity<Player>()
                        .Property(p => p.IsInjured).HasColumnOrder(11);

                    modelBuilder.Entity<Player>()
                        .Property(p => p.Age).HasColumnOrder(12);
                    modelBuilder.Entity<Player>()
                        .Property(p => p.Position)
                        .HasConversion(
                            v => v.ToString(),
                            v => (Position)Enum.Parse(typeof(Position), v)
                        );
                    modelBuilder.Entity<Staff>()
            .Property(c => c.Role)
            .HasConversion(
                v => v.ToString(),
                v => (Role)Enum.Parse(typeof(Role), v)
            );
                }
            }
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
            public class Staff : ClubMember
            {
                public int ID { get; set; }
                public int YearsOfExperience { get; set; }
                public DateTime? DateOfEndTask { get; set; }
                public Staff(Role role, string firstName, string lastName, int age, int yearsOfExperience, DateTime? dateOfEndTask) : base(firstName, lastName, age, role)
                {
                    YearsOfExperience = yearsOfExperience;
                    DateOfEndTask = dateOfEndTask;
                }
                public void StartTask(DateTime endTime)
                {
                    if (endTime != null)
                    {
                        DateOfEndTask = endTime;
                        Console.WriteLine($"{LastName} starts task. It will exired at {endTime.ToLongDateString()}");
                    }
                }
            }

            public class Player : ClubMember
            {
                public Position Position { get; set; }
                public int Pace { get; set; }
                public int Shooting { get; set; }
                public int Passing { get; set; }
                public int Dribling { get; set; }
                public int Defense { get; set; }
                public int Physical { get; set; }
                public bool IsInjured { get; set; }
                public int Number { get; set; }
                public Player(int number, Position position, int pace, int shooting, int passing, int dribling, int defense, int physical, bool isInjured, string firstName, string lastName, int age) : base(firstName, lastName, age, Role.Player)
                {
                    Position = position;
                    Pace = pace;
                    Shooting = shooting;
                    Passing = passing;
                    Dribling = dribling;
                    Defense = defense;
                    Physical = physical;
                    IsInjured = isInjured;
                    Number = number;
                }
                public virtual int OverallStats()
                {
                    return (int)Math.Round((double)(Pace + Shooting + Passing + Dribling + Defense + Physical) / 6);
                }
                public virtual void Train()
                {
                    Random random = new Random();
                    int injurystatus = random.Next(200);
                    if (injurystatus == 0)
                    {
                        IsInjured = true;
                        Console.WriteLine($"Player {FirstName} {LastName} got injury during training");
                    }
                    else if (injurystatus >= 1 && injurystatus <= 5)
                    {
                        Console.WriteLine($"Player {FirstName} {LastName} has improved");
                        int bonusstat = random.Next(6);
                        switch (bonusstat)
                        {
                            case 0:
                                Pace += 1;
                                break;
                            case 1:
                                Shooting += 1;
                                break;
                            case 2:
                                Passing += 1;
                                break;
                            case 3:
                                Dribling += 1;
                                break;
                            case 4:
                                Defense += 1;
                                break;
                            case 5:
                                Physical += 1;
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Player {FirstName} {LastName} trained");
                    }
                }
            }
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
                    else if (injurystatus == 1 || injurystatus == 2)
                    {
                        GoalkeeperStats += 1;
                        Console.WriteLine($"Player {FirstName} {LastName} has improved");
                    }
                    else
                    {
                        Console.WriteLine($"Player {FirstName} {LastName} trained");
                    }
                }


            }
            public class Message
            {
                public int ID { get; set; }
                public int Member_ID { get; set; }
                public string Content { get; set; }
                public bool IsReaded { get; set; }
                public Message() { }
                public void ReadMessage()
                {
                    Console.WriteLine(Content);
                    if (!IsReaded)
                    {
                        IsReaded = true;
                    }
                }
                public Message(int id, int member_id, string content, bool isReaded)
                {
                    ID = id;
                    Member_ID = member_id;
                    Content = content;
                    IsReaded = isReaded;
                }
            }
            static void Main(string[] args)
            {
                List<ClubMember> lista = new List<ClubMember>();
                using (var context = new AppDbContext())
                {
                    var players = context.players.Where(p => p.Position != Position.Goalkeeper).ToList();
                    foreach (var player in players)
                    {
                        lista.Add(new Player(
                        player.Number,
                        player.Position,
                        player.Pace,
                        player.Shooting,
                        player.Passing,
                        player.Dribling,
                        player.Defense,
                        player.Physical,
                        player.IsInjured,
                        player.FirstName,
                        player.LastName,
                        player.Age));

                    }
                }
                using (var context = new AppDbContext())
                {
                    var stafflist = context.staff.ToList();
                    foreach (var staff in stafflist)
                    {
                        lista.Add(new Staff(
                        staff.Role,
                        staff.FirstName,
                        staff.LastName,
                        staff.Age,
                        staff.YearsOfExperience,
                        staff.DateOfEndTask));
                    }
                }
                using (var context = new AppDbContext())
                {
                    var goalkeepers = context.goalkeepers.ToList();
                    foreach (var player1 in goalkeepers)
                    {
                        lista.Add(new Goalkeeper(
                        player1.Number,
                        player1.GoalkeeperStats,
                        player1.Position,
                        player1.Pace,
                        player1.Shooting,
                        player1.Passing,
                        player1.Dribling,
                        player1.Defense,
                        player1.Physical,
                        player1.IsInjured,
                        player1.FirstName,
                        player1.LastName,
                        player1.Age));

                    }
                }
                List<Message> messageslist = new List<Message>();

                using (var context = new AppDbContext())
                {
                    var messages = context.messages.ToList();
                    foreach (var message in messages)
                    {
                        messageslist.Add(new Message(
                                message.ID,
                                message.Member_ID,
                                message.Content,
                                (bool)message.IsReaded
                            ));
                    }
                }
                Club club = new Club("Dębiec FC", lista, messageslist);
                club.MakeTeamTraining();
                club.DisplayMembersList();
                club.SetLineup();
            }
        }
    }
}
