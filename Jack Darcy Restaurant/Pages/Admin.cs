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
        public static void ChangeUser()
        {
            User[] users = DB.LoadUser().Where(el => el.Id != Manager.User.Id).ToArray();
            ConsoleTable userTable = new ConsoleTable("ID", "Name", "Role name");
            foreach (User item in users)
            {
                Role role = DB.GetRole(item.Role_Id);
                userTable.AddRow(item.Id, item.Name, role.Name);
            }

            userTable.Write(Format.Minimal);
            Console.WriteLine("choose a user to change the role");
            string answer = Console.ReadLine();
            int idRole;
            Console.Clear();
            User choosenUser = Array.Find(users, el => el.Name == answer || (int.TryParse(answer, out idRole) && idRole == el.Id));
            if(choosenUser == null || choosenUser.Id == Manager.User.Id)
            {
                Console.WriteLine("Could not find user");
                Program.ToMainMenu();
            }
            
            ConsoleTable roleTable = new ConsoleTable("ID", "Name");
            foreach (Role item in DB.GetAllRole())
            {
                roleTable.AddRow(item.Id, item.Name);
            }
            roleTable.Write(Format.Minimal);

            Console.WriteLine("choose a role for the user");
            answer = Console.ReadLine();
            Console.Clear();
            if (int.TryParse(answer, out idRole))
            {
                choosenUser.Role_Id = idRole;
            } else
            {
                choosenUser.Role_Id = DB.GetRole(answer).Id;
            }

            if (DB.GetRole(choosenUser.Role_Id) == null)
            {
                Console.WriteLine("could not find role");
                Program.ToMainMenu();
            }

            if (DB.UpdateUser(choosenUser))
            {
                Console.WriteLine("succes");
                Program.ToMainMenu();
            } else
            {
                Console.WriteLine("something went wrong");
                Program.ToMainMenu();
            }
        }
    }
}
