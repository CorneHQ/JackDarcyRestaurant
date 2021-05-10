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
        private static string filterCategory = "all";
        private static string filterVegan = "all";
        private static string filterName = "all";



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

            if(filterVegan == "yes")
            {
                collection = collection.Where(m => m.Vegan == true);
            }

            if(filterCategory != "all")
            {
                collection = collection.Where(m => m.Category.Contains(filterCategory));
            }

            if(filterName != "all")
            {
                collection = collection.Where(m => m.Name.Contains(filterName));
            }
          
            return collection;
        }

        private static void ShowMenu(int menuId)
        {
            Console.Clear();
            if (showError != "")
            {
                Console.WriteLine("\n");
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(showError);
                Console.ResetColor();
                Console.WriteLine("\n");
                showError = "";
            }

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
            Console.WriteLine("Please press 'Backspace' to customize the filters or press 'Enter' to go back to the main menu or press 'Esc' to go back to the menu list");
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    PageHandler.switchPage(-1);
                    break;
                } else if (Console.ReadKey().Key == ConsoleKey.Escape)
                {
                    PageHandler.switchPage(3);
                    break;
                } else if(Console.ReadKey().Key == ConsoleKey.Backspace)
                {
                    Console.WriteLine("\nChoose your filter here:");
                    Console.WriteLine($"[0] {(filterVegan == "all" ? "Enable" : "Disable")} Vegan ({(filterVegan == "all" ? "All" : "Only Vegan")})");
                    Console.WriteLine($"[1] Search For Category ({(filterCategory == "all" ? "All" : filterCategory)})");
                    Console.WriteLine($"[2] Search On Name ({(filterName == "all" ? "All" : filterName)})");
                    string Output = Console.ReadLine();
                    int Choice;
                    bool success = Int32.TryParse(Output, out Choice);
                    if (success && Choice == 0 || Choice == 1 || Choice == 2)
                    {
                        if (Choice == 0)
                        {
                            switch (filterVegan)
                            {
                                case "all":
                                    filterVegan = "yes";
                                    break;
                                case "yes":
                                    filterVegan = "all";
                                    break;
                            }
                            ShowMenu(menuId);
                            break;
                        }
                        else if (Choice == 1)
                        {
                            Console.WriteLine("Please type in a filter for the category (Type 'all' to show all the categories):");
                            string Category = Console.ReadLine();
                            filterCategory = Category;
                            ShowMenu(menuId);
                            break;
                        }
                        else if (Choice == 2)
                        {
                            Console.WriteLine("Please type in a filter for the name of the product (Type 'all' to show all the products):");
                            string Name = Console.ReadLine();
                            filterName = Name;
                            ShowMenu(menuId);
                            break;
                        }
                    } else
                    {
                        showError = "Filter option doesn\'t exist!";
                        ShowMenu(menuId);
                    }
                }
            }
        }

        static void AddMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome in the addMenu Fearture");
            

        }
        
        public static void PageHandlerMenu()
        {
            Console.Clear();
            string s = $" choose your feature \n ";
            s += $"[0] Go back \n";
            s += $"[1] Showing the menu \n";
            s += $"[2] Add menu \n";
            s += $"[3] Remove Menu \n ";
            s += $"[4] Add Product to existing Menu \n";
            s += $"[5] Remove product from existing Menu \n "; 



            Console.WriteLine(s);
            string input = Console.ReadLine();// neemt input
            bool test = int.TryParse(input, out int page); //kijken of string kan omzetten naar nummer
            if (test)
            {
                if (page == 0)
                {
                    Console.Clear(); // just in case moet checken of dit nodig is
                    MainMenu.Show();
                }
                else if (page == 1)
                {
                    Menus.ShowMenus();
                }
                else if (page == 2)
                {
                    Menus.AddMenu();
                }
                else if (page == 3)
                {
                    ;
                }
                else if (page == 4)
                {
                    ;
                }
                else
                {
                    Console.WriteLine("" +
                        "Sorry this is not a page \n" +
                        "   Please Try Again");
                    Menus.PageHandlerMenu();
                }               
            }
            else
            {
                Console.WriteLine("" +
                    "That's not a valid number for a page \n" +
                    "           Please Try again");
                Menus.PageHandlerMenu();
            }
        }
    }
}
