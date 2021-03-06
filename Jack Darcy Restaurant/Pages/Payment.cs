﻿using Jack_Darcy_Restaurant.Models;
using System;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Jack_Darcy_Restaurant.Pages
{
    class Payment
    {
        public static void Pay(double amount)
        {
            Console.Clear();
            if (amount == 0)
            {
                Console.WriteLine("The price is 0,00 Euro, please continue");
                Program.ToMainMenu();
            }
            Console.WriteLine($"Your amount is ${amount}");
            
            Console.Clear();
            Console.WriteLine("Please enter your card number");
            string cardId = Console.ReadLine();
            if(cardId.Length < 8 && cardId.Length > 18)
            {
                Console.WriteLine("credit card does is not long enough");
                Program.ToMainMenu();
            }
            
            Console.Clear();
            Console.WriteLine("Please enter your expiration date (MM/YYYY)");
            string expiryDate = Console.ReadLine();
            
            Console.Clear();
            Console.WriteLine("Please enter your cvv");
            string cvv = Console.ReadLine();

            Console.Clear();
            BigInteger number;
            if(!BigInteger.TryParse(cardId, out number))
            {
                Console.WriteLine("Credit card is not a number");
                Program.ToMainMenu();
            }
            int sum = 0;
            int temp;
            bool b = true;
            //https://en.wikipedia.org/wiki/Luhn_algorithm
            //using luhn algorith where is look at the last number each loop
            while (number > 0)
            {

                temp = b ? (int)(number % 10)  * 1: (int)(number % 10) * 2;
                b = !b;
                if((int)temp / 10 % 10 != 0)
                {
                    temp = ((int)temp / 10 % 10) + temp % 10;
                }

                sum += temp;
                number = number / 10;
            }
            // its valid if sum% 0 == 0 else it's not valid
            if (sum % 10 != 0)
            {
                Console.WriteLine("Card number not valid");
                Program.ToMainMenu();
            }
            // made regex to validate the date and cvv
            Regex monthCheck = new Regex(@"^(0[0-9]|1[0-2])$");
            Regex yearCheck = new Regex(@"^20[0-9]{2}$");
            Regex cvvCheck = new Regex(@"^\d{3}$");

            if (!cvvCheck.IsMatch(cvv))
            {
                Console.WriteLine("CVV invalid");
                Program.ToMainMenu();
            }
            
            var dateParts = expiryDate.Split('/');
            if (!monthCheck.IsMatch(dateParts[0]) || !yearCheck.IsMatch(dateParts[1]))
            {
                Console.WriteLine("Invalid date");
                Program.ToMainMenu();
            }

            var year = int.Parse(dateParts[1]);
            var month = int.Parse(dateParts[0]);
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month); //get the last day of the given month and year
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);// the full date when it wil expire

            // check if the card is expired
            if (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6))
            {
                Manager.User.Cart.Clear();
                DB.UpdateUser(Manager.User);
                Console.WriteLine("Processing");
                System.Threading.Thread.Sleep(2000);
                Console.WriteLine("Payment succesfull");
                Program.ToMainMenu();
            }
        }
    }
}
