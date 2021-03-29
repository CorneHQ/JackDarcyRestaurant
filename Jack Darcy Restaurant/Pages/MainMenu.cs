using Jack_Darcy_Restaurant.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jack_Darcy_Restaurant.Pages
{
    public class MainMenu
    {
        public static void Show()
        {
            Console.WriteLine("Welcome to Jack Darcy's! Choose one of the following options to go to the desired page: \n" +
            $"[0] About \n" +
            $"[1] Login \n" +
            $"[2] Register \n" +
            $"[3] Show Menus");

            string Output = Console.ReadLine();
            int Page;
            bool success = Int32.TryParse(Output, out Page);
            if (success && Page != -1)
                PageHandler.switchPage(Page);
            else
                PageHandler.switchPage(int.MaxValue);
        }
    }
}
