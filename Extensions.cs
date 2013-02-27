using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NavisTimelinerPlugin
{
    public static class Extensions
    {
        public static double ToDouble(this string s)
        {
            double result;
            double.TryParse(s, out result);
            return result;
        }

        /// <summary>
        /// Убирает буквы из полученной строки.
        /// </summary>
        public static string removeLetters(this string str)
        {
            string result = null;
            foreach (char c in str)
            {
                if (char.IsDigit(c) || c == ',' || c == '.')
                    result += c;
            }
            return result;
        }
    }
}
