using Jack_Darcy_Restaurant.Utils;
using Jack_Darcy_Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jack_Darcy_Restaurant.Pages
{
    public class MainMenu
    {
        public static void Show()
        {
            string s = "Welcome to Jack Darcy's! Choose one of the following options to go to the desired page: \n";

            s += $"[0] About \n";

            if (Manager.User == null) s += $"[1] Login \n";

            if (Manager.User == null) s += $"[2] Register \n";

            s += $"[3] Show Menus";

            Console.WriteLine(s);

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
