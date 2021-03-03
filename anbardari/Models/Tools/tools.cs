using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace anbardari.Models.Tools
{
    public class tools
    {
        public static string toPersianNumber(string input)
        {
            string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int j = 0; j < persian.Length; j++)
                input = input.Replace(j.ToString(), persian[j]);

            return input;
        }

        public static string toEnglishNumber(string input)
        {
            string[] pn = { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
            string[] en = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string chash = input;
            for (int i = 0; i < 10; i++)
                chash = chash.Replace(pn[i], en[i]);
            return chash;
        }

        public static string toShamsiMonthName(int month)
        {
            if (month == 1)
            {
                return "فروردین";
            }
            else if (month == 2)
            {
                return "اردیبهشت";

            }
            else if (month == 3)
            {
                return "خرداد";

            }
            else if (month == 4)
            {
                return "تیر";

            }
            else if (month == 5)
            {
                return "مرداد";

            }
            else if (month == 6)
            {
                return "شهریور";

            }
            else if (month == 7)
            {
                return "مهر";

            }
            else if (month == 8)
            {
                return "آبان";

            }
            else if (month == 9)
            {
                return "آذر";

            }
            else if (month == 10)
            {
                return "دی";

            }
            else if (month == 11)
            {
                return "بهمن";

            }
            else if (month == 12)
            {
                return "اسفند";

            }
            return "";
        }
    }
}