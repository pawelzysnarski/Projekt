using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clubs;
using static Clubs.Program;

namespace Clubs
{
    internal class player
    {
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
                else if (injurystatus >= 1 && injurystatus <= 6)
                {
                    Console.WriteLine($"Player {FirstName} {LastName} has improved");
                    int bonusstat = random.Next(6);
                    switch (bonusstat)
                    {
                        case 0:
                            Pace += 1;
                            using (var context = new AppDbContext())
                            {
                                var player = context.players.Find(Number);
                                if (player != null)
                                {
                                    player.Pace += 1;
                                    context.SaveChanges();
                                }
                            }
                            break;
                        case 1:
                            Shooting += 1;
                            using (var context = new AppDbContext())
                            {
                                var player = context.players.Find(Number);
                                if (player != null)
                                {
                                    player.Shooting += 1;
                                    context.SaveChanges();
                                }
                            }
                            break;
                        case 2:
                            Passing += 1;
                            using (var context = new AppDbContext())
                            {
                                var player = context.players.Find(Number);
                                if (player != null)
                                {
                                    player.Passing += 1;
                                    context.SaveChanges();
                                }
                            }
                            break;
                        case 3:
                            Dribling += 1;
                            using (var context = new AppDbContext())
                            {
                                var player = context.players.Find(Number);
                                if (player != null)
                                {
                                    player.Dribling += 1;
                                    context.SaveChanges();
                                }
                            }
                            break;
                        case 4:
                            Defense += 1;
                            using (var context = new AppDbContext())
                            {
                                var player = context.players.Find(Number);
                                if (player != null)
                                {
                                    player.Defense += 1;
                                    context.SaveChanges();
                                }
                            }
                            break;
                        case 5:
                            Physical += 1;
                            using (var context = new AppDbContext())
                            {
                                var player = context.players.Find(Number);
                                if (player != null)
                                {
                                    player.Physical += 1;
                                    context.SaveChanges();
                                }
                            }
                            break;
                    }
                }
                else
                {
                    Console.WriteLine($"Player {FirstName} {LastName} trained");
                }
            }
        }
    }
}
