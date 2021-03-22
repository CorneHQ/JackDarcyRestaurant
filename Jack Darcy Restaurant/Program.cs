﻿using System;
using Jack_Darcy_Restaurant.Model;


namespace Jack_Darcy_Restaurant
{
    class Program
    {
        public string role = "customer";
        static void Main(string[] args)
        {
            // until the page screen is implemented
            //Console.BackgroundColor = ConsoleColor.Red;
            //Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Do you have a account? Y/N");
            string response = Console.ReadLine();
            Authenticate authenticate = new Authenticate();
            if (response.ToUpper() == "Y") {
                authenticate.Login(); 
            } else {
                authenticate.Register(); 
            }

            Console.WriteLine($"" +
                $"              _____________   \n" +
                $"              Opening Hours   \n"+
                $"                              \n" +
                $"Monday                12:00 - 16:00 and 18:00 - 00:00\n" +
                $"Tuesday               12:00 - 16:00 and 18:00 - 00:00\n" +
                $"Wednesday             12:00 - 16:00 and 18:00 - 00:00\n" +
                $"Thursday              12:00 - 16:00 and 18:00 - 00:00\n" +
                $"Friday                12:00 - 16:00 and 18:00 - 00:00\n" +
                $"Saturday              12:00 - 16:00 and 18:00 - 00:00\n" +
                $"Sunday                12:00 - 16:00 and 18:00 - 00:00\n" +              
                $"\n\n" +
                $"                  ________    \n" +
                $"                  Location    \n" +
                $"                              \n" +
                $"Address                     : Wijnhaven 107\n" +
                $"Zip Code                    : 3011 WN, Rotterdam\n" +
                $"Country                     : The Netherlands\n" +
                $"\n\n" +
                $"             _________________\n" +
                $"             Travel Directions\n" +
                $"                              \n" +
                $"The location is near subway station Blaak.    \n" +
                $"If you get off at station Blaak and walk      \n" +
                $"towards station Beurs via Hoogstraat. You     \n" +
                $"will pass by McDonald at some point. At the   \n" +
                $"McDonald to the left. Continue straight on    \n" +
                $"until you see a bridge and turn right before  \n" +
                $"the bridge.                                   \n" +
                $"\n\n" +
                $"                  _______ \n" +
                $"                  Contact \n" +
                $"                          \n" +
                $"Telephone Number          : 010 0000000               \n" +
                $"Number For Foreigners     : +3100 0 0000000           \n" +
                $"E-mail                    : hogeschool@hr.nl          \n" +
                $"Facebook                  : https://www.facebook.com  \n" +
                $"Instagram                 : https://www.instagram.com \n" +
                $"Twitter                   : https://twitter.com/      \n" +
                $"Pinterest                 : https://www.pinterest.com \n" +
                $"");


        }
    }
}
