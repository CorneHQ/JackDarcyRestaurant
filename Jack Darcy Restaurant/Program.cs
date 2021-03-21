using System;

namespace Jack_Darcy_Restaurant
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Jack Darcy's! Type one of the following options to go to the desired page: \n" +
                $"About \n" +
                $"Login \n" +
                $"Register");
            Handler(0);
        }

        static void Handler(int page)
        {
            string input = Console.ReadLine();
            if (page == 0)
            {
                if (input.ToLower() == "about")
                {
                    Console.WriteLine($"" +
                $"              _____________   \n" +
                $"              Opening Hours   \n" +
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
                $"\n  > To return to the main menu, press enter");
                    string Back = Console.ReadLine();
                    if (Back == "" ^ Back != "")
                    {
                        Console.Clear();
                        Console.WriteLine("Type one of the following options and press enter to go to the desired page: \n" +
                            $"About \n" +
                            $"Login \n" +
                            $"Register");
                        Handler(0);
                    }
                }
                else if (input.ToLower() == "login")
                {
                    Login();
                }
                else if (input.ToLower() == "register")
                {
                    Register();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine(" > '" + input + "' wasn't recognised as a command, please try again\n" +
                        $"Type one of the following options and press enter to go to the desired page: \n" +
                        $"About \n" +
                        $"Login \n" +
                        $"Register");
                    Handler(0);
                }
            }
        }

        static void Login()
        {
            Console.WriteLine("LOGIN \r\nUsername: ");
            if (Console.ReadLine() != String.Empty)
            { 
                string username = Console.ReadLine();

            }
            else
            {
                Handler(1);
            }
            Console.WriteLine("Password: ");
            if (Console.ReadLine() != "")
            {

            }
        }
        static void Register()
        {
            Console.WriteLine("REGISTER \r\nUsername: ");
            if (Console.ReadLine() != "")
            {
                string username = Console.ReadLine();
            }
            else
            {
                Handler(1);
            }
            Console.WriteLine("Password: ");
            if (Console.ReadLine() != "")
            {

            }
        }
    }
}
