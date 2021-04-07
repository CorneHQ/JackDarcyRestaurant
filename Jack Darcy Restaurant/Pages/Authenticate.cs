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
            Console.WriteLine("login");
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
                }
                else
                {
                    password += temp.ToString().ToLower();
                }
            }


            Console.Clear();
            User[] users = DB.LoadUser();
            bool validate = false;
            if(users.Length == 0)
            {
                Console.WriteLine("Failed no user exist");
                Program.ToMainMenu();
                return;
            }
            
            if (username != "" && password != "")
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
                Console.WriteLine("blank Username/Password");
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
            Console.WriteLine("register");
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
                } else
                {
                    password += temp.ToString().ToLower();
                }
            }
            Console.Clear();
            Console.WriteLine("Enter your Email");
            string email = Console.ReadLine();

            Console.Clear();
            if (username == "" || password == "" || email == "")
            {
                Console.WriteLine("No empty field allowed");
                Program.ToMainMenu();
                return;
            }
            User user = new User(1, username, password, email, 0);
            if(DB.SetUser(user) && user.Validate(username, password))
            {
                Console.WriteLine($"Welcome {username}, {Manager.Role.Name}");
            } else
            {
                Console.WriteLine("Something went wrong. Try again");
            }

            Program.ToMainMenu();
        }
    }

    

 

    
}
