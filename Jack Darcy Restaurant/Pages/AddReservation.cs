using Jack_Darcy_Restaurant.Models;
using Jack_Darcy_Restaurant.Utils;
using JsonFlatFileDataStore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jack_Darcy_Restaurant.Pages
{
    public class AddReservation
    {
        public static List<string> errors;

        public static void showForm()
        {
            Console.Clear();
            showErrors();
            Console.WriteLine("What is the date you want to make a reservation for? [dd-mm-yyyy]");
            string dateStr = Console.ReadLine();
            Console.WriteLine("What is the start time for your reservation? [hh:mm]");
            string startTimeStr = Console.ReadLine();
            Console.WriteLine("How many blocks of two hours do you want to stay?");
            string endTimeStr = Console.ReadLine();
            Console.WriteLine("With how many people are coming?");
            string amountOfPeopleStr = Console.ReadLine();

            DateTime date;
            if (!DateTime.TryParse(dateStr, out date))
            {
                addError("The given date is not a valid date");
            }

            double dateDiff = (date.Date - DateTime.Today).TotalDays;
            if(dateDiff <= 0)
            {
                addError("The given date needs to be later than today or earlier");
            }

            TimeSpan startTime;
            if (!TimeSpan.TryParse(startTimeStr, out startTime))
            {
                addError("The given start time is not a valid start time");
            }

            int endTimeInt;
            if (!Int32.TryParse(endTimeStr, out endTimeInt))
            {
                addError("The given end time is not a valid number");
            }

            if (startTime.Hours < 12 || startTime.Hours > 22)
            {
                addError("The given start time is outside the opening times of the restaurant");
            }

            int amountOfPeople;
            if (!Int32.TryParse(amountOfPeopleStr, out amountOfPeople))
            {
                addError("The given amount of people is not a valid number");
            }

            TimeSpan endTime = startTime.Add(new TimeSpan(endTimeInt * 2, 0, 0));
            if (endTime.Hours < 14 && endTime.Hours > 0)
            {
                addError("The given end time is outside the opening times of the restaurant");
            }

            if (errors != null && errors.Count > 0)
                showForm();

            DataStore store = new DataStore("data.json");
            var collection = store.GetCollection<Models.Reservation>();
            string code = Str.randomRegistrationKey();
            Models.Reservation newReservation = new Models.Reservation { Id = 0, User_Id = Manager.User.Id, Reservation_Date = date, Amount_People = amountOfPeople, From = startTime, Till = endTime, Code = code };
            collection.InsertOne(newReservation);

            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Green;
            Console.WriteLine("\n");
            Console.WriteLine($"Reservation succesfully added to your reservations with the code {code}");
            Console.WriteLine("\n");
            Console.ResetColor();

            Program.ToMainMenu();
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
    }
}
