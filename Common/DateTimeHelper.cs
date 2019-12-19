using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DateTimeHelper
    {
        public static string CurrentDateTime
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        #region 方法

        /// <summary>
        /// 计算某年某月某日是星期几（周日为0）
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static int GetWeekByDay(int year, int month, int day)
        {
            DateTime dt = new DateTime(year, month, day);
            switch (dt.DayOfWeek.ToString())
            {
                case "Monday":
                    return 1;
                case "Tuesday":
                    return 2;
                case "Wednesday":
                    return 3;
                case "Thursday":
                    return 4;
                case "Friday":
                    return 5;
                case "Saturday":
                    return 6;
                case "Sunday":
                    return 0;
                default:
                    return 1;
            }
        }

        /// <summary>
        /// //计算某年某月1号是星期几（周日为0）
        /// </summary>
        /// <returns></returns>
        public static int GetWeekInMonth(int year, int month)
        {
            DateTime dt = new DateTime(year, month, 1);
            switch (dt.DayOfWeek.ToString())
            {
                case "Monday":
                    return 1;
                case "Tuesday":
                    return 2;
                case "Wednesday":
                    return 3;
                case "Thursday":
                    return 4;
                case "Friday":
                    return 5;
                case "Saturday":
                    return 6;
                case "Sunday":
                    return 0;
                default:
                    return 1;
            }
        }

        /// <summary>
        /// //计算某年某月最后一天是星期几（周日为0）
        /// </summary>
        /// <returns></returns>
        public static int GetLastWeekInMonth(int year, int month)
        {
            DateTime dt = new DateTime(year, month, 1);
            dt = dt.AddMonths(1).AddDays(-1);
            switch (dt.DayOfWeek.ToString())
            {
                case "Monday":
                    return 1;
                case "Tuesday":
                    return 2;
                case "Wednesday":
                    return 3;
                case "Thursday":
                    return 4;
                case "Friday":
                    return 5;
                case "Saturday":
                    return 6;
                case "Sunday":
                    return 0;
                default:
                    return 1;
            }
        }

        /// <summary>
        /// 计算某年某月有多少天
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int GetMonthLength(int year, int month)
        {
            return DateTime.DaysInMonth(year, month);
        }

        /// <summary>
        /// 计算某月有多少周
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int GetMonthWeek(int year, int month)
        {
            int totalDays = GetMonthLength(year, month);
            int startWeek = GetWeekInMonth(year, month);
            int lastWeek = GetLastWeekInMonth(year, month);

            //第一周天数
            int firstWeekDays = startWeek == 0 ? 1 : (7 - startWeek + 1);

            //最后一周天数
            int lastWeekDays = lastWeek == 0 ? 7 : lastWeek;

            return (2 + (totalDays - firstWeekDays - lastWeekDays) / 7);

        }

        /// <summary>
        /// 计算某天是当月的第几周
        /// </summary>
        /// <returns></returns>
        public static int GetCurrMonthWeek(int year, int month, int day)
        {
            int startWeek = GetWeekInMonth(year, month);
            //第一周天数
            int firstWeekDays = startWeek == 0 ? 1 : (7 - startWeek + 1); ;

            if (day <= firstWeekDays)
            {
                return 1;
            }
            else if (day <= (firstWeekDays + 7))
            {
                return 2;
            }
            else if (day <= (firstWeekDays + 7 * 2))
            {
                return 3;
            }
            else if (day <= (firstWeekDays + 7 * 3))
            {
                return 4;
            }
            else if (day <= (firstWeekDays + 7 * 4))
            {
                return 5;
            }
            else
            {
                return 6;
            }
        }

        /// <summary>
        /// 计算某年某月某周第一天是几号
        /// </summary>
        /// <returns></returns>
        public static int GetWeekFirstDay(int year, int month, int week)
        {
            int startWeek = GetWeekInMonth(year, month);
            int lastWeek = GetLastWeekInMonth(year, month);

            //最后一周天数
            int lastWeekDays = lastWeek == 0 ? 7 : lastWeek;

            //每个月至少有4周
            int totalWeek = GetMonthWeek(year, month);

            //最后一周第一天
            if (week >= totalWeek)
            {
                return (GetMonthLength(year, month) + 1 - lastWeekDays);
            }

            if (week == 1)
            {
                return 1;
            }
            else if (week == 2)
            {
                return GetWeekLastDay(year, month, 1) + 1;
            }
            else if (week == 3)
            {
                return GetWeekLastDay(year, month, 2) + 1;
            }
            else if (week == 4)
            {
                return GetWeekLastDay(year, month, 3) + 1;
            }
            else if (week == 5)
            {
                return GetWeekLastDay(year, month, 4) + 1;
            }
            else if (week == 6)
            {
                return GetWeekLastDay(year, month, 5) + 1;
            }
            else
            {
                return (GetMonthLength(year, month) + 1 - lastWeekDays);
            }

        }

        /// <summary>
        /// 计算某年某月某周最后一天是几号
        /// </summary>
        /// <returns></returns>
        public static int GetWeekLastDay(int year, int month, int week)
        {
            int startWeek = GetWeekInMonth(year, month);

            //每个月至少有4周
            int totalWeek = GetMonthWeek(year, month);

            //第一周天数
            int firstWeekDays = startWeek == 0 ? 1 : (7 - startWeek + 1);

            //最后一周
            if (week >= totalWeek)
            {
                return GetMonthLength(year, month);
            }

            if (week == 1)
            {
                return firstWeekDays;
            }
            else if (week == 2)
            {
                return (firstWeekDays + 7);
            }
            else if (week == 3)
            {
                return (firstWeekDays + 7 * 2);
            }
            else if (week == 4)
            {
                return (firstWeekDays + 7 * 3);
            }
            else if (week == 5)
            {
                return (firstWeekDays + 7 * 4);
            }
            else
            {
                return GetMonthLength(year, month);
            }

        }

        /// <summary>
        /// 星期数字转中文
        /// </summary>
        /// <param name="week"></param>
        /// <returns></returns>
        public static string GetWeekCN(int week)
        {
            switch (week)
            {
                case 1:
                    return "星期一";
                case 2:
                    return "星期二";
                case 3:
                    return "星期三";
                case 4:
                    return "星期四";
                case 5:
                    return "星期五";
                case 6:
                    return "星期六";
                case 0:
                    return "星期日";
                default:
                    return "星期有误";
            }
        } 
        #endregion
    }
}
