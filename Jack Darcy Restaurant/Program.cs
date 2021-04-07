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
            Init();
            PageHandler.switchPage(-1);
        }

        static void Init()
        {
            DB.RoleInit();
            var store = new DataStore("data.json");
            var collection = store.GetCollection<User>();
            if(collection.Count == 0)
            {
                User user = new User(0, "owner", "secret", "owner@jackdarcy.com", 1);
                DB.SetUser(user);
            }
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