using System;
using System.Collections.Generic;
using System.Text;

namespace Jack_Darcy_Restaurant.Models
{
    class Manager//keeping up which user is logged in with which Role
    {
        public static Role Role { get; set; }
        public static User User { get; set; }
    }
}
