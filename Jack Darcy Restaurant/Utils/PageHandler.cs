using Jack_Darcy_Restaurant.Pages;
using Jack_Darcy_Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jack_Darcy_Restaurant.Utils
{
    public class PageHandler
    {
        // Switches to a page with the given page ID
        public static void switchPage(int newPage)
        {
            Console.Clear();
            if (newPage == 0)
            {
                About.Show();
            }
            else if (Manager.User == null && newPage == 1) // Manager.User is null if the user isn't logged in, so only non-logged in users can see these pages
            {
                Authenticate.Login();
            }
            else if (Manager.User == null && newPage == 2)
            {
                Authenticate.Register();
            }
            else if (Manager.User != null && newPage == 3) 
            {
                Menus.PageHandlerMenu();
            } 
            else if(Manager.Role != null && (Manager.Role.Add_Reservation_Customer || Manager.Role.See_Reservation || Manager.Role.Add_Reservation || Manager.Role.See_All_Reservations) && newPage == 4)
            {
                Pages.Reservation.showMenu();
            }
            else if (Manager.User != null && newPage == 8)
            {
                Authenticate.Logout();
            }
            else if (Manager.User != null && newPage == 5 && Manager.Role.Name.ToLower() == "owner")
            {
                Admin.ManageUsers();
            } else if(Manager.User != null && newPage == 6)
            {
                ViewCart.CurrentCart();
            }
            else if (Manager.User != null && newPage == 7)
            {
                Menus.ShowMenus();
            } if (newPage == -1)
            {
                MainMenu.Show();
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("The requested page does not exist! \n");
                Console.ResetColor();
                MainMenu.Show();
            } 
        }
    }
}
