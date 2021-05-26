using Jack_Darcy_Restaurant.Models;
using Jack_Darcy_Restaurant.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jack_Darcy_Restaurant.Pages
{
    public class Reservation
    {
        public static List<string> errors;

        public static void showMenu()
        {
            showErrors();

            Console.WriteLine("[0] Add Reservation");
            Console.WriteLine("[1] See Reservations");
            Console.WriteLine("[2] Go back to the main menu");

            int[] allowedNumbers = new int[] { 0, 1, 2 };

            string Output = Console.ReadLine();
            int Page;
            bool success = Int32.TryParse(Output, out Page);
            if (success && allowedNumbers.Contains(Page))
                if(Page == 2)
                {
                    PageHandler.switchPage(-1);
                }
                else
                {
                    if (Page == 0)
                        addReservation();
                    else if (Page == 1)
                        if (Manager.Role.See_All_Reservations)
                            seeAllReservations();
                        else
                            seeReservations();
                }
            else
            {
                addError("Page could not be found!");
                PageHandler.switchPage(4);
            }
        }

        private static void showErrors()
        {
            if(errors != null && errors.Count > 0)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("\n");
                foreach (string error in errors)
                {
                    Console.WriteLine(error);
                }
                Console.WriteLine("\n");
                Console.ResetColor();
                errors.Clear();
            }
        } 

        private static void addError(string error)
        {
            if(errors == null)
                errors = new List<string>();

            errors.Add(error);
        }

        private static void addReservation()
        {
            AddReservation.showForm();
        }

        private static void seeReservations()
        {
            ReservationList.showTable();
        }

        private static void seeAllReservations()
        {
            ReservationList.showAllTable();
        }
    }
}
