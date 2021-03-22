using System;
using System.Collections.Generic;
using System.Text;

namespace Jack_Darcy_Restaurant.Model
{
    public class User
    {
        public int Id { get; set; }
        public int Role_Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User(int cId, string cName, string cPassword, string cEmail, int cRoles)
        {
            Id = cId;
            Name = cName;
            Password = cPassword;
            Email = cEmail;
            Role_Id = cRoles;
        }

        public bool Validate(string vName, string vPassword)
        {
            if (Name == vName && Password == vPassword)
            {
                Manager.Role_id = Role_Id;
                return true;
            }

            return false;
        }
    }
}
