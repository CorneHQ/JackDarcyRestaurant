using System;
using System.Collections.Generic;
using System.Text;

namespace Jack_Darcy_Restaurant.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public int Menu_Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool Vegan { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
    }
}
