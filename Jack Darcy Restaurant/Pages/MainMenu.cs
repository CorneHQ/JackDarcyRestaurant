using Jack_Darcy_Restaurant.Utils;
using Jack_Darcy_Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jack_Darcy_Restaurant.Pages
{
    public class MainMenu
    {
        public static void Show()//main menu 
        {
            string s = "Welcome to Jack Darcy's! Choose one of the following options to go to the desired page: \n";

            s += $"[0] About \n";

            if (Manager.User == null) s += $"[1] Login \n";

            if (Manager.User == null) s += $"[2] Register \n";

            if (Manager.User != null) s += $"[3] Show Menus \n";
            if (Manager.User != null ) s += $"[4] Reservations\n";
            if (Manager.User != null && Manager.Role.Name.ToLower() == "owner") s += $"[5] Manage users\n";
            if (Manager.User != null) s += $"[6] View Cart\n";
            if (Manager.User != null) s += $"[7] Order\n";
            if (Manager.User != null) s += $"[8] Logout\n";

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
