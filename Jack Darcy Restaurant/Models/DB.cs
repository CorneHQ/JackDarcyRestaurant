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
            bool result = collection.AsQueryable().Where(e => e.Email == newUser.Email).Count() == 1 || collection.AsQueryable().Where(e => e.Name == newUser.Name).Count() == 1;
            if (result) return false;
            collection.InsertOne(newUser);
            return true;
        }

        public static bool UpdateUser(User user)
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<User>();
            collection.ReplaceOne(user.Id, user);
            User dbUser = collection.AsQueryable().FirstOrDefault(e => e.Id == user.Id);
            if (user.Id == dbUser.Id
                        && user.Role_Id == dbUser.Role_Id
                        && user.Name == dbUser.Name
                        && user.Email == dbUser.Email
                        && user.Password == dbUser.Password) return true;
              
            return false;
        }
        public static bool RemoveUser(User user)
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<User>();
            collection.DeleteOne(a => a.Id == user.Id);
            return true;
        }

        public static bool RemoveCart()
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<User>();
            User c = collection.AsQueryable().FirstOrDefault(e => e.Id == Manager.User.Id);
            c.Cart.Clear();
            return true;
        }

        public static Role GetRole(int ID)
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<Role>();
            Role r = collection.AsQueryable().FirstOrDefault(e => e.Id == ID);
            return r;
        }

        public static Role GetRole(string roleName)
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<Role>();
            Role r = collection.AsQueryable().FirstOrDefault(e => e.Name == roleName);
            return r;
        }

        public static Role[] GetAllRole()
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<Role>();
            Role[] r = collection.AsQueryable().ToArray();
            return r;
        }
        public static bool UpdateCart(MenuItem menuItem)
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<User>();
            User c = collection.AsQueryable().FirstOrDefault(e => e.Id == Manager.User.Id);
            if (c == null) return false;
            c.Cart.Add(menuItem);
            collection.ReplaceOne(c.Id, c);
            return true;
        }
        public static User[] LoadCart()
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<User>();

            User[] carts = collection.AsQueryable().ToArray<User>();
            return carts;
        }
        public static MenuItem[] LoadMenuItems()
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<MenuItem>();

            MenuItem[] items = collection.AsQueryable().ToArray<MenuItem>();
            return items;
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
                    Add_Reservation = true, // Can add an reservation
                    See_Reservation = true, // Can only see his own reservations
                    See_All_Reservations = false, // Can see all the reservations that are made
                    Cancel_Reservation = true, // Can cancel reservations
                    See_Menu = true, // Can see the menus
                    Add_Menu = false, // Can add new menus
                    Edit_Menu = false, // Edit an existing menus with new meals
                    Assign_Roles = false, // Can assign a new role to an user
                    Add_Reservation_Customer = false, // Can add an reservation to a user
                    Edit_Roles = false, // Can edit the roles
                    See_Transactions = false, // Can see the transactions
                    See_Takeaway_Orders = false, // Can see the takeaway orders
                    Access_Shopping_Card = true // Can access the shopping card and add meals to the shopping card
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
                    Add_Reservation_Customer = true,
                    Edit_Roles = true,
                    See_Transactions = true,
                    See_Takeaway_Orders = false,
                    Access_Shopping_Card = true
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
                    Add_Reservation_Customer = false,
                    Edit_Roles = false,
                    See_Transactions = false,
                    See_Takeaway_Orders = true,
                    Access_Shopping_Card = true
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
                    Add_Reservation_Customer = true,
                    Edit_Roles = false,
                    See_Transactions = true,
                    See_Takeaway_Orders = true,
                    Access_Shopping_Card = true
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
            if(collection.Count == 0)
            {
                Menu[] menu = {
                    new Menu
                    {
                        Id = 0,
                        Name = "Appetizer"
                    },
                    new Menu
                    {
                        Id = 1,
                        Name = "Main Dish"
                    },
                    new Menu
                    {
                        Id = 2,
                        Name = "Side Dish"
                    },
                    new Menu
                    {
                        Id = 3,
                        Name = "Dessert"
                    },
                    new Menu
                    {
                        Id = 4,
                        Name = "Drinks"
                    }
                };
                collection.InsertMany(menu);
            }
           
        }
        public static void MenuItemInit()
        {
            var store = new DataStore("data.json");
            var collection = store.GetCollection<MenuItem>();
            if(collection.Count == 0)
            {
                MenuItem[] menuitem =
                {
                    new MenuItem
                    {
                        Id = 0,
                        Menu_Id = 0,
                        Name = "Spring Rolls",
                        Price = 3.00,
                        Vegan = true,
                        Category = "Chinese"
                    },
                    new MenuItem
                    {
                        Id = 0,
                        Menu_Id = 0,
                        Name = "Steamed Buns",
                        Price = 4.00,
                        Vegan = true,
                        Category = ""
                    },
                    new MenuItem
                    {
                        Id = 0,
                        Menu_Id = 0,
                        Name = "Mais Soep",
                        Price = 2.50,
                        Vegan = true,
                        Category = ""
                    },
                    new MenuItem
                    {
                        Id = 0,
                        Menu_Id = 1,
                        Name = "Ramen",
                        Price = 12.00,
                        Vegan = true,
                        Category = ""
                    },
                    new MenuItem
                    {
                        Id = 0,
                        Menu_Id = 1,
                        Name = "Pokebolw",
                        Price = 10.00,
                        Vegan = true,
                        Category = ""
                    },
                    new MenuItem
                    {
                        Id = 0,
                        Menu_Id = 1,
                        Name = "T-Bone Steak",
                        Price = 25.50,
                        Vegan = false,
                        Category = ""
                    },
                    new MenuItem
                    {
                        Id = 0,
                        Menu_Id = 2,
                        Name = "Salade Mix",
                        Price = 5.00,
                        Vegan = true,
                        Category = ""
                    },
                    new MenuItem
                    {
                        Id = 0,
                        Menu_Id = 2,
                        Name = "Kim Chi",
                        Price = 4.00,
                        Vegan = true,
                        Category = ""
                    },
                    new MenuItem
                    {
                        Id = 0,
                        Menu_Id = 2,
                        Name = "patat",
                        Price = 2.60,
                        Vegan = true,
                        Category = ""
                    },
                    new MenuItem
                    {
                        Id = 0,
                        Menu_Id = 3,
                        Name = "Fruit Mix",
                        Price = 3.00,
                        Vegan = true,
                        Category = ""
                    },
                    new MenuItem
                    {
                        Id = 0,
                        Menu_Id = 3,
                        Name = "1 Bol of Ice of choice",
                        Price = 1.50,
                        Vegan = true,
                        Category = ""
                    },
                    new MenuItem
                    {
                        Id = 0,
                        Menu_Id = 3,
                        Name = "Random Alcohol Shot",
                        Price = 5.00,
                        Vegan = true,
                        Category = "vezels"
                    },
                    new MenuItem
                    {
                        Id = 0,
                        Menu_Id = 4,
                        Name = "all Drinks",
                        Price = 2.00,
                        Vegan = true,
                        Category = ""
                    },
                    new MenuItem
                    {
                        Id = 0,
                        Menu_Id = 4,
                        Name = "Alcoholic Drinks",
                        Price = 6.00,
                        Vegan = true,
                        Category = ""
                    }


                };

                collection.InsertMany(menuitem);
            }
        }
    }

}
