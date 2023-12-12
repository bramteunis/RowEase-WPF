using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities
{
    public class GenRandom
    {
        public static string GenerateRandomString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(5, true));
            builder.Append(RandomNumber(100, 999));
            builder.Append(RandomString(4, false));
            return builder.ToString();
        }

        private static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char character;
            for (int i = 0; i < size; i++)
            {
                //haalt een random getal en zet die om naar een karakter
                character = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(character);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
    }
}
