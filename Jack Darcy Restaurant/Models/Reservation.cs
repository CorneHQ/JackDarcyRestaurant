using System;
using System.Collections.Generic;
using System.Text;

namespace Jack_Darcy_Restaurant.Models
{
    public class Reservation
    {

        public int Id { get; set; }
        public int User_Id { get; set; }
        public int Amount_People { get; set; }
        public DateTime Reservation_Date { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan Till { get; set; }
        public string Code { get; set; }
        
    }
}
