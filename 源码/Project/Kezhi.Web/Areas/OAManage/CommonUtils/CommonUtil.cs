using Kezhi.Application.OAManage;
using Kezhi.Application.SystemManage;
using Kezhi.Domain.Entity.OA;
using Kezhi.Domain.Entity.SystemManage;
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

        /// <summary>
        /// 转字符串处理NULL值问题
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static string ToStr(object v)
        {
            if (v is System.DBNull || v == null)
            {
                return "";
            }
            else
            {
                return Convert.ToString(v);
            }
        }

        /// <summary>
        /// 根据用户名获取Id
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string getUserId(string userName)
        {
            UserApp userApp = new UserApp();
            UserEntity user = userApp.GetUser(userName);
            if (user == null || string.IsNullOrEmpty(user.F_Id))
            {

                return null;
            }
            else
            {
                return user.F_Id;
            }
        }

        /// <summary>
        /// 根据项目编号获取项目Id
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public static ProjectEntity getProjectid(string projectCode)
        {
            ProjectApp projectApp = new ProjectApp();
            ProjectEntity project = new ProjectEntity();
            if (string.IsNullOrEmpty(projectCode))
            {
                project.F_ProjectName = "";
                project.F_Id = "";
                return project;
            }
            project = projectApp.GetProject(projectCode);
            if (project == null || string.IsNullOrEmpty(project.F_Id))
            {
                return null;
            }
            else
            {
                return project;
            }
            //return projectApp.GetProject(projectCode).F_Id;
        }

        public static List<LodgingHouseEntity> getLoadingHouse(string houseCode)
        {
            LodgingHouseApp app = new LodgingHouseApp();
            return app.GetEntityByCode(houseCode);
            
        }

    }
}