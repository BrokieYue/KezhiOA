﻿/*******************************************************************************
 * Copyright © 2016 NFine.Framework 版权所有
 * Author: NFine
 * Description: NFine快速开发平台
 * Website：http://www.nfine.cn
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Kezhi.Code
{
    /// <summary>
    /// 常用公共类
    /// </summary>
    public class Common
    {
        #region Stopwatch计时器
        /// <summary>
        /// 计时器开始
        /// </summary>
        /// <returns></returns>
        public static Stopwatch TimerStart()
        {
            Stopwatch watch = new Stopwatch();
            watch.Reset();
            watch.Start();
            return watch;
        }
        /// <summary>
        /// 计时器结束
        /// </summary>
        /// <param name="watch"></param>
        /// <returns></returns>
        public static string TimerEnd(Stopwatch watch)
        {
            watch.Stop();
            double costtime = watch.ElapsedMilliseconds;
            return costtime.ToString();
        }
        #endregion

        #region 删除数组中的重复项
        /// <summary>
        /// 删除数组中的重复项
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string[] RemoveDup(string[] values)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < values.Length; i++)//遍历数组成员
            {
                if (!list.Contains(values[i]))
                {
                    list.Add(values[i]);
                };
            }
            return list.ToArray();
        }
        #endregion

        #region 自动生成编号
        /// <summary>
        /// 表示全局唯一标识符 (GUID)。
        /// </summary>
        /// <returns></returns>
        public static string GuId()
        {
            return Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 自动生成编号  201008251145409865
        /// </summary>
        /// <returns></returns>
        public static string CreateNo()
        {
            Random random = new Random();
            string strRandom = random.Next(1000, 10000).ToString(); //生成编号 
            string code = DateTime.Now.ToString("yyyyMMddHHmmss") + strRandom;//形如
            return code;
        }
        #endregion

        #region 生成0-9随机数
        /// <summary>
        /// 生成0-9随机数
        /// </summary>
        /// <param name="codeNum">生成长度</param>
        /// <returns></returns>
        public static string RndNum(int codeNum)
        {
            StringBuilder sb = new StringBuilder(codeNum);
            Random rand = new Random();
            for (int i = 1; i < codeNum + 1; i++)
            {
                int t = rand.Next(9);
                sb.AppendFormat("{0}", t);
            }
            return sb.ToString();

        }
        #endregion

        #region 删除最后一个字符之后的字符
        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }
        /// <summary>
        /// 删除最后结尾的长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string DelLastLength(string str, int Length)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            str = str.Substring(0, str.Length - Length);
            return str;
        }
        #endregion

        #region 字符串与数字的验证与转换
        /// <summary>
        /// 检测字符串能否转换成数字（int，float）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckIsNumByString(string str)
        {
            try
            {
                string[] nums = str.Split('.');
                //有小数点，判断他是否是float，无小数点，判断是否是int
                if (str.Contains("."))
                {
                    if (nums.Length == 2) //是float
                    {
                        if (nums[0].Equals("") || nums[1].Equals(""))
                        {
                            return false;
                        }
                        return true;
                    }
                    else if (nums.Length > 2 || nums.Length == 1)//带好几个点\
                    {
                        return false;
                    }
                }
                else //没小数点
                {
                    System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"^\d+$");
                    if (rex.IsMatch(str))//int
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
           
           
        }
        //字符串转换成数字
        public static float SetNumberByString(string str)
        {
            try
            {
                string[] nums = str.Split('.');
                //有小数点，判断他是否是float，无小数点，判断是否是int
                if (str.Contains("."))
                {
                    if (nums.Length == 2) //是float
                    {
                        if (nums[0].Equals("") || nums[1].Equals(""))
                        {
                            return 0;
                        }
                        return Convert.ToSingle(str);
                    }
                    else if (nums.Length > 2 || nums.Length == 1)//带好几个点\
                    {
                        return 0;
                    }
                }
                else //没小数点
                {
                    System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"^\d+$");
                    if (rex.IsMatch(str))//int
                    {
                        return Convert.ToInt32(str);
                    }
                }
                return 0;
            }
            catch (Exception e)
            {
                return 0;
            }
            
        }
        #endregion

        #region 获取指定日期所在周的第一天和最后一天
        /// <summary>  
        /// 获取指定日期所在周的第一天，星期一为第一天  
        /// </summary>  
        /// <param name="dateTime"></param>  
        /// <returns></returns>  
        public static DateTime GetDateTimeWeekFirstDayMon(DateTime dateTime)
        {
            DateTime firstWeekDay = DateTime.Now;

            try
            {
                int weeknow = Convert.ToInt32(dateTime.DayOfWeek);

                //星期一为第一天，weeknow等于0时，要向前推6天。     
                weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));

                int daydiff = (-1) * weeknow;

                firstWeekDay = dateTime.AddDays(daydiff);
            }
            catch { }

            return firstWeekDay;
        }
        /// <summary>  
        /// 获取指定日期所在周的最后一天，星期天为最后一天  
        /// </summary>  
        /// <param name="dateTime"></param>  
        /// <returns></returns>  
        public static DateTime GetDateTimeWeekLastDaySun(DateTime dateTime)
        {
            DateTime lastWeekDay = DateTime.Now;

            try
            {
                int weeknow = Convert.ToInt32(dateTime.DayOfWeek);

                weeknow = (weeknow == 0 ? 7 : weeknow);

                int daydiff = (7 - weeknow);

                lastWeekDay = dateTime.AddDays(daydiff);
            }
            catch { }

            return lastWeekDay;
        }
        /// <summary>
        /// 获取指定日期前一周的第一天，星期一为第一天  
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetDateTimePrevWeekFirstDayMon(DateTime dateTime)
        {
            DateTime firstWeekDay = DateTime.Now;

            try
            {
                int weeknow = Convert.ToInt32(dateTime.DayOfWeek);

                weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));

                int daydiff = (-1) * weeknow - 7;

                firstWeekDay = dateTime.AddDays(daydiff);
            }
            catch { }

            return firstWeekDay;
        }
        /// <summary>  
        /// 获取指定日期前一周的最后一天，星期天为最后一天  
        /// </summary>  
        /// <param name="dateTime"></param>  
        /// <returns></returns>  
        public static DateTime GetDateTimePrevWeekLastDaySun(DateTime dateTime)
        {
            DateTime lastWeekDay = DateTime.Now;

            try
            {
                int weeknow = Convert.ToInt32(dateTime.DayOfWeek);

                weeknow = (weeknow == 0 ? 7 : weeknow);

                int daydiff = (7 - weeknow) - 7;

                lastWeekDay = dateTime.AddDays(daydiff);
            }
            catch { }

            return lastWeekDay;
        }  
        #endregion

        #region 将时间转换成float
        /// <summary>
        /// 将时间转换成float
        /// </summary>
        /// <param name="dayTime">时间（HH:mm;ss）</param>
        /// <returns></returns>

        public static float SetNumberByDateTime(string dayTime){
            float workHours = 0;
            if (dayTime.Contains("："))
            {
                dayTime.Replace("：", ":");
            }
            if (dayTime.Contains(":"))
            {
                string[] time = dayTime.Split(':');
                if (time.Length == 3 || (time.Length == 2 && !time[1].Equals("")))
                {
                    if (Convert.ToInt32(time[0]) == 0)
                    {
                        time[0] = "24";
                    }
                    //早晨8点以后为新的一天，不计入加班时间
                    else if (Convert.ToInt32(time[0]) > 0 && Convert.ToInt32(time[0]) < 8)
                    {
                        time[0] = (Convert.ToInt32(time[0]) + 24).ToString();
                    }
                    float mm = Convert.ToSingle(time[1]) / 60;
                    //在0分-25分,25分-45分，45分-0分
                    if(mm < 0.5){
                        mm = 0F;
                    }else if(mm >= 0.5){
                        mm = 0.5F;
                    }
                    workHours = Convert.ToSingle(time[0]) + mm;
                }
            }
            return workHours;
        }
        #endregion

        #region 计算指定日期所在月份的第一天和最后一天
        //计算输入日期所在的月的第一天
        public static DateTime GetMonthStartTime(DateTime dt)
        {

            DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初
            return startMonth;

        }
        //计算输入日期所在月的最后一天
        public static DateTime GetMonthEndTime(DateTime dt)
        {
            DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末//
            return endMonth;
        }
        #endregion
    }
}
