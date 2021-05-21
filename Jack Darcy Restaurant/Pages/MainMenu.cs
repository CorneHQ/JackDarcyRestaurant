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

            s += $"[3] Show Menus \n";
            if (Manager.Role != null ) s += $"[4] Reservations\n";
            if (Manager.User != null) s += $"[5] Logout\n";
            if (Manager.Role != null && Manager.Role.Name.ToLower() == "owner") s += $"[6] Manage users\n";
            if (Manager.User != null) s += $"[7] Payment\n";
            if (Manager.User != null) s += $"[8] Order\n";
            if (Manager.User != null) s += $"[9] View Cart\n";

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
