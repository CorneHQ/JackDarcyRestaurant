using System;
using System.Collections.Generic;
using System.Text;
using ConsoleTables;
using Jack_Darcy_Restaurant.Models;
using System.Linq;

namespace Jack_Darcy_Restaurant.Pages
{
    class ViewCart
    {
        public static void CurrentCart()
        {
            Cart cart = DB.LoadCart().SingleOrDefault(e => e.UserID == Manager.User.Id);
            ConsoleTable products = new ConsoleTable("Name", "Price");
            if(cart == null)
            {
                Console.WriteLine("no Product in cart");
                Program.ToMainMenu();
            }
            foreach (MenuItem item in cart.dishes)
            {
                products.AddRow(item.Name, item.Price);
            }
            products.AddRow("\nTotal: ", cart.Price);
            products.Write(Format.Minimal);
            Console.WriteLine("To checkout write 'Pay', to go to the main menu write anything else:");
            string a = Console.ReadLine();
            if (a.ToLower() == "pay")
            {
                Payment.Pay(cart.Price);
            }
            else
            {
                Console.Clear();
                MainMenu.Show();
            }
        }
    }
}
