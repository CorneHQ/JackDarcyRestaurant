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
            User user = DB.LoadUser().SingleOrDefault(e => e.Id == Manager.User.Id); // Since the cart is an array inside the User array, loading the user is enough to get the content of the cart
            ConsoleTable products = new ConsoleTable("Name", "Price", "Quantity");
            double Total = 0.0;
            int TotalItems = 0;
            if(user.Cart.Count == 0) // Checks if the cart is empty
            {
                Console.WriteLine("Your cart is empty");
                Program.ToMainMenu();
            }
            foreach (MenuItem item in user.Cart)
            {
                products.AddRow(item.Name, String.Format("{0:N2} Euro", item.Price), item.Quantity);
                Total += item.Price * item.Quantity; // Total is used to see the total price of all the items combined
                TotalItems += item.Quantity;
            }
            products.AddRow("Total:", String.Format("{0:N2} Euro", Total), TotalItems);
            products.Write(Format.Alternative);
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
