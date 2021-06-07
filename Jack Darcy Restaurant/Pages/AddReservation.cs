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
    public class AddReservation
    {
        public static List<string> errors;

        // Show the form with the questions and checks if there is still room available
        public static void showForm()
        {
            Console.Clear();// getting all inputs
            showErrors();

            Console.WriteLine("What is the date you want to make a reservation for? [dd-mm-yyyy]");
            string dateStr = Console.ReadLine();
            DateTime date;
            if (!DateTime.TryParse(dateStr, out date))
            {
                addError("The given date is not a valid date");
                showForm();
            }

            double dateDiff = (date.Date - DateTime.Today).TotalDays;
            if (dateDiff <= 0)
            {
                addError("The given date needs to be later than today or earlier");
                showForm();
            }

            Console.WriteLine("\nWith how many people are coming?");
            string amountOfPeopleStr = Console.ReadLine();

            int amountOfPeople;
            if (!Int32.TryParse(amountOfPeopleStr, out amountOfPeople))
            {
                addError("The given amount of people is not a valid number");
                showForm();
            }

            if (amountOfPeople > 36)
            {
                addError("We only have place for 36 people at once, please come in smaller groups");
                showForm();
            }


            DataStore store = new DataStore("data.json");

            var beginTimeCollection = store.GetCollection<Models.Reservation>()
                .AsQueryable();

            // Checks if there is still room available with the given date
            TimeSpan startTime = new TimeSpan(12, 0, 0);
            TimeSpan endTime = new TimeSpan(14, 0, 0);
            int availableSpots = 0;
            while(!startTime.Equals(new TimeSpan(24, 0, 0)))
            {
                int amountOfSeatsFilled = 0;

                var db = beginTimeCollection.AsQueryable()
                    .Where(item => item.Reservation_Date == date && item.From == startTime && item.Till == endTime);

                foreach(Models.Reservation reservation in db)
                {
                    amountOfSeatsFilled += reservation.Amount_People;
                }

                if (36 - amountOfSeatsFilled - amountOfPeople >= 0)
                    availableSpots++;

                startTime += TimeSpan.FromHours(2);
                endTime += TimeSpan.FromHours(2);
            }

            if (availableSpots == 0)
            {
                addError("There is no place for you on this date, please choose a other date");
                showForm();
            }

            // In this tuple array all the available timeslots will be saved for later
            Tuple<TimeSpan, TimeSpan>[] availableTimes = new Tuple<TimeSpan, TimeSpan>[availableSpots];

            startTime = new TimeSpan(12, 0, 0);
            endTime = new TimeSpan(14, 0, 0);
            int i = 0;
            while (!startTime.Equals(new TimeSpan(24, 0, 0)))
            {
                int amountOfSeatsFilled = 0;

                var db = beginTimeCollection.AsQueryable()
                    .Where(item => item.Reservation_Date == date && item.From == startTime && item.Till == endTime);

                foreach (Models.Reservation reservation in db)
                {
                    amountOfSeatsFilled += reservation.Amount_People;
                }

                if (36 - amountOfSeatsFilled - amountOfPeople >= 0)
                    availableTimes[i++] = Tuple.Create(startTime, endTime);

                startTime += TimeSpan.FromHours(2);
                endTime += TimeSpan.FromHours(2);
            }

            string output = "\n";
            for(int j = 0; j < availableTimes.Length; j++)
            {
                output += $"[{j}] {availableTimes[j].Item1.ToString(@"hh\:mm")} - {availableTimes[j].Item2.ToString(@"hh\:mm")}\n";
            }
            Console.WriteLine(output);

            Console.WriteLine("Please enter the number you wish to come");
            string strTime = Console.ReadLine();

            int chosenTime;
            if (!Int32.TryParse(strTime, out chosenTime))
            {
                addError("Please enter a valid number when choosing the time");
                showForm();
            }

            // Checks if the given number is in the range of the tuple array
            if (chosenTime > availableTimes.Length - 1)
            {
                addError("Please enter a number that's on the screen when choosing the time");
                showForm();
            }

            int userID;

            // Show all the users when logged in as a owner.
            if (Manager.Role.Add_Reservation_Customer)
            {
                var usersCollection = store.GetCollection<User>().AsQueryable();

                Console.WriteLine("\n");

                ConsoleTable usersTable = new ConsoleTable("User ID", "Name", "Email");
                foreach (User user in usersCollection)
                {
                    usersTable.AddRow(user.Id, user.Name, user.Email);
                }
                usersTable.Write(Format.Alternative);

                Console.WriteLine("Please choose the user by his user ID");
                string userIDStr = Console.ReadLine();
                if (!Int32.TryParse(userIDStr, out userID))
                {
                    addError("The given user ID is not a valid number");
                    showForm();
                }
            }
            else
            {
                userID = Manager.User.Id;
            }

            var collection = store.GetCollection<Models.Reservation>();
            // Generates eight characters long unique reservation code
            string code = Str.randomRegistrationKey();
            Models.Reservation newReservation = new Models.Reservation { Id = 0, User_Id = userID, Reservation_Date = date, Amount_People = amountOfPeople, From = availableTimes[chosenTime].Item1, Till = availableTimes[chosenTime].Item2, Code = code };
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
