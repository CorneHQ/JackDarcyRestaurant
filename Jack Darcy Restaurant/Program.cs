using System;
using Jack_Darcy_Restaurant.Models;
using Jack_Darcy_Restaurant.Utils;
using JsonFlatFileDataStore;

namespace Jack_Darcy_Restaurant
{
    class Program
    {
        static void Main(string[] args)
        {
            DB.RoleInit();
            DB.UserInit();
            DB.MenuInit();
            DB.MenuItemInit();
            PageHandler.switchPage(-1);
        }

        public static void ToMainMenu()
        {
            Console.WriteLine("Press enter to go main menu");
            while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
            PageHandler.switchPage(-1);
        }
    }

    
}

//namespace Jack_Darcy_Restaurant.Basic
//{
//    static void ToMainMenu()
//    {
//        Console.WriteLine("Press enter to go main menu");
//        while (Console.ReadKey(true).Key != ConsoleKey.Enter) { }
//        PageHandler.switchPage(-1);
//    }
//}