using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Jan.Core
{
    /// <summary>
    /// Provides utility methods for working with strings in the application.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Splits a camel case or Pascal case string into separate words.
        /// Each word is separated by a space.
        /// If the string already contains spaces, it returns the string as-is.
        /// </summary>
        /// <param name="stringToSplit">The string to be split into words.</param>
        /// <returns>A string with the input words separated by spaces.</returns>
        public static string SplitWords(this string stringToSplit)
        {
            // If the string already contains spaces, return it as-is
            if (stringToSplit.Contains(' '))
            {
                return stringToSplit;
            }

            string[] str = Regex.Split(stringToSplit, @"(?<!^)(?=[A-Z])");
            StringBuilder nameSection = new StringBuilder();

            foreach (var s in str)
            {
                nameSection.Append(s).Append(" ");
            }

            return nameSection.ToString().TrimEnd();
        }

        public static string MergeWords(this string[] words)
        {
            StringBuilder nameSection = new StringBuilder();

            foreach (var word in words)
            {
                nameSection.Append(word).Append(" ");
            }

            return nameSection.ToString().TrimEnd();
        }

        public static string ToRoman(this int input)
        {
            return input switch
            {
                1 => "I",
                2 => "II",
                3 => "III",
                4 => "IV",
                5 => "V",
                6 => "VI",
                7 => "VII",
                8 => "VIII",
                9 => "IX",
                10 => "X",
                _ => input.ToString()
            };
        }
    }
}