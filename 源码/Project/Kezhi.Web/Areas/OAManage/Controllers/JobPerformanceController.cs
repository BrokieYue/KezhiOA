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
using System.Web;
using System.Web.Mvc;

namespace Kezhi.Web.Areas.OAManage.Controllers
{

    public class JobPerformanceController : ControllerBase
    {
        private WorkDailyRecordApp workDailyRecordApp = new WorkDailyRecordApp();
        private AreaApp areaApp = new AreaApp();
        private UserApp userApp = new UserApp();
        private ProjectApp projectApp = new ProjectApp();
        private RoleApp roleApp = new RoleApp();
        private ItemsDetailApp itemsDetailApp = new ItemsDetailApp();
        private ItemsApp itemApp = new ItemsApp();
        private OrganizeApp oraginzeApp = new OrganizeApp();

        /// <summary>
        /// 界面模糊查询功能
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, DateTime? startTime, DateTime? endTime, string organize, string filiale, string prevmonth)
        {
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            if (prevmonth != null && prevmonth.Equals("true"))
            {
                startTime = Common.GetMonthStartTime(DateTime.Now.Date).AddMonths(-1);
                endTime = Common.GetMonthStartTime(DateTime.Now.Date).AddHours(-1);
            }
            if (startTime == null || endTime == null)
            {
                startTime = Common.GetMonthStartTime(DateTime.Now.Date);
                endTime = Common.GetMonthEndTime(DateTime.Now.Date);
            }
            //非行政部管理人员只能查询本部门日志
            UserEntity user = userApp.GetForm(LoginInfo.UserId);
            if (!LoginInfo.UserCode.Equals("admin"))
            {
                var organizeName = oraginzeApp.GetForm(user.F_DepartmentId).F_FullName;
                if (!organizeName.Equals("行政部"))
                {
                    organize = organizeName;
                }
            }
            List<V_WorkDailyRecordEntity> list = workDailyRecordApp.GetList(pagination, keyword, startTime, endTime, organize, filiale);
            foreach (var entity in list)
            {
                if (entity.F_WorkAddress.Equals("其他"))
                {
                    entity.F_WorkAddress = entity.F_OtherAddress;
                }
                if (!string.IsNullOrEmpty(entity.F_WorkCategory) && !entity.F_WorkCategory.Equals("项目实施"))
                {
                    entity.F_ProjectCode = entity.F_ProjectId;
                    entity.F_ProjectName = entity.F_ProjectId;
                }
            }
            var data = new
            {
                rows = list,
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        /// <summary>
        /// 导出绩效
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="organize"></param>
        /// <param name="filiale"></param>
        /// <returns></returns>
        public ActionResult ExcelPerformance(string keyword, DateTime? startTime, DateTime? endTime, string organize, string filiale, string prevmonth)
        {
            if (prevmonth != null && prevmonth.Equals("true"))
            {
                startTime = Common.GetMonthStartTime(DateTime.Now.Date).AddMonths(-1);
                endTime = Common.GetMonthStartTime(DateTime.Now.Date).AddHours(-1);
            }
            if (startTime == null || endTime == null)
            {
                startTime = Common.GetMonthStartTime(DateTime.Now.Date);
                endTime = Common.GetMonthEndTime(DateTime.Now.Date);
            }
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            //非行政部管理人员只能查询本部门日志
            UserEntity user = userApp.GetForm(LoginInfo.UserId);
            if (!LoginInfo.UserCode.Equals("admin"))
            {
                var organizeName = oraginzeApp.GetForm(user.F_DepartmentId).F_FullName;
                if (!organizeName.Equals("行政部"))
                {
                    organize = organizeName;
                }
            }
            List<V_WorkDailyRecordEntity> list = workDailyRecordApp.GetListNoPage(keyword, startTime, endTime, organize, filiale);
            if (list.Count < 1)
            {
                return Error("没有需要导出的数据");
            }
            foreach (var entity in list)
            {
                if (entity.F_WorkAddress.Equals("其他"))
                {
                    entity.F_WorkAddress = entity.F_OtherAddress;
                }
                //if (!string.IsNullOrEmpty(entity.F_WorkCategory) && !entity.F_WorkCategory.Equals("项目实施"))
                //{
                //    entity.F_ProjectCode = entity.F_ProjectId;
                //    entity.F_ProjectName = entity.F_ProjectId;
                //}
            }
            DataTable dataTable = KezhiExcel.ListToDataTable(list, true);

            GetPerformance(dataTable, list);


            //添加星期列
            dataTable.Columns.Add("F_Weekday", typeof(string));
            dataTable.Columns["F_Weekday"].SetOrdinal(2);
            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                string date = dataTable.Rows[i]["F_WorkDate"].ToString();
                //如果日期为空，不添加星期和日期
                if (date.Equals(""))
                {
                    continue;
                }
                dataTable.Rows[i]["F_WorkDate"] = CommonUtil.ConvertDate(date).ToString("yyyy-MM-dd");
                dataTable.Rows[i]["F_Weekday"] = CommonUtil.GetWeekByTime(date);
            }
            return File(KezhiExcel.DataTableToMSByJobPerformance(dataTable, "performance", ""), "application/vnd.ms-excel", "上海科致考勤绩效" + DateTime.Now.ToShortDateString().ToString() + ".xls");
        }

        /// <summary>
        /// 获取绩效
        /// </summary>
        /// <param name="list">绩效信息</param>
        /// <param name="list1">小计信息</param>
        /// <param name="list2">小计数量</param>
        private static void GetPerformance(DataTable dataTable, List<V_WorkDailyRecordEntity> list)
        {
            //统计个人绩效
            var WorkedHours = 0;//工作时长（结束的位置）
            var WorkedHourStart = 2;//开始的位置
            var PayHours = 0;//支付时长
            var PayHourStart = 2;
            var RestHours = 0;//调休时长
            var RestHourStart = 2;
            var DeductHours = 0;//考核扣除工时
            var DeductHourStart = 2;
            var MealSubsidy = 0;//餐贴
            var MealSubsidyStart = 2;
            var WorkSubsidy = 0;//津贴
            var WorkSubsidyStart = 2;
            var listCount = list.Count;

            List<JobPerformanceEntitiy> list1 = new List<JobPerformanceEntitiy>();
            List<int> list2 = new List<int>();

            //小计的计算
            for (int i = 0; i < listCount; i++)
            {
                
                if (i == listCount - 1 || !list[i].F_WorkUserId.Equals(list[i + 1].F_WorkUserId))
                {

                    JobPerformanceEntitiy entity = new JobPerformanceEntitiy();
                    entity.F_WorkUserName = "小计";
                    PayHours = PayHourStart + PayHours;
                    entity.F_PayHours = "sum(H" + PayHourStart + ":H" + PayHours + ")";
                    MealSubsidy = MealSubsidyStart + MealSubsidy;
                    entity.F_MealSubsidy = "sum(K" + MealSubsidyStart + ":K" + MealSubsidy + ")";
                    WorkSubsidy = WorkSubsidyStart + WorkSubsidy;
                    entity.F_WorkSubsidy = "sum(L" + WorkSubsidyStart + ":L" + WorkSubsidy + ")";
                    entity.F_WorkUserId = list[i].F_WorkUserId;
                    WorkedHours = WorkedHourStart + WorkedHours;
                    entity.F_WorkedHours = "sum(G" + WorkedHourStart + ":G" + WorkedHours + ")";
                    RestHours = RestHourStart + RestHours;
                    entity.F_RestHours = "sum(I" + RestHourStart + ":I" + RestHours + ")";
                    DeductHours = DeductHourStart + DeductHours;
                    entity.F_DeductHours = "sum(J" + DeductHourStart + ":J" + DeductHours + ")";
                    list1.Add(entity);
                    list2.Add(i);
                    //开始位置=前一个位置+ 2
                    PayHourStart = PayHours + 2;
                    MealSubsidyStart = MealSubsidy + 2;
                    WorkSubsidyStart = WorkSubsidy + 2;
                    WorkedHourStart = WorkedHours + 2;
                    RestHourStart = RestHours + 2;
                    DeductHourStart = DeductHours + 2;
                    //结束的位置
                    WorkedHours = -1;
                    PayHours = -1;
                    RestHours = -1;
                    DeductHours = -1;
                    MealSubsidy = -1;
                    WorkSubsidy = -1;
                }
                WorkedHours++;
                PayHours++;
                RestHours++;
                DeductHours++;
                MealSubsidy++;
                WorkSubsidy++;
            }
            //list.Sort((a, b) => a.F_WorkUserId.CompareTo(b.F_WorkUserId));

            //小计的设计（添加到table）
            int num = 0;
            for (var i = 0; i < list2.Count; i++)
            {
                num++;
                int r = list2[i] + num;
                if (i == list2.Count - 1)
                {
                    r += 1;
                }
                DataRow dr = dataTable.NewRow();
                dr["F_WorkUserName"] = "小计";
                dr["F_WorkedHours"] = list1[i].F_WorkedHours;
                dr["F_PayHours"] = list1[i].F_PayHours;
                dr["F_MealSubsidy"] = list1[i].F_MealSubsidy;
                dr["F_WorkSubsidy"] = list1[i].F_WorkSubsidy;
                dr["F_WorkUserId"] = list1[i].F_WorkUserId;
                dr["F_RestHours"] = list1[i].F_RestHours;
                dr["F_DeductHours"] = list1[i].F_DeductHours;
                dataTable.Rows.InsertAt(dr, r);

            }
            //删除多与的列
            dataTable.Columns.Remove("F_Id");
            dataTable.Columns.Remove("F_DailyRecord");
            dataTable.Columns.Remove("F_WorkUserId");
            dataTable.Columns.Remove("F_ProjectId");
            dataTable.Columns.Remove("F_IfLocal");
            dataTable.Columns.Remove("F_WorkType");
            dataTable.Columns.Remove("F_WorkTimeStart");
            dataTable.Columns.Remove("F_WorkTimeEnd");
            dataTable.Columns.Remove("F_CreatType");
            dataTable.Columns.Remove("F_Description");
            dataTable.Columns.Remove("F_EnabledMark");
            dataTable.Columns.Remove("F_DeleteMark");
            dataTable.Columns.Remove("F_CreatorTime");
            dataTable.Columns.Remove("F_CreatorUserId");
            dataTable.Columns.Remove("F_LastModifyTime");
            dataTable.Columns.Remove("F_LastModifyUserId");
            dataTable.Columns.Remove("F_DeleteTime");
            dataTable.Columns.Remove("F_CreateorUserName");
            dataTable.Columns.Remove("F_LastModifyUserName");
            dataTable.Columns.Remove("F_DeleteUserName");
            dataTable.Columns.Remove("F_ProjectName");
            dataTable.Columns.Remove("F_DeleteUserId");
            dataTable.Columns.Remove("F_RoleName");
            dataTable.Columns.Remove("F_ItemId");
            dataTable.Columns.Remove("F_ProjectManagerName");
            dataTable.Columns.Remove("F_OrganizeName");
            dataTable.Columns.Remove("F_ItemName");
            dataTable.Columns.Remove("F_WorkCategory");
            dataTable.Columns.Remove("F_OtherAddress");
            dataTable.Columns.Remove("F_CurrentDayMark");

            dataTable.Columns["F_WorkUserName"].SetOrdinal(0);
            dataTable.Columns["F_WorkDate"].SetOrdinal(1);
            dataTable.Columns["F_ProjectCode"].SetOrdinal(2);
            dataTable.Columns["F_WorkAddress"].SetOrdinal(3);
            dataTable.Columns["F_WorkAddressCode"].SetOrdinal(4);
            dataTable.Columns["F_WorkedHours"].SetOrdinal(5);
            dataTable.Columns["F_PayHours"].SetOrdinal(6);
            dataTable.Columns["F_RestHours"].SetOrdinal(7);
            dataTable.Columns["F_DeductHours"].SetOrdinal(8);
            dataTable.Columns["F_MealSubsidy"].SetOrdinal(9);
            dataTable.Columns["F_WorkSubsidy"].SetOrdinal(10);
        }


        /// <summary>
        /// 修改绩效
        /// </summary>
        /// <param name="workDailyRecordEntity"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitFormPerformance(WorkDailyRecordEntity workDailyRecordEntity, string keyValue)
        {

            if (workDailyRecordEntity.F_WorkAddress.Equals("在家休息"))
            {
                workDailyRecordEntity.F_WorkTimeStart = "\\";
                workDailyRecordEntity.F_WorkTimeEnd = "\\";
            }
            //1、支付工时 2、考核扣除工时
            string weekDate = CommonUtil.GetWeekByTime(workDailyRecordEntity.F_WorkDate.ToString());
            if (weekDate.Equals("星期六") || weekDate.Equals("星期日"))
            {
                workDailyRecordEntity.F_PayHours = "0";
                workDailyRecordEntity.F_DeductHours = workDailyRecordEntity.F_WorkedHours;
            }
            else
            {
                workDailyRecordEntity.F_PayHours = "8";
                float span = Convert.ToSingle(workDailyRecordEntity.F_WorkedHours) - 8F;
                workDailyRecordEntity.F_DeductHours = span > 0 ? span.ToString() : "0";
            }
            //可调休工时
           WorkDailyRecordEntity entity = workDailyRecordApp.GetEntity(keyValue);
           if (entity.F_WorkType.Equals("加班"))
            {
                workDailyRecordEntity.F_RestHours = workDailyRecordEntity.F_WorkedHours;
            }
            workDailyRecordApp.SubmitForm(workDailyRecordEntity, keyValue);

            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            return Success("操作成功。");
        }

    }
    
}
