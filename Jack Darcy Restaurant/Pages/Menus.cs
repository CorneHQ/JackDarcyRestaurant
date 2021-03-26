using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleTables;
using Jack_Darcy_Restaurant.Models;
using Jack_Darcy_Restaurant.Utils;
using JsonFlatFileDataStore;

namespace Jack_Darcy_Restaurant.Pages
{
    public class Menus
    {
        private static string showError = "";

        public static void ShowMenus()
        {
            IEnumerable<Menu> menus = GetMenus();

            if(showError != "")
            {
                Console.WriteLine("\n");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(showError);
                Console.ResetColor();
                Console.WriteLine("\n");
                showError = "";
            }

            Console.WriteLine("Please choose a menu:");
            foreach(Menu menu in menus)
            {
                Console.WriteLine($"[{menu.Id}] {menu.Name}");
            }

            string Output = Console.ReadLine();
            int Menu;
            bool success = Int32.TryParse(Output, out Menu);
            IEnumerable<Menu> selectedMenu = menus.Where(m => m.Id == Menu);

            if (success && selectedMenu.Any())
            {
                Console.Clear();
                ShowMenu(Menu);
            } else
            {
                showError = "Menu does not exist. Please try it again.";
                PageHandler.switchPage(3);
            }
        }

        private static IEnumerable<Menu> GetMenus()
        {
            DataStore store = new DataStore("data.json");

            var collection = store.GetCollection<Menu>().AsQueryable();

            return collection;
        }

        private static IEnumerable<MenuItem> GetMenuItems(int menuID)
        {
            DataStore store = new DataStore("data.json");

            var collection = store.GetCollection<MenuItem>().AsQueryable().Where(m => m.Menu_Id == menuID);

            return collection;
        }

        private static void ShowMenu(int menuId)
        {
            IEnumerable<MenuItem> menuItems = GetMenuItems(menuId);
            ConsoleTable menuTable = new ConsoleTable("Name", "Price", "Vegan", "Category");
            if (!menuItems.Any())
            {
                menuTable.AddRow("There are no items added to this menu.", "", "", "");
            } else
            {
                foreach (MenuItem menuItem in menuItems)
                {
                    menuTable.AddRow(menuItem.Name, String.Format("{0:N2} Euro", menuItem.Price), menuItem.Vegan ? "Yes" : "No", menuItem.Category);
                }
            }
            menuTable.Write(Format.Alternative);
            Console.WriteLine("Press 'Enter' to go back to the main menu or press 'Esc' to go back to the menu list");
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    PageHandler.switchPage(-1);
                    break;
                }
                else if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    PageHandler.switchPage(3);
                    break;
                }
            }
        }
    }
}
