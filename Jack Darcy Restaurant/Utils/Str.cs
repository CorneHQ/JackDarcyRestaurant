using System;
using System.Collections.Generic;
using System.Text;

namespace Jack_Darcy_Restaurant.Utils
{
    public class Str
    {
        public static string randomRegistrationKey(int length = 8)
        {
            char[] alphabet = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string output = "";
            Random rnd = new Random();
            for(int i = 0; i < length; i++)
            {
                output += alphabet[rnd.Next(0, 25)];
            }
            return output;
        }
    }
}
