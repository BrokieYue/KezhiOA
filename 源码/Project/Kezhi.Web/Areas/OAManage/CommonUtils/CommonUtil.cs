using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Kezhi.Web.Areas.OAManage.CommonUtils
{
    public class CommonUtil
    {
        /// <summary>
        /// 将字符串转换成DateTime
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime ConvertDate(string date)
        {
            DateTime dt = new DateTime();
            try
            {
                if (!string.IsNullOrEmpty(date))
                {
                    dt = Convert.ToDateTime(date);
                }
                return dt;
            }
            catch (Exception e)
            {
                return dt;
            }
            
        }

        /// <summary>
        /// 根据日期字符串获取对应的星期
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetWeekByTime(string time)
        {
            string dt = "";
            string week = "";
            dt = ConvertDate(time).DayOfWeek.ToString();
            switch (dt)
            {
            case "Monday":
            week = "星期一";
            break;
            case "Tuesday":
            week = "星期二";
            break;
            case "Wednesday":
            week = "星期三";
            break;
            case "Thursday":
            week = "星期四";
            break;
            case "Friday":
            week = "星期五";
            break;
            case "Saturday":
            week = "星期六";
            break;
            case "Sunday":
            week = "星期日";
            break;
            }
            return week; 
        }
    }
}