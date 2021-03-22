using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;



/// <summary>
/// text opmaken
/// background css
/// clear 
/// </summary>
namespace Jack_Darcy_Restaurant.Model
{
        
    class Authenticate
    {

        public void Login()
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
                } else
                {
                    password += temp.ToString().ToLower();
                }
            }
            

            Console.Clear();
            User[] users = new DB().LoadUser();
            bool validate = false;
            if (username != "" && password != "")
            {

                foreach (User user in users)
                {
                    if (user.Validate(username ,password))
                    {
                        // set global role here user.role;
                        validate = true;
                    }
                }

            } else
            {
                Console.WriteLine("blank Username/Password");
                return;
            }
            if (!validate)
            {
                Console.WriteLine("Failed wrong Username/Password");
                return; 
                
            }

            Console.WriteLine($"Welcome {username}, {Manager.Role_id}");
        }

        public void Register()
        {
            //string jsonString = File.ReadAllText("../../../DB.json");
            //Console.WriteLine(jsonString);
            //User test = JsonSerializer.Deserialize<User>(jsonString);
            //Console.WriteLine(test);

            Console.WriteLine("register");
            Console.WriteLine("Enter your Username:");
            string username = Console.ReadLine();

            Console.WriteLine("Enter your Password:");
            string password = Console.ReadLine();

            //User user = new User(username, password, "customer");
            //string jsonString = JsonSerializer.Serialize<User>(user);
            //Console.WriteLine(jsonString);
            //File.WriteAllText("test.json" , jsonString);
        }
    }

    public class DB
    {
        public User[] LoadUser()
        {
            

            User[] users = {
                    new User(0, "djamal", "king", "save@gmail.com", 0),
                    new User(1, "djamal2", "king", "chef@gmail.com", 1)
                };
            return users;
        }

        
    }

 

    
}
