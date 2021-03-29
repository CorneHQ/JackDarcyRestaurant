using System;
using System.Collections.Generic;
using System.Text;
using JsonFlatFileDataStore;
using System.Linq;

namespace Jack_Darcy_Restaurant.Models
{
    public class DB
    {
        public static User[] LoadUser()
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<User>();

            User[] users = collection.AsQueryable().ToArray<User>();
            return users;
        }

        public static bool SetUser(User newUser)
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<User>();
            int i = collection.AsQueryable().Where(e => e.Email == newUser.Email).Count();
            bool result = collection.AsQueryable().Where(e => e.Email == newUser.Email).Count() == 1;
            if (result) return false;
            collection.InsertOne(newUser);
            return true;
        }

        public static Role GetRole(int ID)
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<Role>();
            Role r = (collection.AsQueryable().Where(e => e.Id == ID).ToArray<Role>())[0];
            return r;
        }

        public static void RoleInit()
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<Role>();
            Role[] roles =
            {

                new Role {
                    Id = 0,
                    Name = "Customer",
                    Add_Reservation = true,
                    See_Reservation = true,
                    See_All_Reservations = false,
                    Cancel_Reservation = true,
                    See_Menu = true,
                    Add_Menu = false,
                    Edit_Menu = false,
                    Assign_Roles = false,
                    Add_Reservation_Customer = false,// dont know what this does
                    Edit_Roles = false,
                    See_Transactions = false,
                    See_Takeaway_Orders = false,
                    Access_Shopping_Card = true // dont know what this does
                },
                new Role {
                    Id = 1,
                    Name = "Owner",
                    Add_Reservation = true,
                    See_Reservation = true,
                    See_All_Reservations = true,
                    Cancel_Reservation = false,
                    See_Menu = true,
                    Add_Menu = false,
                    Edit_Menu = false,
                    Assign_Roles = true,
                    Add_Reservation_Customer = false,// dont know what this does
                    Edit_Roles = true,
                    See_Transactions = true,
                    See_Takeaway_Orders = false,
                    Access_Shopping_Card = true // dont know what this does
                },
                new Role {
                    Id = 2,
                    Name = "Chef",
                    Add_Reservation = false,
                    See_Reservation = false,
                    See_All_Reservations = true,
                    Cancel_Reservation = false,
                    See_Menu = true,
                    Add_Menu = true,
                    Edit_Menu = true,
                    Assign_Roles = false,
                    Add_Reservation_Customer = false,// dont know what this does
                    Edit_Roles = false,
                    See_Transactions = false,
                    See_Takeaway_Orders = true,
                    Access_Shopping_Card = true // dont know what this does
                },
                new Role {
                    Id = 3,
                    Name = "manager",
                    Add_Reservation = false,
                    See_Reservation = false,
                    See_All_Reservations = true,
                    Cancel_Reservation = false,
                    See_Menu = true,
                    Add_Menu = false,
                    Edit_Menu = false,
                    Assign_Roles = false,
                    Add_Reservation_Customer = false,// dont know what this does
                    Edit_Roles = false,
                    See_Transactions = true,
                    See_Takeaway_Orders = true,
                    Access_Shopping_Card = true // dont know what this does
                }
            };

            collection.InsertMany(roles);

        }
    }
}
