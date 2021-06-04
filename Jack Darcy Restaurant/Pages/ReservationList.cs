using ConsoleTables;
using Jack_Darcy_Restaurant.Models;
using Jack_Darcy_Restaurant.Utils;
using JsonFlatFileDataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jack_Darcy_Restaurant.Pages
{
    public class ReservationList
    {
        public static List<string> errors;
        public static List<string> successMessages;

        // Show the reservations when the user is a customer
        public static void showTable()
        {
            Console.Clear();
            showErrors();
            showSuccessMessages();
            DataStore store = new DataStore("data.json");

            // Get all the upcoming reservations
            var collectionFuture = store.GetCollection<Models.Reservation>()
                .AsQueryable()
                .Where(item => item.User_Id == Manager.User.Id && item.Reservation_Date >= DateTime.Today)
                .OrderBy(item => item.Reservation_Date)
                .ThenBy(item => item.From);

            // Get all the past reservations
            var collectionPast = store.GetCollection<Models.Reservation>()
                .AsQueryable()
                .Where(item => item.User_Id == Manager.User.Id && item.Reservation_Date < DateTime.Today)
                .OrderBy(item => item.Reservation_Date)
                .ThenBy(item => item.From);

            Console.WriteLine("Your reservations at Jack Darcy \n");
            ConsoleTable reservationTable = new ConsoleTable("Date", "From", "Till", "How many people", "Reservation code");

            // Load the data in a clear console table
            foreach(Models.Reservation reservation in collectionFuture)
            {
                reservationTable.AddRow(reservation.Reservation_Date.ToLongDateString(), reservation.From.ToString(@"hh\:mm"), reservation.Till.ToString(@"hh\:mm"), reservation.Amount_People + " People", reservation.Code);
            }

            foreach (Models.Reservation reservation in collectionPast)
            {
                reservationTable.AddRow(reservation.Reservation_Date.ToLongDateString(), reservation.From.ToString(@"hh\:mm"), reservation.Till.ToString(@"hh\:mm"), reservation.Amount_People + " People", reservation.Code);
            }

            reservationTable.Write(Format.Alternative);

            Console.WriteLine("\n");
            Console.WriteLine("Please press 'Enter' to go back to the main menu");
            Console.WriteLine("Please press 'Backspace' to cancel a reservation");

            // Waits for input from the user. Enter is going back to the main menu and Backspace is for cancelling a reservation
            while(true)
            {
                ConsoleKey consoleKey = Console.ReadKey().Key;
                if (consoleKey == ConsoleKey.Enter)
                {
                    PageHandler.switchPage(-1);
                    break;
                } else if(consoleKey == ConsoleKey.Backspace)
                {
                    cancelReservation();
                    break;
                } else { }
            }
        }

        // Almost the same as showTable(), but this shows all the upcomming reservations for each customer
        public static void showAllTable()
        {
            Console.Clear();
            showErrors();
            showSuccessMessages();
            DataStore store = new DataStore("data.json");

            var collection = store.GetCollection<Models.Reservation>()
                .AsQueryable()
                .Where(item => item.Reservation_Date >= DateTime.Today)
                .OrderBy(item => item.Reservation_Date)
                .ThenBy(item => item.From);

            Console.WriteLine("All the reservations at Jack Darcy \n");

            ConsoleTable reservationTable = new ConsoleTable("Date", "From", "Till", "How many people", "Reservation code");
            foreach (Models.Reservation reservation in collection)
            {
                reservationTable.AddRow(reservation.Reservation_Date.ToLongDateString(), reservation.From.ToString(@"hh\:mm"), reservation.Till.ToString(@"hh\:mm"), reservation.Amount_People + " People", reservation.Code);
            }

            reservationTable.Write(Format.Alternative);

            Console.WriteLine("Please press 'Enter' to go back to the main menu");
            Console.WriteLine("Please press 'Backspace' to cancel a reservation");

            while (true)
            {
                ConsoleKey consoleKey = Console.ReadKey().Key;
                if (consoleKey == ConsoleKey.Enter)
                {
                    PageHandler.switchPage(-1);
                    break;
                }
                else if (consoleKey == ConsoleKey.Backspace)
                {
                    cancelReservation();
                    break;
                } else { }
            }
        }

        // This function cancels a reservation from an customer
        private static void cancelReservation()
        {
            Console.Clear();

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Notice: You cannot delete a reservation that's in 24 hours from now\n");
            Console.ResetColor();

            Console.WriteLine("Can you please provide your reservation code");
            string reservationCode = Console.ReadLine();

            DataStore store = new DataStore("data.json");
            IEnumerable<Models.Reservation> collection;
            if(Manager.Role.See_All_Reservations)
            {
                collection = store.GetCollection<Models.Reservation>()
                    .AsQueryable()
                    .Where(item => item.Code == reservationCode);
            } else
            {
                collection = store.GetCollection<Models.Reservation>()
                    .AsQueryable()
                    .Where(item => item.User_Id == Manager.User.Id && item.Code == reservationCode);
            }

            // Checks if the given reservation code is found in the database
            if(collection == null || collection.Count() <= 0)
            {
                addError("Your reservation code cannot be found, please try it again");
                Console.Clear();
                if (Manager.Role.See_All_Reservations)
                    showAllTable();
                else
                    showTable();
            }

            var item = collection.First();

            // Checks if the reservation is within 24 hours if it is, it will deny the cancellation
            DateTime currentDate = item.Reservation_Date.Date + item.Till;
            if((currentDate.Subtract(DateTime.Now).TotalHours) <= 24) {
                addError("You cannot cancel a reservation within 24 hours of the reservation");
                Console.Clear();
                if (Manager.Role.See_All_Reservations)
                    showAllTable();
                else
                    showTable();
            }

            // Confirmation screen with all the information
            Console.WriteLine("\nYour reservation details:\n");
            Console.WriteLine($"Date: {item.Reservation_Date.ToLongDateString()}\n");
            Console.WriteLine($"From: {item.From}\n");
            Console.WriteLine($"Till: {item.Till}\n");
            Console.WriteLine($"Amount of people: {item.Amount_People}\n");
            Console.WriteLine($"Code: {item.Code}\n");
            Console.WriteLine("Are you sure, you want to cancel this reservation? [Y/N]");

            while(true)
            {
                ConsoleKey keyIN = Console.ReadKey().Key;
                if (keyIN == ConsoleKey.N)
                {
                    Console.Clear();
                    if (Manager.Role.See_All_Reservations)
                        showAllTable();
                    else
                        showTable();
                    break;
                } else if(keyIN == ConsoleKey.Y)
                {
                    // Deletes the reservation with the given ID
                    var deleteCollection = store.GetCollection<Models.Reservation>();
                    deleteCollection.DeleteOne(item.Id);
                    addSuccessMessage("We cancelled your reservation");
                    if (Manager.Role.See_All_Reservations)
                        showAllTable();
                    else
                        showTable();
                    break;
                }
            }
        }

        private static void showErrors()
        {
            if (errors != null && errors.Count > 0)
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
            if (errors == null)
                errors = new List<string>();

            errors.Add(error);
        }

        private static void showSuccessMessages()
        {
            if (successMessages != null && successMessages.Count > 0)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine("\n");
                foreach (string message in successMessages)
                {
                    Console.WriteLine(message);
                }
                Console.WriteLine("\n");
                Console.ResetColor();
                successMessages.Clear();
            }
        }

        private static void addSuccessMessage(string message)
        {
            if (successMessages == null)
                successMessages = new List<string>();

            successMessages.Add(message);
        }
    }
}
