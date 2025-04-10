using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Clubs.Program;

using static Clubs.AppDbContext;
using System.Security.Cryptography;


namespace Clubs
{
    internal class login
    {
        public class LogData
        {
            public int Member_ID { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
            public LogData() { }
            public LogData(int member_ID, string login, string password)
            {
                Member_ID = member_ID;
                Login = login;
                Password = password;
            }
        }
        public static bool Login(string password, string email)
        {
            // Hash the password to compare with stored hashed passwords
            string hashedPassword = HashPassword(password);

            using (var context = new AppDbContext())
            {
                // Find the user by email
                var logData = context.logDatas.FirstOrDefault(ld => ld.Login == email);

                if (logData != null && logData.Password == hashedPassword)
                {
                    Console.WriteLine($"Welcome, {email}! You have successfully logged in.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Invalid login credentials. Please try again.");
                    return false;
                }
            }
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
    }
