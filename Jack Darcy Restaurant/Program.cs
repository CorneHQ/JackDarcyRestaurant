using System;
using Jack_Darcy_Restaurant.Model;


namespace Jack_Darcy_Restaurant
{
    class Program
    {
        public string role = "customer";
        static void Main(string[] args)
        {
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

        }
    }
}
