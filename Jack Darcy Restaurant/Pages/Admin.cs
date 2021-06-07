using System;
using System.Collections.Generic;
using System.Text;
using ConsoleTables;
using Jack_Darcy_Restaurant.Models;
using System.Linq;

namespace Jack_Darcy_Restaurant.Pages
{
    class Admin
    {
        public static void ManageUsers()
        {
            // get all user that are not the logged in user and add them to the table
            User[] users = DB.LoadUser().Where(el => el.Id != Manager.User.Id).ToArray();
            ConsoleTable userTable = new ConsoleTable("ID", "Name", "Role name");
            foreach (User item in users)
            {
                Role role = DB.GetRole(item.Role_Id);
                userTable.AddRow(item.Id, item.Name, role.Name);
            }

            userTable.Write(Format.Minimal);
            Console.WriteLine("Choose a user to manage, or press enter without typing anything to go to the main menu:");
            string answer = Console.ReadLine();
            if (answer == "")
            {
                Utils.PageHandler.switchPage(-1);
            }
            int idRole;
            Console.Clear();
            // find user by id or name and if found give a choice
            User chosenUser = Array.Find(users, el => el.Name == answer || (int.TryParse(answer, out idRole) && idRole == el.Id));
            if(chosenUser == null || chosenUser.Id == Manager.User.Id)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("Could not find a user with that user ID \n");
                Console.ResetColor();
                Admin.ManageUsers();
            }

            Console.WriteLine("Choose the desired operation: \n [0] Change user role \n [1] Delete user");
            answer = Console.ReadLine();
            if (answer == "0")
            {
                // get all the role in the table and the ans the users give if it's found update the user with  that role
                Console.Clear();
                ConsoleTable roleTable = new ConsoleTable("ID", "Name");
                foreach (Role item in DB.GetAllRole())
                {
                    roleTable.AddRow(item.Id, item.Name);
                }
                roleTable.Write(Format.Minimal);
                Console.WriteLine("Choose a role for the user:");
                answer = Console.ReadLine();
                Console.Clear();
                if (int.TryParse(answer, out idRole))
                {
                    chosenUser.Role_Id = idRole;
                }
                else
                {
                    chosenUser.Role_Id = DB.GetRole(answer).Id;
                }

                if (DB.GetRole(chosenUser.Role_Id) == null)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Could not find role \n");
                    Console.ResetColor();
                    Admin.ManageUsers();
                }

                if (DB.UpdateUser(chosenUser))
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("The role has been successfully edited \n");
                    Console.ResetColor();
                    Admin.ManageUsers();
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Something went wrong \n");
                    Console.ResetColor();
                    Admin.ManageUsers();
                }
            }
            else if (answer == "1")
            {
                Console.Clear();
                Console.WriteLine("Are you sure you want to delete this user? Type 'Yes' to confirm, or anything else to cancel");
                answer = Console.ReadLine().ToLower();
                if (answer == "yes")
                {
                    DB.RemoveUser(chosenUser);
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("The user has been removed \n");
                    Console.ResetColor();
                    Admin.ManageUsers();
                }
                else
                {
                    Console.Clear();
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("The user has not been removed \n");
                    Console.ResetColor();
                    Admin.ManageUsers();
                }
            }    
            else
            {
                Console.Clear();
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("The command could not be recognised \n");
                Console.ResetColor();
                Admin.ManageUsers();
            }
        }
    }
}
