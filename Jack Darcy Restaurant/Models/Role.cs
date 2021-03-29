using System;
using System.Collections.Generic;
using System.Text;

namespace Jack_Darcy_Restaurant.Models
{
    class Role
    {
        public int Id { get; set; }
        public bool Add_Reservation { get; set; }
        public bool See_Reservation { get; set; }
        public bool See_All_Reservations { get; set; }
        public bool Cancel_Reservation { get; set; }
        public bool See_Menu { get; set; }
        public bool Add_Menu { get; set; }
        public bool Edit_Menu { get; set; }
        public bool Assign_Roles { get; set; }
        public bool Add_Reservation_Customer { get; set; }
        public bool Edit_Roles { get; set; }
        public bool See_Transactions { get; set; }
        public bool See_Takeaway_Orders { get; set; }
        public bool Access_Shopping_Card { get; set; }
    }
}
