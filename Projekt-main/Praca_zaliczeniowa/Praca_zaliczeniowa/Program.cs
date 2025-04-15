using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using static Clubs.AppDbContext;

using static Clubs.Classes.clubMember;
using static Clubs.Classes.staff;
using static Clubs.login;
using static Clubs.Classes.goalkeeper;
using static Clubs.Classes.player;
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
            CentreBack,
            RightBack,
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
            public List<Task> Tasks { get; set; }
            public string Name { get; set; }
            public List<ClubMember> Members { get; set; }
            public Dictionary<Player, Position> Lineup { get; set; }
            public Action<string, ClubMember> MessageSender { get; set; }
            public event Action<Staff> OnTaskEnd;
            public readonly Dictionary<Role, List<string>> Permissions = new Dictionary<Role, List<string>>
            {
                { Role.Coach,new List<string>{"Make lineup","See lineup","Make team training session","Chat" } },
                { Role.Medic, new List<string>{"Heal player","Chat"} },
                { Role.Boss, new List<string>{"See lineup","Sack staff","Hire staff","Chat"} },
                { Role.Player, new List<string>{"Chat","See lineup","Train"} }

            };
            public bool HasThatPermission(ClubMember clubmember, string permission)
            {
                if (Permissions.ContainsKey(clubmember.Role) && Permissions[clubmember.Role].Contains(permission))
                {
                    return true;
                }
                return false;
            }

            public Club(string name, List<ClubMember> members, List<Message> messages, List<Task> tasks)
            {
                Name = name;
                Members = members;
                Messages = messages;
                Tasks = tasks;
                Lineup = new Dictionary<Player, Position>();
            }
            public void MyTasks(Staff staff)
            {
                foreach (var task in Tasks.Where(t => t.Member_ID == staff.ID))
                {
                    task.TaskInfo();
                }
            }
            public void EndTask(Staff staff)
            {
                foreach (var task in Tasks.Where(t => t.Member_ID == staff.ID))
                {
                    if (task.Task_End_Date < DateTime.Now)
                    {
                        Console.WriteLine("Task ended");
                        Tasks.Remove(task);
                        using (var context = new AppDbContext())
                        {
                            var taskToRemove = context.tasks.Find(task.Member_ID);
                            if (taskToRemove != null)
                            {
                                context.tasks.Remove(taskToRemove);
                                context.SaveChanges();
                                OnTaskEnd?.Invoke(staff);
                            }
                        }
                    }
                }
            }
            public void SeeLineup()
            {
                Console.WriteLine("Lineup:");
                int i = 1;
                foreach (var player in Lineup.OrderBy(p => p.Value))
                {
                    if (player.Key.IsInjured)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{i}. {player.Value}: {player.Key.FirstName} {player.Key.LastName} (Injured)");
                        Console.ResetColor();
                    }
                    else if (player.Value != player.Key.Position)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{i}. {player.Value}: {player.Key.FirstName} {player.Key.LastName}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"{i}. {player.Value}: {player.Key.FirstName} {player.Key.LastName}");
                    }
                    i++;
                }
            }
            public void MyMessages(ClubMember clubMember)
            {
                Console.WriteLine("Your messages:");
                foreach (var message in Messages)
                {
                    if (clubMember is Staff staff)
                    {
                        if (message.Member_ID == staff.ID)
                        {
                            message.ReadMessage();
                        }
                    }
                    else if (clubMember is Player player)
                    {
                        if (message.Member_ID == player.Number)
                        {
                            message.ReadMessage();
                        }

                    }
                }
            }
            public void SendMessage(ClubMember clubMember)
            {
                Console.WriteLine("Type a content of message: ");
                string content = Console.ReadLine();
                while (string.IsNullOrEmpty(content))
                {
                    Console.WriteLine("Content message cannot be empty. Type a content of message: ");
                    content = Console.ReadLine();
                }
                Console.Clear();
                bool checker = false;
                while (!checker)
                {
                    Console.WriteLine("Choose a type of receiver: ");
                    Console.WriteLine("1. Public message (to all club members)");
                    Console.WriteLine("2. Group message (to specific group");
                    Console.WriteLine("3. Private message (to specific member)");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            MessageSender = SendToAll;
                            MessageSender(content, clubMember);
                            checker = true;
                            break;
                        case "2":
                            MessageSender = SendToSpecificGroup;
                            MessageSender(content, clubMember);
                            checker = true;
                            break;
                        case "3":
                            MessageSender = SendToSpecificPerson;
                            MessageSender(content, clubMember);
                            checker = true;
                            break;
                    }
                }

                Console.WriteLine("Message was sent");
            }
            public void SendToAll(string content, ClubMember sender)
            {
                foreach (var clubMember in Members)
                {
                    if (sender == clubMember)
                    {
                        continue;
                    }
                    else
                    {
                        if (clubMember is Staff staff)
                        {
                            Message message = new Message
                            {
                                Sender_Name = $"{sender.FirstName} {sender.LastName}",
                                Member_ID = staff.ID,
                                Content = content,
                                IsReaded = false
                            };
                            using (var context = new AppDbContext())
                            {
                                context.messages.Add(message);
                                context.SaveChanges();
                            }
                        }
                        else if (clubMember is Player player)
                        {
                            Message message = new Message
                            {
                                Sender_Name = $"{sender.FirstName} {sender.LastName}",
                                Member_ID = player.Number,
                                Content = content,
                                IsReaded = false
                            };
                            using (var context = new AppDbContext())
                            {
                                context.messages.Add(message);
                                context.SaveChanges();
                            }


                        }
                    }
                }
            }
            public void SendToSpecificGroup(string content, ClubMember sender)
            {
                bool checker = false;
                while (!checker)
                {
                    Console.WriteLine("Choose a group to send the message to:");
                    Console.WriteLine("1. Players");
                    Console.WriteLine("2. Staff");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            foreach (var player in Members.OfType<Player>())
                            {
                                if (sender == player)
                                {
                                    continue;
                                }
                                else
                                {
                                    Message message = new Message
                                    {
                                        Sender_Name = $"{sender.FirstName} {sender.LastName}",
                                        Member_ID = player.Number,
                                        Content = content,
                                        IsReaded = false
                                    };
                                    using (var context = new AppDbContext())
                                    {
                                        context.messages.Add(message);
                                        context.SaveChanges();
                                    }
                                }
                            }
                            checker = true;
                            break;
                        case "2":
                            foreach (var staff in Members.OfType<Staff>())
                            {
                                if (sender == staff)
                                {
                                    continue;
                                }
                                else
                                {
                                    Message message = new Message
                                    {
                                        Sender_Name = $"{sender.FirstName} {sender.LastName}",
                                        Member_ID = staff.ID,
                                        Content = content,
                                        IsReaded = false
                                    };
                                    using (var context = new AppDbContext())
                                    {
                                        context.messages.Add(message);
                                        context.SaveChanges();
                                    }
                                }
                            }
                            checker = true;
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                    }
                }
            }
            public void SendToSpecificPerson(string content, ClubMember sender)
            {
                bool checker = false;
                int choice = 0;
                if (!checker)
                {
                    Console.WriteLine("Choose a member to send the message to:");
                    for (int i = 0; i < Members.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {Members[i].FirstName} {Members[i].LastName}");
                    }
                    while (int.TryParse(Console.ReadLine(), out choice) == false || choice <= 1 || choice > Members.Count)
                    {
                        Console.WriteLine("Invalid choice. Please select a valid member number.");
                    }
                    choice -= 1;
                    if (Members[choice] == sender)
                    {
                        Console.WriteLine("You cannot send message to yourself");
                        Console.ReadKey();
                        Console.Clear();
                    }
                    else
                    {
                        checker = true;
                    }
                }
                if (Members[choice] is Staff staff)
                {
                    Message message = new Message
                    {
                        Sender_Name = $"{sender.FirstName} {sender.LastName}",
                        Member_ID = staff.ID,
                        Content = content,
                        IsReaded = false
                    };
                    using (var context = new AppDbContext())
                    {
                        context.messages.Add(message);
                        context.SaveChanges();
                    }
                }
                else if (Members[choice] is Player player)
                {
                    Message message = new Message
                    {
                        Sender_Name = $"{sender.FirstName} {sender.LastName}",
                        Member_ID = player.Number,
                        Content = content,
                        IsReaded = false
                    };
                    using (var context = new AppDbContext())
                    {
                        context.messages.Add(message);
                        context.SaveChanges();
                    }


                }
            }
            public void SetLineup()
            {
                using(var context = new AppDbContext())
                {
                    foreach (var item in Lineup)
                    {
                        context.lineup.Remove(new Lineup(item.Key.Number, item.Value));
                        context.SaveChanges();
                    }
                }
                int numDefenders = 0;
                int numMidfielders = 0;
                int numForwards = 0;
                bool formationchecker = false;
                while (!formationchecker)
                {
                    try
                    {
                        Console.WriteLine("Enter the formation (e.g., 4-4-2, 4-3-3, etc.):");
                        string formation = Console.ReadLine();

                        string[] positions = formation.Split('-');
                        numDefenders = int.Parse(positions[0]);
                        numMidfielders = int.Parse(positions[1]);
                        numForwards = int.Parse(positions[2]);
                        if (numDefenders < 1 || numMidfielders < 1 || numForwards < 1)
                        {
                            Console.WriteLine("Invalid formation. Please enter a valid formation.");
                        }
                        else if (numDefenders > 5 || numMidfielders > 5 || numForwards > 5)
                        {
                            Console.WriteLine("Invalid formation. Please enter a valid formation.");
                        }
                        else if (numDefenders + numMidfielders + numForwards != 10)
                        {
                            Console.WriteLine("Wrong players number. Please enter a valid formation.");
                        }
                        else
                        {
                            formationchecker = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Invalid input. Please enter a valid formation.");
                    }
                    Console.ReadKey();
                    Console.Clear();
                }
                Console.WriteLine("Assign players to positions.");

                List<Player> availablePlayers = Members.OfType<Player>().ToList();
                Lineup = new Dictionary<Player, Position>();

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
                int gkNumber = 0;
                Console.WriteLine("Select goalkeeper number:");
                while (int.TryParse(Console.ReadLine(), out gkNumber) == false || gkNumber < 1 || gkNumber > goalkeepers.Count)
                {
                    Console.WriteLine("Invalid choice. Please select a valid goalkeeper number.");
                }
                gkNumber -= 1;
                Player selectedGoalkeeper = goalkeepers[gkNumber];
                Lineup.Add(selectedGoalkeeper, Position.Goalkeeper);
                availablePlayers.Remove(selectedGoalkeeper);
                Console.Clear();
                // Assign defenders with multiple players per position
                Console.WriteLine("\n=== DEFENDERS ===");
                for (int i = 0; i < numDefenders; i++)
                {
                    Console.WriteLine($"\nChoosing defender {i + 1} of {numDefenders}:");
                    Console.WriteLine("Available positions: LeftBack, CentreBack, RightBack");
                    Console.Write("Enter position: ");
                    Position defenderPos;
                    while (true)
                    {
                        string input = Console.ReadLine();
                        if (Enum.TryParse(input, true, out defenderPos) && Enum.IsDefined(typeof(Position), defenderPos))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid position. Please enter a valid position:");
                        }
                    }
                    DisplayPlayersWithHighlight(defenderPos);

                    Console.WriteLine("Select player number:");
                    int playerNumber;
                    while (int.TryParse(Console.ReadLine(), out playerNumber) == false || playerNumber < 1 || playerNumber > availablePlayers.Count)
                    {
                        Console.WriteLine("Invalid choice. Please select a valid player number.");
                    }
                    playerNumber -= 1;
                    // Get the selected player
                    var orderedPlayers = availablePlayers.OrderByDescending(p => p.Position == defenderPos).ToList();
                    Player selectedDefender = orderedPlayers[playerNumber];


                    Lineup.Add(selectedDefender, defenderPos);
                    availablePlayers.Remove(selectedDefender);
                    Console.Clear();
                }

                // Assign midfielders with multiple players per position
                Console.WriteLine("\n=== MIDFIELDERS ===");
                for (int i = 0; i < numMidfielders; i++)
                {
                    Console.WriteLine($"\nChoosing midfielder {i + 1} of {numMidfielders}:");
                    Console.WriteLine("Available positions: LeftMidfielder, Midfielder, RightMidfielder, DefensiveMidfielder, OffensiveMidfielder");
                    Console.Write("Enter position: ");
                    Position midfielderPos;
                    while (true)
                    {
                        string input = Console.ReadLine();
                        if (Enum.TryParse(input, true, out midfielderPos) && Enum.IsDefined(typeof(Position), midfielderPos))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid position. Please enter a valid position:");
                        }
                    }

                    DisplayPlayersWithHighlight(midfielderPos);

                    Console.WriteLine("Select player number:");
                    int playerNumber;
                    while (int.TryParse(Console.ReadLine(), out playerNumber) == false || playerNumber < 1 || playerNumber > availablePlayers.Count)
                    {
                        Console.WriteLine("Invalid choice. Please select a valid player number.");
                    }
                    playerNumber -= 1;
                    var orderedPlayers = availablePlayers.OrderByDescending(p => p.Position == midfielderPos).ToList();
                    Player selectedMidfielder = orderedPlayers[playerNumber];


                    Lineup.Add(selectedMidfielder, midfielderPos);
                    availablePlayers.Remove(selectedMidfielder);
                    Console.Clear();
                }

                // Assign forwards with multiple players per position
                Console.WriteLine("\n=== FORWARDS ===");
                for (int i = 0; i < numForwards; i++)
                {
                    Console.WriteLine($"\nChoosing forward {i + 1} of {numForwards}:");
                    Console.WriteLine("Available positions: LeftWinger, RightWinger, Striker");
                    Console.Write("Enter position: ");
                    Position forwardPos;
                    while (true)
                    {
                        string input = Console.ReadLine();
                        if (Enum.TryParse(input, true, out forwardPos) && Enum.IsDefined(typeof(Position), forwardPos))
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid position. Please enter a valid position:");
                        }
                    }

                    DisplayPlayersWithHighlight(forwardPos);

                    Console.WriteLine("Select player number:");
                    int playerNumber = 0;
                    while (int.TryParse(Console.ReadLine(), out playerNumber) == false || playerNumber < 1 || playerNumber > availablePlayers.Count)
                    {
                        Console.WriteLine("Invalid choice. Please select a valid player number.");
                    }
                    playerNumber -= 1;
                    var orderedPlayers = availablePlayers.OrderByDescending(p => p.Position == forwardPos).ToList();
                    Player selectedForward = orderedPlayers[playerNumber];


                    Lineup.Add(selectedForward, forwardPos);
                    availablePlayers.Remove(selectedForward);
                    Console.Clear();
                }

                // Display final lineup
                Console.WriteLine("\nLineup set successfully!");
                Console.WriteLine("\n=== SELECTED LINEUP ===");
                using(var content =new AppDbContext()){
                    foreach(var item in Lineup)
                    {
                        content.lineup.Add(new Lineup(item.Key.Number,item.Value));
                        content.SaveChanges();
                    }
                }
                // Group by base position (without the +i modification)
                SeeLineup();
            }
            public void HireClubMember(ClubMember clubMember)
            {
                Members.Add(clubMember);
                using (var context = new AppDbContext())
                {
                    if (clubMember is Player player)
                    {
                        if(player is Goalkeeper goalkeeper)
                        {
                            context.goalkeepers.Add(goalkeeper);
                            context.logDatas.Add(new LogData
                            {
                                Member_ID = goalkeeper.Number,
                                Login = goalkeeper.FirstName.ToLower()+"."+goalkeeper.LastName.ToLower()+"@"+goalkeeper.Role,
                                Password = HashPassword( goalkeeper.Role.ToString() + goalkeeper.Number.ToString())
                            });
                        }
                        else
                        {
                            context.players.Add(player);
                            context.logDatas.Add(new LogData
                            {
                                Member_ID = player.Number,
                                Login = player.FirstName.ToLower() + "." + player.LastName.ToLower() + "@" + player.Role,
                                Password = HashPassword(player.Role.ToString() + player.Number.ToString())
                            });
                        }
                    }
                    else if (clubMember is Staff staff)
                    {
                        context.staff.Add(staff);
                        context.logDatas.Add(new LogData
                        {
                            Member_ID = staff.ID,
                            Login = staff.FirstName.ToLower() + "." + staff.LastName.ToLower() + "@" + staff.Role,
                            Password = HashPassword(staff.Role.ToString() + staff.ID.ToString())
                        });
                    }
                    context.SaveChanges();
                }
                Console.WriteLine($"{clubMember.FirstName} {clubMember.LastName} hired to the club.");
            }
            public void SackClubMember(ClubMember clubMember)
            {
                if (!Members.Contains(clubMember))
                {
                    Console.WriteLine($"{clubMember.FirstName} {clubMember.LastName} is not a member of the club.");
                    return;
                }
                else
                {
                    if (clubMember is Player player)
                    {
                        using (var context = new AppDbContext())
                        {
                            var playerToRemove = context.players.Find(player.Number);
                            if (playerToRemove != null)
                            {
                                if (Lineup.ContainsKey(playerToRemove))
                                {
                                    Console.WriteLine("You cannot remove player who is in current lineup");
                                }
                                else
                                {
                                    context.players.Remove(playerToRemove);
                                    context.SaveChanges();
                                    Console.WriteLine($"{playerToRemove.FirstName} {playerToRemove.LastName} was removed from club");
                                }
                            }
                        }
                    }
                    else if (clubMember is Staff staff)
                    {
                        using (var context = new AppDbContext())
                        {
                            var staffToRemove = context.staff.Find(staff.ID);
                            if (staffToRemove != null)
                            {
                                context.staff.Remove(staffToRemove);
                                context.SaveChanges();
                                Console.WriteLine($"{staffToRemove.FirstName} {staffToRemove.LastName} was removed from club");
                            }
                        }
                    }
                }
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
        }
        static void OnTaskEnd(Staff staff)
        {
            Console.WriteLine($"Task ended for {staff.FirstName} {staff.LastName}");
        }
        
        
        public class Lineup
        {
            public int Number { get; set; }
            public Position Position { get; set; }
            public Lineup() { }
            public Lineup(int number, Position position)
            {
                Number = number;
                Position = position;
            }
        }
        

        
        public class Task
        {
            public int Member_ID { get; set; }
            public DateTime Task_End_Date { get; set; }
            public string TaskType { get; set; }
            public int Player_Number { get; set; }
            public Task() { }
            public Task(int memberid, DateTime date, string taskType, int playernumber)
            {
                Member_ID = memberid;
                Task_End_Date = date;
                TaskType = taskType;
                Player_Number = playernumber;
            }
            public void TaskInfo()
            {
                if (TaskType == "Healing")
                {
                    Console.WriteLine($"Healing player with number {Player_Number}. Ends in {Task_End_Date - DateTime.Now}");
                }
            }
        }
        
        public class Message
        {
            public int ID { get; set; }
            public string Sender_Name { get; set; }
            public int Member_ID { get; set; }
            public string Content { get; set; }
            public bool IsReaded { get; set; }
            public Message() { }
            public void ReadMessage()
            {
                Console.WriteLine($"{Sender_Name} : {Content}");
                if (!IsReaded)
                {
                    IsReaded = true;
                    using (var context = new AppDbContext())
                    {
                        var message = context.messages.Find(ID);
                        if (message != null)
                        {
                            message.IsReaded = true;
                            context.SaveChanges();
                        }
                    }
                }
            }
            public Message(int id, string sender_name, int member_id, string content, bool isReaded)
            {
                ID = id;
                Sender_Name = sender_name;
                Member_ID = member_id;
                Content = content;
                IsReaded = isReaded;
            }
        }
        

        static void Main(string[] args)
        {
            List<ClubMember> lista = new List<ClubMember>();
            List<LogData> loglist = new List<LogData>();
            using (var context = new AppDbContext())
            {
                var logins = context.logDatas.ToList();
                foreach (var login in logins)
                {
                    loglist.Add(new LogData
                    {
                        Member_ID = login.Member_ID,
                        Login = login.Login,
                        Password = login.Password
                    });
                }
            }
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
                    staff.DateOfEndTask,
                    staff.ID));
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
                            message.Sender_Name,
                            message.Member_ID,
                            message.Content,
                            (bool)message.IsReaded
                        ));
                }
            }
            List<Task> taskslist = new List<Task>();

            using (var context = new AppDbContext())
            {
                var tasks = context.tasks.ToList();
                foreach (var task in tasks)
                {
                    taskslist.Add(new Task(
                            task.Member_ID,
                            task.Task_End_Date,
                            task.TaskType,
                            task.Player_Number
                        ));
                }
            }
            Club club = new Club("Dębiec FC", lista, messageslist, taskslist);

            club.OnTaskEnd += OnTaskEnd;
            foreach (var task in taskslist)
            {
                if (task.Task_End_Date < DateTime.Now)
                {
                    var staff = club.Members.OfType<Staff>().FirstOrDefault(s => s.ID == task.Member_ID);
                    if (staff != null)
                    {
                        staff.DateOfEndTask = null;
                        club.Members.OfType<Player>().FirstOrDefault(p => p.Number == task.Player_Number).IsInjured = false;
                        club.EndTask(staff);
                    }
                }
            }
            using (var context = new AppDbContext())
            {
                var lineup = context.lineup.ToList();
                foreach (var player in lineup)
                {
                    var currplayer = context.players.FirstOrDefault(p => p.Number == player.Number);
                    club.Lineup.Add(
                        currplayer,
                        player.Position
                    );
                }
            }
            while (true)
            {
                Console.WriteLine("Insert email: ");
                string email = Console.ReadLine();
                Console.WriteLine("Insert password: ");
                string password = Console.ReadLine();
                while (!Login(password, email))
                {
                    Console.ReadKey();
                    Console.Clear();
                    Console.WriteLine("Insert email: ");
                    email = Console.ReadLine();
                    Console.WriteLine("Insert password: ");
                    password = Console.ReadLine();
                }
                bool islogged = true;
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("Welcome to the club!");
                ClubMember clubMember;
                List<string> firstNames = new List<string>
            {
                "Adam", "Bartosz", "Cezary", "Damian", "Emil", "Filip",
                "Grzegorz", "Hubert", "Igor", "Jakub", "Kamil", "Łukasz",
                "Mateusz", "Norbert", "Oskar", "Patryk", "Rafał", "Sebastian",
                "Tomasz", "Wojciech", "Zbigniew", "Adrian", "Daniel", "Ernest",
                "Fryderyk", "Henryk", "Janusz", "Karol", "Leon", "Michał"
            };

                List<string> lastNames = new List<string>
            {
                "Kowalski", "Nowak", "Wiśniewski", "Wójcik", "Kaczmarek", "Mazur",
                "Zieliński", "Szymański", "Woźniak", "Dąbrowski", "Zając", "Król",
                "Pawlak", "Dudek", "Piotrowski", "Kubiak", "Sobczak", "Malinowski",
                "Jaworski", "Górski", "Lis", "Baran", "Czarnecki", "Błaszczyk",
                "Chmielewski", "Wróbel", "Sikora", "Olejniczak", "Kołodziej", "Kamiński"
            };
                using (var context = new AppDbContext())
                {
                    var curruser = context.logDatas.FirstOrDefault(ld => ld.Login == email).Member_ID;
                    clubMember = context.players.FirstOrDefault(p => p.Number == curruser) ?? (ClubMember)context.staff.FirstOrDefault(s => s.ID == curruser);
                }
                while (islogged)
                {
                    Console.WriteLine("Please select an option");
                    if (clubMember is Player player)
                    {
                        if (club.HasThatPermission(player, "Chat"))
                        {
                            Console.WriteLine($"1. Chat");
                        }
                        if (club.HasThatPermission(player, "See lineup"))
                        {
                            Console.WriteLine($"2. See lineup");
                        }
                        if (club.HasThatPermission(player, "Train"))
                        {
                            Console.WriteLine($"3. Train");
                        }
                        Console.WriteLine($"4. Logout");
                        Console.WriteLine($"5. Exit");
                        string choice = Console.ReadLine();
                        Console.Clear();
                        switch (choice)
                        {
                            case "1":
                                Console.WriteLine("1. Send message");
                                Console.WriteLine("2. See messages");
                                string choice1 = Console.ReadLine();
                                Console.Clear();
                                switch (choice1)
                                {
                                    case "1":
                                        club.SendMessage(player);
                                        break;
                                    case "2":
                                        club.MyMessages(player);
                                        break;
                                    default:
                                        Console.WriteLine("Invalid choice");
                                        break;
                                }
                                break;
                            case "2":
                                club.SeeLineup();
                                break;
                            case "3":
                                player.Train();
                                break;
                            case "4":
                                islogged = false;
                                Console.WriteLine("See you soon");
                                break;
                            case "5":
                                Console.WriteLine("Thanks for using our program");
                                Environment.Exit(0);
                                break;
                            default:
                                Console.WriteLine("Invalid choice");
                                break;
                        }

                    }
                    else if (clubMember is Staff staff)
                    {
                        if (club.HasThatPermission(staff, "Chat"))
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"1. Chat");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"1. Chat");
                        }
                        if (club.HasThatPermission(staff, "Make lineup"))
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"2. Make lineup");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"2. Make lineup");
                        }
                        if (club.HasThatPermission(staff, "See lineup"))
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"3. See lineup");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"3. See lineup");
                        }
                        if (club.HasThatPermission(staff, "Make team training session"))
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"4. Make team training session");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"4. Make team training session");
                        }
                        if (club.HasThatPermission(staff, "Hire staff"))
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"5. Hire club member");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"5. Hire club member");
                        }
                        if (club.HasThatPermission(staff, "Sack staff"))
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"6. Sack club member");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"6. Sack club member");
                        }
                        if (club.HasThatPermission(staff, "Heal player"))
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine($"7. Heal player");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"7. Heal player");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("8. Logout");
                        Console.WriteLine("9. Exit");
                        string choice = Console.ReadLine();
                        Console.Clear();
                        switch (choice)
                        {
                            case "1":
                                Console.WriteLine("1. Send message");
                                Console.WriteLine("2. See messages");
                                string choice1 = Console.ReadLine();
                                Console.Clear();
                                switch (choice1)
                                {
                                    case "1":
                                        club.SendMessage(staff);
                                        break;
                                    case "2":
                                        club.MyMessages(staff);
                                        break;
                                    default:
                                        Console.WriteLine("Invalid choice");
                                        break;
                                }
                                break;
                            case "2":
                                if (club.HasThatPermission(staff, "Make lineup"))
                                {
                                    club.SetLineup();
                                }
                                else
                                {
                                    Console.WriteLine("You don't have permission to do that");
                                }
                                break;
                            case "3":
                                if (club.HasThatPermission(staff, "See lineup"))
                                {
                                    club.SeeLineup();
                                }
                                else
                                {
                                    Console.WriteLine("You don't have permission to do that");
                                }
                                break;
                            case "4":
                                if (club.HasThatPermission(staff, "Make team training session"))
                                {
                                    club.MakeTeamTraining();
                                }
                                else
                                {
                                    Console.WriteLine("You don't have permission to do that");
                                }
                                break;
                            case "5":
                                if (club.HasThatPermission(staff, "Hire staff"))
                                {

                                    Random random = new Random();
                                    Console.WriteLine("Select type of club member");
                                    Console.WriteLine("1. Player");
                                    Console.WriteLine("2. Staff");
                                    string choice2 = Console.ReadLine();
                                    Console.Clear();
                                    switch (choice2)
                                    {
                                        case "1":
                                            Console.WriteLine("Type position");
                                            string position;
                                            Position pos;
                                            while (true)
                                            {

                                                position = Console.ReadLine();
                                                if (Enum.TryParse(position, true, out pos) && Enum.IsDefined(typeof(Position), pos))
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Invalid position. Please enter a valid position:");
                                                }
                                            }
                                            Console.WriteLine("Select player number:");
                                            int playerNumber;
                                            var playerlist = club.Members.OfType<Player>().ToList();
                                            while (int.TryParse(Console.ReadLine(), out playerNumber) == false || playerNumber < 1 || playerNumber > 99 || playerlist.Where(p => p.Number == playerNumber).Count() != 0)
                                            {
                                                Console.WriteLine("Invalid choice. Please select a valid player number.");
                                            }

                                            Player player1;
                                            if (position == "Goalkeeper")
                                            {
                                                player1 = new Goalkeeper(playerNumber, random.Next(40, 90), pos, random.Next(40, 90), random.Next(40, 90), random.Next(40, 90), random.Next(40, 90), random.Next(40, 90), random.Next(40, 90), false, firstNames[random.Next(30)], lastNames[random.Next(30)], 18);
                                            }
                                            else
                                            {
                                                player1 = new Player(playerNumber, pos, random.Next(40, 90), random.Next(40, 90), random.Next(40, 90), random.Next(40, 90), random.Next(40, 90), random.Next(40, 90), false, firstNames[random.Next(30)], lastNames[random.Next(30)], 18);
                                            }
                                            club.HireClubMember(player1);
                                            break;
                                        case "2":
                                            Console.WriteLine("Select type of staff");
                                            Console.WriteLine($"Medic");
                                            Console.WriteLine($"Coach");
                                            string stafftype;
                                            Role role;
                                            while (true)
                                            {
                                                stafftype = Console.ReadLine();
                                                if (Enum.TryParse(stafftype, true, out role) && Enum.IsDefined(typeof(Role), role))
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Invalid role. Please enter a valid role:");
                                                }
                                            }
                                            var staffmaxid = club.Members.OfType<Staff>().Max(s => s.ID)+1;
                                            Staff staff1 = new Staff(role, firstNames[random.Next(30)], lastNames[random.Next(30)], random.Next(2, 40), random.Next(1, 10), null,staffmaxid);
                                            club.HireClubMember(staff1);


                                            break;
                                        default:
                                            Console.WriteLine("Invalid choice");
                                            break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("You don't have permission to do that");
                                }
                                break;
                            case "6":
                                if (club.HasThatPermission(staff, "Sack staff"))
                                {
                                    int i = 1;
                                    Console.WriteLine("Select club member to sack");
                                    foreach (var member in club.Members)
                                    {
                                        Console.WriteLine($"{i}. {member.FirstName} {member.LastName}");
                                        i++;

                                    }
                                    int choice2 = 0;
                                    while (int.TryParse(Console.ReadLine(), out choice2) == false || choice2 < 1 || choice2 > club.Members.Count)
                                    {
                                        Console.WriteLine("Invalid choice. Please select a valid club member number.");
                                    }
                                    choice2 -= 1;
                                    club.SackClubMember(club.Members[choice2]);
                                }
                                else
                                {
                                    Console.WriteLine("You don't have permission to do that");
                                }
                                break;
                            case "7":
                                if (club.HasThatPermission(staff, "Heal player"))
                                {
                                    int i = 1;
                                    Console.WriteLine("Select player to heal");
                                    var list = club.Members.OfType<Player>().Where(p => p.IsInjured).ToList();
                                    foreach (var member in list)
                                    {
                                        Console.WriteLine($"{i}. {member.FirstName} {member.LastName}");
                                        i++;
                                    }
                                    int choice2 = 0;
                                    while (int.TryParse(Console.ReadLine(), out choice2) == false || choice2 < 1 || choice2 > club.Members.OfType<Player>().Where(p => p.IsInjured).Count())
                                    {
                                        Console.WriteLine("Invalid choice. Please select a valid player number.");
                                    }
                                    choice2 -= 1;
                                    Player selectedPlayer = list[choice2];
                                    List<Player> players = club.Members.OfType<Player>().ToList();
                                    staff.StartTask(players.Where(p => p.Number == selectedPlayer.Number).FirstOrDefault());
                                }
                                else
                                {
                                    Console.WriteLine("You don't have permission to do that");
                                }
                                break;
                            case "8":
                                islogged = false;
                                Console.WriteLine("See you soon");
                                break;
                            case "9":
                                Console.WriteLine("Thanks for using our program");
                                Environment.Exit(0);
                                break;
                            default:
                                Console.WriteLine("Invalid choice");
                                break;
                        }
                    }
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}
