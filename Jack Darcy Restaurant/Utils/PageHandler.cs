using Jack_Darcy_Restaurant.Pages;
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
            } else if  (newPage == 1)
            {
                Console.WriteLine("Login Comes Here");
            } else if (newPage == 2)
            {
                Console.WriteLine("Register Comes Here");
            } else if (newPage == 3) 
            {
                MainMenu.Show();
            } else
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
