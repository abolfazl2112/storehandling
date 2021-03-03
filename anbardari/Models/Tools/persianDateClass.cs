using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace anbardari.Models.Tools
{
    public class persianDateClass
    {
        /// <summary>
        /// این تابع تاریخ شمسی و زمان را با فرمت های مختلف را برمی گرداند
        /// </summary>
        /// <param name="yyyy/mm/dd H:i:s"></param>
        /// <returns></returns>
        public static string getPersianDateTime(string format="")
        {
            if (format == "")
                format = "yyyy/mm/dd hh:ii:ss";

            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            
            var day = pc.GetDayOfMonth(DateTime.Now);
            var month = pc.GetMonth(DateTime.Now);
            var y = pc.GetYear(DateTime.Now).ToString();

            var d = (day > 9 ? day.ToString() : "0" + day.ToString());
            var m = (month > 9 ? month.ToString() : "0" + month.ToString());

            var hour = pc.GetHour(DateTime.Now);
            var minute = pc.GetMinute(DateTime.Now);
            var second = pc.GetSecond(DateTime.Now);

            var h = (hour > 9 ? hour.ToString() : "0" + hour.ToString());
            var i = (minute > 9 ? minute.ToString() : "0" + minute.ToString());
            var s = (second > 9 ? second.ToString() : "0" + second.ToString());

            format = format.Replace("yyyy", y);
            format = format.Replace("mm", m);
            format = format.Replace("dd", d);

            format = format.Replace("yy", y.PadRight(2));
            format = format.Replace("m", month.ToString());
            format = format.Replace("d", day.ToString());

            format = format.Replace("hh", h);
            format = format.Replace("ii", i);
            format = format.Replace("ss", s);

            format = format.Replace("h", hour.ToString());
            format = format.Replace("i", minute.ToString());
            format = format.Replace("s", second.ToString());

            return format;
        }

        /// <summary>
        /// تبدیل رشته به تاریخ
        /// </summary>
        /// <param name="تاریخ بصورت رشته ای"></param>
        /// <returns>تاریخ با فرمت yyyy/mm/dd</returns>
        public static string strToDate(string date)
        {
            try
            {
                var y = date.Substring(0, 4);
                var m = date.Substring(4, 2);
                var d = date.Substring(6, 2);

                return y + "/" + m + "/" + d;
            }
            catch (Exception ex)
            {
                return "1300/01/01";
            }
            
        }

        /// <summary>
        /// تبدیل رشته به ساعت و زمان
        /// </summary>
        /// <param name="زمان بصورت رشته"></param>
        /// <returns>زمان با فرمت hh:ii</returns>
        public static string strToTime(string date)
        {
            try
            {
                if (date.Length == 0)
                {
                    return "00:00";
                }
                if (date.Length == 1)
                {
                    return "00:0" + date;
                }
                if (date.Length == 2)
                {
                    return "00:" + date;
                }
                if (date.Length == 3)
                {
                    return "0" + date.Substring(0, 1) + ":" + date.Substring(1, 2);
                }
                var h = date.Substring(0, 2);
                var s = date.Substring(2, 2);
                return h + ":" + s;
            }
            catch (Exception ex)
            {
                return "00:00";
            }
            
        }

        /// <summary>
        /// تاریخ چند روز قبل یا بعد
        /// </summary>
        /// <param name="currentDate">تاریخ بصورت عددی با فرمت yyyymmdd</param>
        /// <param name="day">تعداد روز بعد یا قبل</param>
        /// <returns></returns>
        public static int dateDecInc(int date,int day)
        {
            try
            {
                var y = int.Parse(date.ToString().Substring(0, 4));
                var m = int.Parse(date.ToString().Substring(4, 2));
                var d = int.Parse(date.ToString().Substring(6, 2));

                int dayOfMonth = (m <= 6 ? 31 : 30);

                var dTMP = ((d - day) <= 0 ? (d - day) + dayOfMonth : d - day);
                var dd = dTMP < 10 ? "0" + dTMP.ToString() : dTMP.ToString();

                var mTMP = ((d - day) <= 0 ? (m == 1?12:m-1) : m);
                var mm = mTMP < 10 ? "0" + mTMP.ToString() : mTMP.ToString();

                var yTMP = (m == 1 ? y - 1 : y);
                var yy = yTMP.ToString();

                return int.Parse(yy+mm+dd);
            }
            catch (Exception ex)
            {
                return date;
            }

        }
    }
}