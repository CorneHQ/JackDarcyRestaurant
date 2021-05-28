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
            User user = DB.LoadUser().SingleOrDefault(e => e.Id == Manager.User.Id);
            ConsoleTable products = new ConsoleTable("Name", "Price", "Quantity");
            double Total = 0.0;
            int TotalItems = 0;
            if(user.Cart.Count == 0)
            {
                Console.WriteLine("Your cart is empty");
                Program.ToMainMenu();
            }
            foreach (MenuItem item in user.Cart)
            {
                products.AddRow(item.Name, item.Price, item.Quantity);
                Total += item.Price * item.Quantity;
                TotalItems += item.Quantity;
            }
            products.AddRow("\nTotal: ", Total, TotalItems);
            products.Write(Format.Minimal);
            Console.WriteLine("To checkout write 'Pay', to go to the main menu write anything else:");
            string a = Console.ReadLine();
            if (a.ToLower() == "pay")
            {
                Payment.Pay(Total);
            }
            else
            {
                Console.Clear();
                MainMenu.Show();
            }
        }
    }
}
