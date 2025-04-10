using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using static Clubs.Classes.goalkeeper;
using static Clubs.Classes.player;
using static Clubs.Classes.staff;
using static Clubs.login;

using static Clubs.Program;
using Task = Clubs.Program.Task;



namespace Clubs
{


    internal class AppDbContext : DbContext
    {
        public DbSet<Player> players { get; set; }
        public DbSet<Staff> staff { get; set; }
        public DbSet<Goalkeeper> goalkeepers { get; set; }
        public DbSet<Message> messages { get; set; }
        public DbSet<LogData> logDatas { get; set; }
        public DbSet<Task> tasks { get; set; }
        public DbSet<Lineup> lineup { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;Port=3306;Database=club;User=root;Password='';",
                new MySqlServerVersion(new Version(11, 6, 0)));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().HasKey(p => p.Number);
            modelBuilder.Entity<Staff>().HasKey(s => s.ID);
            modelBuilder.Entity<LogData>().HasKey(l => l.Member_ID);
            modelBuilder.Entity<Task>().HasKey(t => t.Member_ID);
            modelBuilder.Entity<Message>().HasKey(m => m.ID);
            modelBuilder.Entity<Lineup>().HasKey(l => l.Number);
            modelBuilder.Entity<Player>().ToTable("players");
            modelBuilder.Entity<Staff>().ToTable("staff");
            modelBuilder.Entity<Goalkeeper>().ToTable("goalkeepers");
            modelBuilder.Entity<Message>().ToTable("messages");
            modelBuilder.Entity<LogData>().ToTable("logdatas");
            modelBuilder.Entity<Task>().ToTable("tasks");
            modelBuilder.Entity<Lineup>().ToTable("lineup");
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
            modelBuilder.Entity<Lineup>().Property(l => l.Position).HasConversion(
                v => v.ToString(), v => (Position)Enum.Parse(typeof(Position), v));
        }
    }
}

