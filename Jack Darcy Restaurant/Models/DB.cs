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
            Role r = collection.AsQueryable().FirstOrDefault(e => e.Id == ID);
            return r;
        }

        public static void RoleInit()
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<Role>();
            collection.DeleteMany(e => e.Id > -1);
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

        public static void UserInit()
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<User>();
            if (collection.Count == 0)
            {
                User user = new User(0, "owner", "secret", "owner@jackdarcy.com", 1);
                SetUser(user);
            }
        }
        public static void MenuInit()
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<Menu>();
            collection.DeleteMany(e => e.Id > -1);
            Menu[] menu =
            {
                new Menu
                {
                    Id = 0,
                    Name = "namestring1"
                },
                new Menu
                {
                    Id = 1,
                    Name = "Menu name 2"
                },
                new Menu
                {
                    Id = 1,
                    Name = "Menu name 3"
                },
                new Menu
                {
                    Id = 1,
                    Name = "Menu name 4"
                },
                new Menu
                {
                    Id = 1,
                    Name = "Menu name 5"
                }
            };

            collection.InsertMany(menu);

           
        }
    }
}
