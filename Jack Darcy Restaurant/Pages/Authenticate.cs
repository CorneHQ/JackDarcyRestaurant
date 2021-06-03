using System;
using Jack_Darcy_Restaurant.Models;
using Jack_Darcy_Restaurant.Utils;

namespace Jack_Darcy_Restaurant.Pages
{

    class Authenticate
    {

        public static void Login()
        {
            Console.Clear();
            Permisions.CheckPermisions(0);
            Console.WriteLine("Login");
            Console.WriteLine("Enter your Username:");
            string username = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Enter your Password:");
            string password = "";
            bool stop = true;
            while (stop) {
                ConsoleKey temp = Console.ReadKey(true).Key;
                if (temp == ConsoleKey.Enter)
                {
                    stop = false;
                }
                else if (temp == ConsoleKey.Backspace)
                {
                    if (password.Length == 0) continue;
                    password = password.Remove(password.Length - 1);
                    Console.Write("\b \b");
                }
                else
                {
                    password += temp.ToString().ToLower();
                    Console.Write("*");
                }
            }


            Console.Clear();
            User[] users = DB.LoadUser();
            
            bool validate = false;
            if(users.ToString().Contains(username))
            {
                Console.WriteLine("Failed username exist");
                Program.ToMainMenu();
                return;
            }

            if (users.Length == 0)
            {
                Console.WriteLine("Failed no user exist");
                Program.ToMainMenu();
                return;
            }
            
            if (username != "" || password != "")
            {

                foreach (User user in users)
                {
                    if (user.Validate(username, password))
                    {
                        // set global role here user.role;
                        validate = true;
                    }
                }

            } else
            {
                Console.WriteLine(username == "" ? "Blank Username": "Blank Password");
                Program.ToMainMenu();
                return;
            }

            if (!validate)
            {
                Console.WriteLine("Failed wrong Username/Password");
                Program.ToMainMenu();
                return;

            }

            Console.WriteLine($"Welcome {username}, {Manager.Role.Name}");
            Program.ToMainMenu();
        }

        public static void Register()
        {
            Console.Clear();
            Permisions.CheckPermisions(0);
            Console.WriteLine("Register");
            Console.WriteLine("Enter your Username:");
            string username = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Enter your Password:");
            string password = "";
            bool stop = true;
            while (stop)
            {
                ConsoleKey temp = Console.ReadKey(true).Key;
                if (temp == ConsoleKey.Enter)
                {
                    stop = false;
                } else if (temp == ConsoleKey.Backspace) {
                    if (password.Length == 0) continue;
                    password = password.Remove(password.Length - 1);
                    Console.Write("\b \b");
                } else
                {
                    password += temp.ToString().ToLower();
                    Console.Write("*");
                }
            }
            Console.Clear();
            Console.WriteLine("Enter your Email");
            string email = Console.ReadLine();

            Console.Clear();
            if (username == "" || password == "" || email == "")
            {
                Console.WriteLine(username == "" ? "No empty username allowed" : password == "" ? "No empty password allowed" : "No empty email allowed");
                Program.ToMainMenu();
                return;
            }

            foreach (User u in DB.LoadUser())
            {
                if(u.Name == username || u.Email == email)
                {
                    Console.WriteLine(u.Name == username ? "Failed username exist": "Failed email exist");
                    Program.ToMainMenu();
                    return;
                }
            }
            
            User user = new User(1, username, password, email, 0);
            if (email.Contains('@') && new System.Net.Mail.MailAddress(email).Address == email && DB.SetUser(user))
            {
                if (user.Validate(username, password))
                {
                    Console.WriteLine($"Welcome {username}, {Manager.Role.Name}");
                }
                else
                {
                    Console.WriteLine("Something went wrong. Try again");
                }
            } else
            {
                Console.WriteLine("Not a unique email");
            }

            Program.ToMainMenu();
        }

        public static void Logout()
        {
            Console.Clear();
            Console.WriteLine("Logout");
            Manager.Role = null;
            Manager.User = null;
            Program.ToMainMenu();
        }

    }
 

    
}
