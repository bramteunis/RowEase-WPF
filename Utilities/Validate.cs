using System.Text.RegularExpressions;

namespace Utilities
{
    public class Validate
    {
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            if (Regex.IsMatch(email, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")) return true;

            return false;
        }

        public static bool ValidCharacters(string characters)
        {
            Regex regex = new Regex("^[0-9A-Za-z\\-\\/]+$");
            if (!regex.IsMatch(characters) || characters.Contains(" "))
            {
                return false;
            }
            return true;
        }
    }
}