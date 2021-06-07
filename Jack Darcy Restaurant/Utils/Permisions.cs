using System;
using System.Collections.Generic;
using System.Text;
using Jack_Darcy_Restaurant.Models;

namespace Jack_Darcy_Restaurant.Utils
{
    class Permisions
    {
        public static void CheckPermisions(int page)
        {   //0 login/register
            //1 add_reservation
            //2 See_Reservation
            //3 Cancel_Reservation
            //4 See_Menu
            //5 Add_Menu
            //6 Edit_Menu
            //7 Assign_Roles
            //8 Edit_Roles
            //9 See_Transactions
            //10 See_Takeaway_Orders           
            //11 Access_Shopping_Cart
            // for every case we see if a role or user exist and has the permision

            if (page == 0) {
                if(Manager.User != null)
                {
                    Console.WriteLine("you are already logged in");
                    Program.ToMainMenu();
                }
            } else if(page == 1)
            {
                if(!(Manager.Role != null && Manager.Role.Add_Reservation || Manager.Role.Add_Reservation_Customer))
                {
                    Console.WriteLine("you are not permitted to see this page");
                    Program.ToMainMenu();
                }
            } else if(page == 2)
            {
                if (!(Manager.Role != null && Manager.Role.See_All_Reservations || Manager.Role.See_Reservation))
                {
                    Console.WriteLine("you are not permitted to see this page");
                    Program.ToMainMenu();
                }
            } else if(page == 3)
            {
                if(!(Manager.Role != null && Manager.Role.Cancel_Reservation))
                {
                    Console.WriteLine("you are not permitted to see this page");
                    Program.ToMainMenu();
                }
            } else if (page == 4)
            {
                if (!(Manager.Role != null && Manager.Role.See_Menu))
                {
                    Console.WriteLine("you are not permitted to see this page");
                    Program.ToMainMenu();
                }
            } else if (page == 5)
            {
                if (!(Manager.Role != null && Manager.Role.Add_Menu))
                {
                    Console.WriteLine("you are not permitted to see this page");
                    Program.ToMainMenu();
                }
            } else if (page == 6)
            {
                if (!(Manager.Role != null && Manager.Role.See_Menu))
                {
                    Console.WriteLine("you are not permitted to see this page");
                    Program.ToMainMenu();
                }
            } else if (page == 7)
            {
                if (!(Manager.Role != null && Manager.Role.Edit_Menu))
                {
                    Console.WriteLine("you are not permitted to see this page");
                    Program.ToMainMenu();
                }
            } else if (page == 8)
            {
                if (!(Manager.Role != null && Manager.Role.Edit_Roles))
                {
                    Console.WriteLine("you are not permitted to see this page");
                    Program.ToMainMenu();
                }
            } else if (page == 9)
            {
                if (!(Manager.Role != null && Manager.Role.See_Transactions))
                {
                    Console.WriteLine("you are not permitted to see this page");
                    Program.ToMainMenu();
                }
            } else if (page == 10)
            {
                if (!(Manager.Role != null && Manager.Role.See_Takeaway_Orders))
                {
                    Console.WriteLine("you are not permitted to see this page");
                    Program.ToMainMenu();
                }
            } else if (page == 11)
            {
                if (!(Manager.Role != null && Manager.Role.Access_Shopping_Card))
                {
                    Console.WriteLine("you are not permitted to see this page");
                    Program.ToMainMenu();
                }
            }
        }
    }
}
