﻿using System;
using System.Collections.Generic;
using System.Globalization;
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



        public static void ShowMenus()// showing menu 
        {
            Permisions.CheckPermisions(4);
            IEnumerable<Menu> menus = GetMenus();

            if(showError != "")// if there is an error show it 
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(showError);
                Console.ResetColor();
                showError = "";
            }

            Console.WriteLine("Please choose a menu:");//showing all menus 
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
                Console.Clear();
                showError = "Menu does not exist. Please try it again.\n";
                ShowMenus();
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
                collection = collection.Where(m => m.Category.ToLower().Contains(filterCategory.ToLower()));
            }

            if(filterName != "all")
            {
                collection = collection.Where(m => m.Name.ToLower().Contains(filterName.ToLower()));
            }
          
            return collection;
        }

        private static void ShowMenu(int menuId)
        {
            Console.Clear();
            if (showError != "")
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine(showError);
                Console.ResetColor();
                showError = "";
            }

            IEnumerable<MenuItem> menuItems = GetMenuItems(menuId);
            ConsoleTable menuTable = new ConsoleTable("Id", "Name", "Price", "Vegan", "Category");
            if (!menuItems.Any())
            {
                menuTable.AddRow("There are no items added to this menu.", "", "", "", "");
            } else
            {
                foreach (MenuItem menuItem in menuItems)
                {
                    menuTable.AddRow(menuItem.Id, menuItem.Name, String.Format("{0:N2} Euro", menuItem.Price), menuItem.Vegan ? "Yes" : "No", menuItem.Category);
                }
            }
            menuTable.Write(Format.Alternative);
            Console.WriteLine("Please press 'Backspace' to customize the filters");
            Console.WriteLine("Please press 'Enter' to go back to the main menu");
            Console.WriteLine("Please press 'Esc' to go back to the menu list");
            Console.WriteLine("Please press 'Tab' to add a dish to your shopping cart");
            while (true)
            {
                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.Enter)
                {
                    PageHandler.switchPage(-1);
                    break;
                } else if (key == ConsoleKey.Escape)
                {
                    PageHandler.switchPage(3);
                    break;
                } else if (key == ConsoleKey.Backspace)
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
                        showError = "Filter option doesn't exist!\n";
                        ShowMenu(menuId);
                    }
                } else if (key == ConsoleKey.Tab)
                {
                    Order(menuId, menuItems); // The code to order is in separate function because otherwise you couldn't press backspace and shift
                    break;
                }
                //string itemName = Console.ReadLine();
                //double itemPrice = 2.0;

                //Cart AddToCart = new Cart(Manager.User.Id, itemName, itemPrice);
                //if (DB.UpdateCart(AddToCart))
                //{
                //    Console.WriteLine("success");
                //}
                //else
                //{
                //    Console.WriteLine("failed");
                //}
            }
        }

        private static void Order(int menuId, IEnumerable<MenuItem> menuItems)
        {
            Console.WriteLine("Please enter the ID of the product");
            string Output = Console.ReadLine();
            int dishChoose;
            if (!Int32.TryParse(Output, out dishChoose))
            {
                showError = "Invalid product ID\n";
                ShowMenu(menuId);
            }
            MenuItem m = menuItems.AsQueryable().FirstOrDefault(m => m.Id == dishChoose || m.Name == Output);
            if (m != null)
            {
                Console.WriteLine("Please enter the quantity: ");
                string q = Console.ReadLine();
                if (!Int32.TryParse(q, out int Quantity) || Quantity < 1)
                {
                    showError = "Invalid quantity\n";
                    ShowMenu(menuId);
                }
                m.Quantity = Quantity;
                DB.UpdateCart(m);
                Console.WriteLine("Product successfully added to cart");
                Program.ToMainMenu();
            }
            else
            {
                showError = "Invalid product ID\n";
                ShowMenu(menuId);
            }
        }

        public static void AddMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Add Menu Feature\n");
            Console.WriteLine("" +
                "[0] to go back \n" +
                "[1] To Add Menu \n");
            string input = Console.ReadLine();
            bool TestingInput = int.TryParse(input, out int IntInput);
            if (TestingInput)// 2 if else statements for testing input with little bit of recursion
            {
                if (IntInput == 0)
                {
                    Console.Clear();
                    Menus.PageHandlerMenu();
                }
                else if (IntInput == 1)// take name and create a menu with that name
                {
                    Console.Clear();
                    Console.WriteLine(" Welcome in the addMenu Fearture");
                    Console.WriteLine(" Please Enter Menu Name");
                    string name = Console.ReadLine();
                    while (name == "")
                    {
                        Console.Clear();
                        Console.WriteLine("" +
                            "\n\n       Name cannot be empty\n\n");
                        Console.WriteLine(" Please Enter Menu Name");
                        name = Console.ReadLine();
                    }
                    var store = new DataStore("data.json");
                    var collection = store.GetCollection<Menu>();
                    int total = collection.Count;
                    Menu[] menu =
                       {
                        new Menu 
                            {
                        Id = total,
                        Name = name
                            }
                        };
                    Console.Clear();
                    Console.WriteLine(" You have Added Menu\n\n " +
                        $"Id = {total} \n " +
                        $"Name = {name} \n ");
                    collection.InsertMany(menu);
                    Console.ReadLine();
                    Menus.PageHandlerMenu();


                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Not a valid number Please Try Again");
                    Console.ReadLine();
                    Menus.AddMenu();
                }

            }
            else if (!TestingInput)
            {
                Console.Clear();
                Console.WriteLine("Wrong Input Please Try Again");
                Console.ReadLine();
                Menus.AddMenu();
            }
            else
            {
                Console.Clear();
                Console.WriteLine(" ERROR : Wrong input\n Returning to previous screen\n " +
                    "press Enter");
                Console.ReadLine();
                Menus.PageHandlerMenu();
            }



            
        }

        public static void RemoveMenu()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Remove Menu Feature\n");
            Console.WriteLine("" +
                "[0] to go back \n" +
                "[1] To Delete Menu \n");
            string input = Console.ReadLine();
            bool TestingInput = int.TryParse(input, out int IntInput);
            if (TestingInput)// a big if else statement for checking input
            {
                if (IntInput == 0)
                {
                    Console.Clear();
                    Menus.PageHandlerMenu();
                }
                else if (IntInput == 1)
                {
                    Console.Clear();

                    var store = new DataStore("data.json");// get all the menus and print them out to show it 
                    var collection = store.GetCollection<Menu>();
                    foreach (var x in collection.AsQueryable())
                    {
                        Console.WriteLine($"Id = {x.Id}\n" +
                            $"Name = {x.Name}\n\n");
                    }
                    Console.WriteLine("Please enter Id of Menu you wanne delete");
                    string input1 = Console.ReadLine();
                    bool testingInput1 = int.TryParse(input1, out int intinput1);
                    if (testingInput1)//checking input 
                    {
                        // get all the ID in a array
                        int[] allID = new int[collection.Count];
                        int ii = 0;
                        foreach (var z in collection.AsQueryable())
                        {
                            allID[ii] = z.Id;
                            ii++;
                        }
                        
                        while (!allID.Contains(intinput1))
                        {
                            Console.Clear();
                            Console.WriteLine("" +
                                "\n\n       ID does not exist\n\n");
                            Console.WriteLine("Please Enter ID");
                            input1 = Console.ReadLine();
                            testingInput1 = int.TryParse(input1, out intinput1);
                        }
                        Console.Clear();
                        collection.DeleteMany(z => z.Id == intinput1);

                        Console.WriteLine("showing new list of Menu");
                        foreach (var x in collection.AsQueryable())
                        {
                            Console.WriteLine($"Id = {x.Id}\n" +
                                $"Name = {x.Name}\n\n");
                        }
                        Console.ReadLine();
                        Menus.PageHandlerMenu();
                    }
                    else if (!testingInput1)
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong Input Please Try Again");
                        Console.ReadLine();
                        Menus.RemoveMenu();
                    }
                    else 
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong Input Please Try Again");
                        Console.ReadLine();
                        Menus.RemoveMenu();
                    }
                    


                }
                else 
                {
                    Console.Clear();
                    Console.WriteLine("Not a valid number Please Try Again");
                    Console.ReadLine();
                    Menus.RemoveMenu();
                }

            }
            else if (!TestingInput)
            {
                Console.Clear();
                Console.WriteLine("Wrong Input Please Try Again");
                Console.ReadLine();
                Menus.RemoveMenu();
            }
            else
            {
                Console.Clear();
                Console.WriteLine(" ERROR : Wrong input\n Returning to previous screen\n " +
                    "press Enter");
                Console.ReadLine();
                Menus.PageHandlerMenu();
            }
            
        }
        public static void AddProduct()// adding a product 
        {
            Console.Clear();
            Console.WriteLine("Welcome to Add Product Feature\n");
            Console.WriteLine("" +
                "[0] to go back \n" +
                "[1] To Add Product \n");
            string input = Console.ReadLine();
            bool TestingInput = int.TryParse(input, out int IntInput);
            if (TestingInput)// big if else statement checking input with a bit recursion
            {
                if (IntInput == 0)
                {
                    Console.Clear();
                    Menus.PageHandlerMenu();
                }
                else if (IntInput == 1)
                {
                    Console.Clear();// getting inputs 
                    Console.WriteLine(" Welcome in the add Product Fearture");
                    Console.WriteLine(" Please Enter Product Name");
                    string name = Console.ReadLine();
                    while (name == "")// check for name 
                    {
                        Console.Clear();
                        Console.WriteLine("" +
                            "\n\n       Name cannot be empty\n\n");
                        Console.WriteLine(" Please Enter Product Name");
                        name = Console.ReadLine();
                    }

                    Console.WriteLine(" Please Enter Price (example = 10.10)");
                    string inputPrice = Console.ReadLine();

                    double Price;
                    while (!Double.TryParse(inputPrice.Replace(',','.'), NumberStyles.Number, CultureInfo.InvariantCulture, out Price))// check for price
                    {
                        Console.WriteLine("Wrong input Please see example (15.53)");
                        inputPrice = Console.ReadLine();
                    }

                    Console.WriteLine(" Please Enter if the dish is Vegan or not (example = true/false)");
                    string inputVegan = Console.ReadLine();

                    bool Vegan;
                    while (!bool.TryParse(inputVegan, out Vegan))// check for vegan
                    {
                        Console.WriteLine("Wrong input Please see example (true/false)");
                        inputVegan = Console.ReadLine();
                    }

                    Console.WriteLine(" Please Enter Category");//check for category 
                    string Category = Console.ReadLine();
                    while (Category == "")
                    {
                        Console.Clear();
                        Console.WriteLine("" +
                            "\n\n       Name cannot be empty\n\n");
                        Console.WriteLine(" Please Enter Category Name");
                        Category = Console.ReadLine();
                    }


                    // Adding to menu part
                    Console.Clear();
                    DataStore storetemp = new DataStore("data.json");// het kiezen welke menu het er bij hoort
                    var collectiontemp = storetemp.GetCollection<Menu>();
                    foreach (var x in collectiontemp.AsQueryable())//showing all menu choices
                    {
                        Console.WriteLine($"Id = {x.Id}\n" +
                            $"Name = {x.Name}\n");
                    }
                    //input
                    Console.WriteLine(" Please Enter Id of Menu you want to append to");
                    string inputMenuID = Console.ReadLine();
                    //checking if it is int 
                    bool testinginputMenuID = int.TryParse(inputMenuID, out int MenuID);
                    while (!testinginputMenuID)
                    {
                        Console.Clear();
                        foreach (var x in collectiontemp.AsQueryable())//showing all menu choices
                        {
                            Console.WriteLine($"Id = {x.Id}\n" +
                                $"Name = {x.Name}\n");
                        }
                        Console.WriteLine("\nThat is not a number please try again\n");
                        Console.WriteLine(" Please Enter Id of Menu you want to append to");
                        inputMenuID = Console.ReadLine();
                        testinginputMenuID = int.TryParse(inputMenuID, out MenuID);
                    }
                    //checking if menu exist
                    //create range to check 
                    int[] array = new int[collectiontemp.Count];
                    int tempi = 0;
                    foreach (var x in collectiontemp.AsQueryable())
                    {
                        array[tempi] = x.Id;
                        tempi++;
                    }
                    // check 
                    bool checking = array.Contains(MenuID);
                    while (!checking)
                    {
                        Console.Clear();
                        foreach (var x in collectiontemp.AsQueryable())//showing all menu choices
                        {
                            Console.WriteLine($"Id = {x.Id}\n" +
                                $"Name = {x.Name}\n");
                        }
                        Console.WriteLine("\nThat Menu does not exist\n");
                        Console.WriteLine(" Please Enter Id of Menu you want to append to");
                        inputMenuID = Console.ReadLine();
                        checking = array.Contains(int.TryParse(inputMenuID, out MenuID) ? MenuID : 1000 );
                    }
                    //convertion for input
                    var store = new DataStore("data.json");
                    var collection = store.GetCollection<MenuItem>();
                    int total = collection.Count;
                    MenuItem[] menuitem =
                       {
                        new MenuItem
                            {
                        Id = total,
                        Menu_Id = MenuID,
                        Name = name,
                        Price = Price,
                        Vegan = Vegan, 
                        Category = Category
                            }
                        };
                    Console.Clear();
                    Console.WriteLine(" You have Added Menu\n\n " +
                        $"Id = {total},\n " +
                        $"Menu_Id = {MenuID},\n " +
                        $"Name = {name},\n " +
                        $"Price = {String.Format("{0:N2} Euro", Price)},\n " +
                        $"Vegan = {Vegan},\n " +
                        $"Category = {Category} \n "
                        );
                    collection.InsertMany(menuitem);
                    Console.ReadLine();
                    Menus.PageHandlerMenu();


                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Not a valid number Please Try Again");
                    Console.ReadLine();
                    Menus.AddProduct();
                }

            }
            else if (!TestingInput)
            {
                Console.Clear();
                Console.WriteLine("Wrong Input Please Try Again");
                Console.ReadLine();
                Menus.AddProduct();
            }
            else
            {
                Console.Clear();
                Console.WriteLine(" ERROR : Wrong input\n Returning to previous screen\n " +
                    "press Enter");
                Console.ReadLine();
                Menus.PageHandlerMenu();
            }


        }
        public static void RemoveProduct()
        {
            Console.Clear();
            Console.WriteLine("Welcome to Remove Product Feature\n");
            Console.WriteLine("" +
                "[0] to go back \n" +
                "[1] To Delete Product \n");
            string input = Console.ReadLine();
            bool TestingInput = int.TryParse(input, out int IntInput);
            if (TestingInput)
            {
                if (IntInput == 0)
                {
                    Console.Clear();
                    Menus.PageHandlerMenu();
                }
                else if (IntInput == 1)
                {
                    Console.Clear();

                    var store = new DataStore("data.json");
                    var collection = store.GetCollection<MenuItem>();
     
                    foreach (var x in collection.AsQueryable())// showing all products
                    {
                        Console.WriteLine($"Id = {x.Id}\n" +
                            $"Name = {x.Name}\n" +
                            $"Price = {String.Format("{0:N2} Euro", x.Price)}\n\n");
                    }


                    Console.WriteLine("Please enter ID of the product you want to delete");
                    string input1 = Console.ReadLine();//input
                    //check input for number

                    bool testinginputID = int.TryParse(input1, out int ID);
                    while (!testinginputID)
                    {
                        Console.Clear();
                        foreach (var x in collection.AsQueryable())//showing all menu choices
                        {
                            Console.WriteLine($"Id = {x.Id}\n" +
                                $"Name = {x.Name}\n");
                        }
                        Console.WriteLine("\nThat is not a number, please try again\n");
                        Console.WriteLine(" Please Enter Id of Menu you want to append to");
                        input1 = Console.ReadLine();
                        testinginputID = int.TryParse(input1, out ID);
                    }
                    //checking if menu exist
                    //create range to check 
                    int[] array = new int[collection.Count];
                    int tempi = 0;
                    foreach (var x in collection.AsQueryable())
                    {
                        array[tempi] = x.Id;
                        tempi++;
                    }
                    // check 
                    bool checking = array.Contains(ID);
                    while (!checking)
                    {
                        Console.Clear();
                        foreach (var x in collection.AsQueryable())//showing all menu choices
                        {
                            Console.WriteLine($"Id = {x.Id}\n" +
                                $"Name = {x.Name}\n"+
                                $"Price = {x.Price}\n");
                        }
                        
                        Console.WriteLine("\nThat Menu does not exist\n");
                        Console.WriteLine(" Please Enter Id of Menu you want to append to");
                        input1 = Console.ReadLine();
                        checking = array.Contains(int.TryParse(input1, out ID) ? ID : 1000);
                    }
                    Console.Clear();
                    collection.DeleteMany(z => z.Id == ID);
                    Console.WriteLine("Showing new list of Product");
                    foreach (var x in collection.AsQueryable())
                    {
                        Console.WriteLine($"Id = {x.Id}\n" +
                            $"Name = {x.Name}\n" +
                            $"Price = {String.Format("{0:N2} Euro", x.Price)}\n");
                    }
                    Console.ReadLine();
                    Menus.PageHandlerMenu();
                    //check if number is in products-----------
                    /*
                    bool testingInput1 = int.TryParse(input1, out int intinput1);
                    if (testingInput1)
                    {
                        Console.Clear();
                        collection.DeleteMany(z => z.Id == intinput1);
                        Console.WriteLine("showing new list of Product");
                        foreach (var x in collection.AsQueryable())
                        {
                            Console.WriteLine($"Id = {x.Id}\n" +
                                $"Name = {x.Name}\n" +
                                $"Price = {x.Price}\n\n");
                        }
                        Console.ReadLine();
                        Menus.PageHandlerMenu();
                    }
                    else if (!testingInput1)
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong Input Please Try Again");
                        Console.ReadLine();
                        Menus.RemoveMenu();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Wrong Input Please Try Again");
                        Console.ReadLine();
                        Menus.RemoveMenu();
                    }
                    */


                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Not a valid number Please Try Again");
                    Console.ReadLine();
                    Menus.RemoveMenu();
                }

            }
            else if (!TestingInput)
            {
                Console.Clear();
                Console.WriteLine("Wrong Input Please Try Again");
                Console.ReadLine();
                Menus.RemoveMenu();
            }
            else
            {
                Console.Clear();
                Console.WriteLine(" ERROR : Wrong input\n Returning to previous screen\n " +
                    "press Enter");
                Console.ReadLine();
                Menus.PageHandlerMenu();
            }
        }

        public static void TestingSmallerPagehandler()
        {

        }
        public static void PageHandlerMenu()//small pagefhandle within menu 
        {
            Console.Clear();
            string s = $"Choose your feature \n";
            s += $"[0] Go back \n";
            s += $"[1] Show the menu \n";
            if (Manager.Role.Name.ToLower() == "owner" || Manager.Role.Name.ToLower() == "chef")
            {
                s += $"[2] Add menu \n";
                s += $"[3] Remove Menu \n";
                s += $"[4] Add Product to existing Menu \n";
                s += $"[5] Remove product from existing Menu \n";
            }



            Console.WriteLine(s);
            string input = Console.ReadLine();// takes input
            bool test = int.TryParse(input, out int page); 
            if (test)
            {
                if (page == 0)
                {
                    Console.Clear(); // just in case moet checken of dit nodig is
                    MainMenu.Show();
                }
                else if (page == 1)
                {
                    Console.Clear();
                    Menus.ShowMenus();
                }
                else if (page == 2 && Manager.Role.Name.ToLower() == "owner" || Manager.Role.Name.ToLower() == "chef")
                {
                    Console.Clear();
                    if(Manager.Role == null)//check current user and look if he has permission 
                    {
                        Console.WriteLine("" +
                            "You have No Permissions\n" +
                            "Please Login \n" +
                            "Press 'enter' to return");
                        Console.ReadLine();
                        Menus.PageHandlerMenu();
                        
                    }
                    else if (Manager.Role.Add_Menu == true)
                    {
                        Menus.AddMenu();
                    }
                    else
                    {
                        Console.WriteLine("" +
                            "You have No Permissions\n" +
                            "Press 'enter' to return");
                        Console.ReadLine();
                        Menus.PageHandlerMenu();
                    }
                }
                else if (page == 3 && Manager.Role.Name.ToLower() == "owner" || Manager.Role.Name.ToLower() == "chef")
                {
                    Console.Clear();
                    if (Manager.Role == null)//check current user and look if he has permission 
                    {
                        Console.WriteLine("" +
                            "You have No Permissions\n" +
                            "Please Login \n" +
                            "Press 'enter' to return");
                        Console.ReadLine();
                        Menus.PageHandlerMenu();

                    }
                    else if (Manager.Role.Edit_Menu == true)
                    {
                        Menus.RemoveMenu();
                    }
                    else
                    {
                        Console.WriteLine("" +
                            "You have No Permissions\n" +
                            "Press 'enter' to return");
                        Console.ReadLine();
                        Menus.PageHandlerMenu();
                    }
                }
                else if (page == 4 && Manager.Role.Name.ToLower() == "owner" || Manager.Role.Name.ToLower() == "chef")
                {
                    Console.Clear();
                    if (Manager.Role == null)//check current user and look if he has permission 
                    {
                        Console.WriteLine("" +
                            "You have No Permissions\n" +
                            "Please Login \n" +
                            "Press 'enter' to return");
                        Console.ReadLine();
                        Menus.PageHandlerMenu();

                    }
                    else if (Manager.Role.Add_Menu == true)
                    {
                        Menus.AddProduct();
                    }
                    else
                    {
                        Console.WriteLine("" +
                            "You have No Permissions\n" +
                            "Press 'enter' to return");
                        Console.ReadLine();
                        Menus.PageHandlerMenu();
                    }
                }
                else if (page == 5 && Manager.Role.Name.ToLower() == "owner" || Manager.Role.Name.ToLower() == "chef")
                {
                    Console.Clear();
                    if (Manager.Role == null)//check current user and look if he has permission 
                    {
                        Console.WriteLine("" +
                            "You have No Permissions\n" +
                            "Please Login \n" +
                            "Press 'enter' to return");
                        Console.ReadLine();
                        Menus.PageHandlerMenu();

                    }
                    else if (Manager.Role.Edit_Menu == true)
                    {
                        Menus.RemoveProduct();
                    }
                    else
                    {
                        Console.WriteLine("" +
                            "You have No Permissions\n" +
                            "Press 'enter' to return");
                        Console.ReadLine();
                        Menus.PageHandlerMenu();
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("" +
                        "Sorry this is not a page \n" +
                        "   Please Try Again");
                    Console.ReadLine();
                    Console.Clear();
                    Menus.PageHandlerMenu();
                }               
            }
            else
            {
                Console.Clear();
                Console.WriteLine("" +
                    "That's not a valid number for a page \n" +
                    "           Please Try again");
                Console.ReadLine();
                Console.Clear();
                Menus.PageHandlerMenu();
            }
        }
    }
}
