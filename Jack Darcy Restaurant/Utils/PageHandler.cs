using Jack_Darcy_Restaurant.Pages;
using Jack_Darcy_Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jack_Darcy_Restaurant.Utils
{
    public class PageHandler
    {
        public static void switchPage(int newPage)
        {
            Console.Clear();
            if (newPage == 0)
            {
                About.Show();
            }
            else if (newPage == 1)
            {
                Authenticate.Login();
            }
            else if (newPage == 2)
            {
                Authenticate.Register();
            }
            else if (newPage == 3) 
            {
                Menus.PageHandlerMenu();
            }
            else if (Manager.User != null && newPage == 5)
            {
                Authenticate.Logout();
            }
            else if (newPage == -1)
            {
                MainMenu.Show();
            }
            else
            {
                Console.WriteLine("\n");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("The requested page does not exist!");
                Console.ResetColor();
                Console.WriteLine("\n");
                MainMenu.Show();
            } 
        }
    }
}
