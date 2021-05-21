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
            Cart[] carts = DB.LoadCart().Where(e => e.UserID == Manager.User.Id).ToArray();
            ConsoleTable products = new ConsoleTable("Name", "Price");
            double total = 0;
            foreach (Cart item in carts)
            {
                products.AddRow(item.Name, item.Price);
                total += item.Price;
            }
            products.AddRow("\nTotal: ", total);
            products.Write(Format.Minimal);
            Console.WriteLine("To checkout write 'Pay', to go to the main menu write anything else:");
            string a = Console.ReadLine();
            if (a.ToLower() == "pay")
            {
                Payment.Pay(total);
            }
            else
            {
                Console.Clear();
                MainMenu.Show();
            }
        }
    }
}
