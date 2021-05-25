using System;
using System.Collections.Generic;
using System.Text;

namespace Jack_Darcy_Restaurant.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public  List<MenuItem> dishes { get; set; }

        public Cart(int uID, string iName, double iPrice)
        {
            UserID = uID;
            Name = iName;
            Price = iPrice;
            dishes = new List<MenuItem>();
        }
    }
}
