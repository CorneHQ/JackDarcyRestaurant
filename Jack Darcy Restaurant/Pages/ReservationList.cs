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

        public static void showTable()
        {
            Console.Clear();
            showErrors();
            showSuccessMessages();
            DataStore store = new DataStore("data.json");

            var collectionFuture = store.GetCollection<Models.Reservation>()
                .AsQueryable()
                .Where(item => item.User_Id == Manager.User.Id && item.Reservation_Date >= DateTime.Today)
                .OrderBy(item => item.Reservation_Date);

            var collectionPast = store.GetCollection<Models.Reservation>()
                .AsQueryable()
                .Where(item => item.User_Id == Manager.User.Id && item.Reservation_Date < DateTime.Today)
                .OrderBy(item => item.Reservation_Date);

            Console.WriteLine("Your reservations at Jack Darcy \n");
            ConsoleTable reservationTable = new ConsoleTable("Date", "From", "Till", "How many people", "Reservation code");

            foreach(Models.Reservation reservation in collectionFuture)
            {
                reservationTable.AddRow(reservation.Reservation_Date.ToLongDateString(), reservation.From.ToString(), reservation.Till.ToString(), reservation.Amount_People + " People", reservation.Code);
            }

            foreach (Models.Reservation reservation in collectionPast)
            {
                reservationTable.AddRow(reservation.Reservation_Date.ToLongDateString(), reservation.From.ToString(), reservation.Till.ToString(), reservation.Amount_People + " People", reservation.Code);
            }

            reservationTable.Write(Format.Alternative);

            Console.WriteLine("\n");
            Console.WriteLine("Please press 'Enter' to go back to the main menu or press 'Backspace' to cancel a reservation");

            while(!Console.KeyAvailable)
            {
                if(Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    PageHandler.switchPage(-1);
                } else if(Console.ReadKey().Key == ConsoleKey.Backspace)
                {
                    cancelReservation();
                }
            }
        }

        public static void showAllTable()
        {
            Console.Clear();
            showErrors();
            showSuccessMessages();
            DataStore store = new DataStore("data.json");

            var collection = store.GetCollection<Models.Reservation>()
                .AsQueryable()
                .Where(item => item.Reservation_Date >= DateTime.Today)
                .OrderBy(item => item.Reservation_Date);

            Console.WriteLine("All the reservations at Jack Darcy \n");

            ConsoleTable reservationTable = new ConsoleTable("Date", "From", "Till", "How many people", "Reservation code");
            foreach (Models.Reservation reservation in collection)
            {
                reservationTable.AddRow(reservation.Reservation_Date.ToLongDateString(), reservation.From.ToString(), reservation.Till.ToString(), reservation.Amount_People + " People", reservation.Code);
            }

            reservationTable.Write(Format.Alternative);

            Console.WriteLine("\n");
            Console.WriteLine("Please press 'Enter' to go back to the main menu or press 'Backspace' to cancel a reservation");

            while (!Console.KeyAvailable)
            {
                if (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    PageHandler.switchPage(-1);
                }
                else if (Console.ReadKey().Key == ConsoleKey.Backspace)
                {
                    cancelReservation();
                } else { }
            }
        }

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

            DateTime currentDate = item.Reservation_Date.Date + item.Till;
            if((currentDate.Subtract(DateTime.Now).TotalHours) <= 24) {
                addError("You cannot cancel a reservation within 24 hours of the reservation");
                Console.Clear();
                if (Manager.Role.See_All_Reservations)
                    showAllTable();
                else
                    showTable();
            }

            Console.WriteLine("\nYour reservation details:\n");
            Console.WriteLine($"Date: {item.Reservation_Date.ToLongDateString()}\n");
            Console.WriteLine($"From: {item.From}\n");
            Console.WriteLine($"Till: {item.Till}\n");
            Console.WriteLine($"Amount of people: {item.Amount_People}\n");
            Console.WriteLine($"Code: {item.Code}\n");
            Console.WriteLine("Are you sure, you want to cancel this reservation? [Y/N]");
            while(!Console.KeyAvailable)
            {
                if(Console.ReadKey().Key == ConsoleKey.N)
                {
                    Console.Clear();
                    if (Manager.Role.See_All_Reservations)
                        showAllTable();
                    else
                        showTable();
                } else if(Console.ReadKey().Key == ConsoleKey.Y)
                {
                    var deleteCollection = store.GetCollection<Models.Reservation>();
                    deleteCollection.DeleteOne(item.Id);
                    addSuccessMessage("We cancelled your reservation");
                    if (Manager.Role.See_All_Reservations)
                        showAllTable();
                    else
                        showTable();
                } else { }
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
