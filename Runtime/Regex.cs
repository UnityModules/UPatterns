using System.Text.RegularExpressions;

namespace UPatterns
{
    public static class URegex
    {
        public static bool Lowercase(string value) =>
             Regex.IsMatch(value, @"[a-z]");
        public static bool Uppercase(string value) =>
             Regex.IsMatch(value, @"[A-Z]");
        public static bool Number(string value) =>
             Regex.IsMatch(value, @"[0-9]");
        public static bool SpecialCharacter(string value) =>
             Regex.IsMatch(value, @"[!@#$%^&*(),.?\:{}|<>]");
        public static bool Email(string value) =>
             Regex.IsMatch(value, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
    }
}