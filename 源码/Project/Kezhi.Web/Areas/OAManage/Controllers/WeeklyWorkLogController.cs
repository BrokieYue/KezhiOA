using Kezhi.Application;
using Kezhi.Application.OAManage;
using Kezhi.Application.SystemManage;
using Kezhi.Application.SystemSecurity;
using Kezhi.Code;
using Kezhi.Code.Excel;
using Kezhi.Domain.Entity.OA;
using Kezhi.Domain.Entity.SystemManage;
using Kezhi.Domain.Entity.SystemSecurity;
using Kezhi.Web.Areas.OAManage.CommonUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Kezhi.Web.Areas.OAManage.Controllers
{

    public class WeeklyWorkLogController : ControllerBase
    {
        private bool ImportResult = false;
        private WorkDailyRecordApp workDailyRecordApp = new WorkDailyRecordApp();
        private AreaApp areaApp = new AreaApp();
        private UserApp userApp = new UserApp();
        private ProjectApp projectApp = new ProjectApp();
        private RoleApp roleApp = new RoleApp();
        private ItemsDetailApp itemsDetailApp = new ItemsDetailApp();
        private ItemsApp itemApp = new ItemsApp();

        /// <summary>
        /// 界面模糊查询功能
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, DateTime? startTime, DateTime? endTime, string organize, string filiale,string weekflag)
        {
            if (weekflag != null && weekflag.Equals("true"))
            {
                startTime = Common.GetDateTimePrevWeekFirstDayMon(DateTime.Now.Date);
                endTime = Common.GetDateTimePrevWeekLastDaySun(DateTime.Now.Date);
            }
            else
            if (startTime.IsEmpty() || endTime.IsEmpty())
            {
                startTime = Common.GetDateTimeWeekFirstDayMon(DateTime.Now.Date);
                endTime = Common.GetDateTimeWeekLastDaySun(DateTime.Now.Date);

            }
            var data = new
            {
                rows = workDailyRecordApp.GetList(pagination, keyword, startTime, endTime, organize, filiale),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        /// <summary>
        /// 根据用户和项目查询日志
        /// </summary>
        /// <param name="user"></param>
        /// <param name="project"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public ActionResult GetListByUserAndProject(string user, string project, DateTime? startTime, DateTime? endTime, string weekflag)
        {
            if (weekflag != null && weekflag.Equals("true"))
            {
                startTime = Common.GetDateTimePrevWeekFirstDayMon(DateTime.Now.Date);
                endTime = Common.GetDateTimePrevWeekLastDaySun(DateTime.Now.Date);
            }
            else
            if (startTime == null && endTime == null)
            {
                startTime = Common.GetDateTimeWeekFirstDayMon(DateTime.Now.Date);
                endTime = Common.GetDateTimeWeekLastDaySun(DateTime.Now.Date);
            }
            var data = workDailyRecordApp.GetListByUserAndProject(user,project,startTime,endTime);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 地点列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetAddressSelectJson()
        {
            var data = areaApp.GetList();
            return Content(data.ToJson());
        }
        /// <summary>
        /// 查询登录用户的信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserLogin()
        {
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            var user = userApp.GetForm(LoginInfo.UserId);
            return Content(user.ToJson());
        }
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetUserSelectJson()
        {
            var data = userApp.GetList();
            return Content(data.ToJson());
        }
        /// <summary>
        /// 项目列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetProjectJson()
        {
            var data = projectApp.GetList();
            return Content(data.ToJson());
        }
        /// <summary>
        /// 根据选中的项目查询项目的地址
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public ActionResult GetProjectId(string projectId)
        {
            var data = projectApp.GetForm(projectId);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 编辑界面根据主键获取日志对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = workDailyRecordApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
     

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            workDailyRecordApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Info()
        {
            return View();
        }

        /// <summary>
        /// 进入导入界面
        /// </summary>
        /// <returns></returns>
        public ActionResult Import()
        {
            return View();
        }

        /// <summary>
        /// 导出模板
        /// </summary>
        /// <returns></returns>
        public ActionResult Excel()
        {
          
            DataTable table = new DataTable();
            try
            {
                return File(KezhiExcel.TemplateExcelToMs("上海科致工作日志模板.xls"), "application/vnd.ms-excel", "上海科致工作日志模板.xls");
            }
            catch (Exception ex)
            {
                return Error("下载出错，请重新下载");
            }
           
        }

        /// <summary>
        /// 导出周报
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="organize"></param>
        /// <param name="filiale"></param>
        /// <param name="weekflag"></param>
        /// <returns></returns>
        public ActionResult ExcelWeekWorkLog(string keyword, DateTime? startTime, DateTime? endTime, string organize, string filiale, string weekflag)
        {
            if (weekflag != null && weekflag.Equals("true"))
            {
                startTime = Common.GetDateTimePrevWeekFirstDayMon(DateTime.Now.Date);
                endTime = Common.GetDateTimePrevWeekLastDaySun(DateTime.Now.Date);
            }
            else if (startTime.IsEmpty() || endTime.IsEmpty())
            {
                startTime = Common.GetDateTimeWeekFirstDayMon(DateTime.Now.Date);
                endTime = Common.GetDateTimeWeekLastDaySun(DateTime.Now.Date);

            }
            List<V_WorkDailyRecordEntity> list = workDailyRecordApp.GetWeekListNoPage(keyword,null, startTime, endTime, organize, filiale);
            if (list.Count < 1)
            {
                return Error("没有需要导出的数据");
            }
            DataTable dataTable = KezhiExcel.ListToDataTable(list, true);
            //删除多与的列并排序
            GetPerformance(dataTable,list);
            //添加项目编号
            int projectNum = 1;
            
            for (var i = 0; i < dataTable.Rows.Count;i++ )
            {
                //如果日期为空，不格式化日期
                string date = dataTable.Rows[i]["F_WorkDate"].ToString();
                if(date.Equals("")){
                    continue;
                }
                dataTable.Rows[i]["F_WorkDate"] = CommonUtil.ConvertDate(date).ToString("yyyy-MM-dd");
                //项目编号
                dataTable.Rows[i]["F_ProjectCodeUser"] = dataTable.Rows[i]["F_ProjectCode"];
                //项目序号
                dataTable.Rows[0]["F_ProjectNum"] = 1;
                var prevProject = dataTable.Rows[i]["F_ProjectCode"].ToString();
                if (dataTable.Rows.Count == 1)
                {
                    continue;
                }else
                if (i < dataTable.Rows.Count - 1)
                {
                    var nextProject = dataTable.Rows[i + 1]["F_ProjectCode"].ToString();
                    if (!prevProject.Equals(nextProject))
                    {
                         projectNum++;
                         dataTable.Rows[i + 1]["F_ProjectNum"] = projectNum;
                    }
                }
                else if (!prevProject.Equals(dataTable.Rows[i - 1]["F_ProjectCode"].ToString()))
                {
                    dataTable.Rows[dataTable.Rows.Count - 1]["F_ProjectNum"] = projectNum;
                }
               

            }
            //人员序号
            int userNum = 1;
            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                
                dataTable.Rows[0]["F_UserNum"] = 1;
                var prevUserName = dataTable.Rows[i]["F_WorkUserName"].ToString();
                var prevProject = dataTable.Rows[i]["F_ProjectCode"].ToString();
                if (dataTable.Rows.Count == 1)
                {
                    continue;
                }
                else
                if (i < dataTable.Rows.Count - 1)
                {
                    var nextUserName = dataTable.Rows[i + 1]["F_WorkUserName"].ToString();
                    var nextProject = dataTable.Rows[i + 1]["F_ProjectCode"].ToString();
                   
                    if (!prevUserName.Equals(nextUserName) || (prevUserName.Equals(nextUserName) && !prevProject.Equals(nextProject)))
                    {
                        userNum++;
                        dataTable.Rows[i + 1]["F_UserNum"] = userNum;
                    }
                }
                else if (!prevUserName.Equals(dataTable.Rows[i - 1]["F_WorkUserName"].ToString()) || (prevUserName.Equals(dataTable.Rows[i - 1]["F_WorkUserName"].ToString()) && !prevProject.Equals(dataTable.Rows[i - 1]["F_ProjectCode"].ToString())))
                {
                    dataTable.Rows[dataTable.Rows.Count - 1]["F_UserNum"] = userNum;
                }
            }
            //周工作时长
            SetWeekWorkedHours(dataTable, startTime, endTime, organize, filiale, weekflag);

           return File(KezhiExcel.DataTableToMS(dataTable, "WorkWeekDailyRecord","周报"), "application/vnd.ms-excel", "上海科致周报" + DateTime.Now.ToShortDateString().ToString() + ".xls");
        }
        /// <summary>
        /// 设置周工作时长
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="organize"></param>
        /// <param name="filiale"></param>
        /// <param name="weekflag"></param>
        public void SetWeekWorkedHours(DataTable dataTable,DateTime? startTime, DateTime? endTime, string organize, string filiale, string weekflag)
        {
            int weekHoursStart = 1;
            int weekHoursEnd = 1;
            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                if (i == 0)
                {
                    weekHoursStart = 2;
                }
                else
                {
                    var prevUserNum = dataTable.Rows[i - 1]["F_UserNum"].ToString();
                    var nextUserNum = dataTable.Rows[i]["F_UserNum"].ToString();
                    if (prevUserNum != nextUserNum && !nextUserNum.Equals(""))
                    {
                        weekHoursStart = i + 2;
                    }
                }
                string keyword = dataTable.Rows[i]["F_WorkUserName"].ToString();
                string project = dataTable.Rows[i]["F_ProjectCode"].ToString();
                List<V_WorkDailyRecordEntity> list = workDailyRecordApp.GetWeekListNoPage(keyword, project, startTime, endTime, organize, filiale);
                weekHoursEnd = weekHoursStart + list.Count - 1;
                if (weekHoursEnd == weekHoursStart)
                {
                    dataTable.Rows[i]["F_WeekWorkedHours"] = list[0].F_WorkedHours;
                }
                else
                {
                    dataTable.Rows[i]["F_WeekWorkedHours"] = "SUM(N" + weekHoursStart + ":N" + weekHoursEnd + ")";
                }
            }
        }
       /// <summary>
       /// 添加、删除列并按照标题排序
       /// </summary>
       /// <param name="dataTable"></param>
       /// <param name="list"></param>
        private static void GetPerformance(DataTable dataTable,List<V_WorkDailyRecordEntity> list)
        {
            //添加项目序号和人员序号及周工作时间
            dataTable.Columns.Add("F_ProjectNum", typeof(Int32));
            dataTable.Columns.Add("F_UserNum", typeof(Int32));
            dataTable.Columns.Add("F_WeekWorkedHours", typeof(string));
            dataTable.Columns.Add("F_ProjectCodeUser", typeof(string));

            //删除多与的列

            dataTable.Columns.Remove("F_DeleteMark");
            dataTable.Columns.Remove("F_CreatorTime");
            dataTable.Columns.Remove("F_CreatorUserId");
            dataTable.Columns.Remove("F_LastModifyTime");
            dataTable.Columns.Remove("F_LastModifyUserId");
            dataTable.Columns.Remove("F_DeleteTime");
            dataTable.Columns.Remove("F_DeleteUserId");
            dataTable.Columns.Remove("F_CreateorUserName");
            dataTable.Columns.Remove("F_LastModifyUserName");
            dataTable.Columns.Remove("F_DeleteUserName");
            dataTable.Columns.Remove("F_WorkAddressCode");
            dataTable.Columns.Remove("F_RestHours");
            dataTable.Columns.Remove("F_DeductHours");
            dataTable.Columns.Remove("F_MealSubsidy");
            dataTable.Columns.Remove("F_WorkSubsidy");
            dataTable.Columns.Remove("F_CreatType");
            dataTable.Columns.Remove("F_IfLocal");
            dataTable.Columns.Remove("F_PayHours");
            dataTable.Columns.Remove("F_ItemId");
            dataTable.Columns.Remove("F_ItemName");
            dataTable.Columns.Remove("F_Id");
            dataTable.Columns.Remove("F_WorkUserId");
            dataTable.Columns.Remove("F_OrganizeName");
            dataTable.Columns.Remove("F_RoleName");
            dataTable.Columns.Remove("F_ProjectId");
            dataTable.Columns.Remove("F_WorkType");
            dataTable.Columns.Remove("F_EnabledMark");

            //设置列排序
            dataTable.Columns["F_ProjectNum"].SetOrdinal(0);
            dataTable.Columns["F_ProjectCode"].SetOrdinal(1);
            dataTable.Columns["F_ProjectName"].SetOrdinal(2);
            dataTable.Columns["F_ProjectManagerName"].SetOrdinal(3);
            dataTable.Columns["F_UserNum"].SetOrdinal(4);
            dataTable.Columns["F_WorkUserName"].SetOrdinal(5);
            dataTable.Columns["F_WorkDate"].SetOrdinal(6);
            dataTable.Columns["F_ProjectCodeUser"].SetOrdinal(7);
            dataTable.Columns["F_WorkAddress"].SetOrdinal(8);
            dataTable.Columns["F_DailyRecord"].SetOrdinal(9);
            dataTable.Columns["F_Description"].SetOrdinal(10);
            dataTable.Columns["F_WorkTimeStart"].SetOrdinal(11);
            dataTable.Columns["F_WorkTimeEnd"].SetOrdinal(12);
            dataTable.Columns["F_WorkedHours"].SetOrdinal(13);
            dataTable.Columns["F_WeekWorkedHours"].SetOrdinal(14);
            
        }

        /// <summary>
        /// 根据用户名获取Id
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        private string getUserId(string userName)
        {
            UserEntity user = userApp.GetUser(userName);
            if(user == null || string.IsNullOrEmpty(user.F_Id)){
                
                return null;
            }
            else
            {
                return user.F_Id;
            }
            //return userApp.GetUser(userName).F_Id;
        }

        /// <summary>
        /// 根据项目编号获取项目Id
        /// </summary>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        private ProjectEntity getProjectid(string projectCode)
        {
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

        /// <summary>
        /// 根据用户名查询岗位
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public RoleEntity getRole(string username)
        {
            UserEntity user = userApp.GetUser(username);
            return roleApp.GetForm(user.F_DutyId);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private bool RemoveFile(string fileName)
        {
            try
            {
                if (!System.IO.File.Exists(fileName))
                {
                    return true;
                }
                System.IO.File.Delete(fileName);
            }
            catch
            {
                return false;
            }

            return true;
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
        /// 转整型处理NULL值问题
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Int32 ToInt(object v)
        {
            if (v is System.DBNull || v == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(v);
            }
        }
        /// <summary>
        /// 转日期处理NULL值问题
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static DateTime ToDate(object v)
        {
            if (v is System.DBNull || v == null)
            {
                return DateTime.Now;
            }
            else
            {
                return Convert.ToDateTime(v);
            }
        }

      
    }
    
}
