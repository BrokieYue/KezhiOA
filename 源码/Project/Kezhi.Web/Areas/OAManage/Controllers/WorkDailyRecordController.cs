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
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kezhi.Web.Areas.OAManage.Controllers
{

    public class WorkDailyRecordController : ControllerBase
    {
        private bool ImportResult = false;
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
        public ActionResult GetGridJson(Pagination pagination, string keyword, DateTime? startTime, DateTime? endTime, string organize, string filiale,string weekflag)
        {
            if (weekflag != null && weekflag.Equals("true"))
            {
                startTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1);
                endTime = Common.GetMonthStartTime(DateTime.Now.Date).AddMinutes(-1);
            }
            //设置项目（非项目实施的，项目编号为工作类型）
            //ItemsEntity itenEntity = itemApp.GetItemByFullName("其他工作");
            //List<ItemsDetailEntity> itemList = itemsDetailApp.GetList(itenEntity.F_Id, "");
            //string category = "项目实施";
            //if (itemList.Count > 0)
            //{
            //    category = itemList[0].F_ItemName;
            //}
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
           
            List<V_WorkDailyRecordEntity> list = workDailyRecordApp.GetList(pagination, keyword, startTime, endTime, organize, filiale);
            foreach (var entity in list)
            {
                if (entity.F_WorkAddress.Equals("其他"))
                {
                    entity.F_WorkAddress = entity.F_OtherAddress;
                }
                //if (!string.IsNullOrEmpty(entity.F_WorkCategory) && !entity.F_WorkCategory.Equals(category))
                //{
                //    entity.F_ProjectCode = entity.F_ProjectId;
                //    entity.F_ProjectName = entity.F_ProjectId;
                //}
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
        /// 更新工作日志
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetUpdateFormJson(string keyValue)
        {
            var data = workDailyRecordApp.GetForm(keyValue);
            V_WorkDailyRecordEntityUpdate entity = new V_WorkDailyRecordEntityUpdate();
            entity.F_Id = data.F_Id;
            entity.F_CreatType = data.F_CreatType;
            entity.F_DailyRecord = data.F_DailyRecord;
            entity.F_Description = data.F_Description;
            entity.F_IfLocal = data.F_IfLocal;
            entity.F_ItemName = data.F_ItemName;
            entity.F_OtherAddress = data.F_OtherAddress;
            entity.F_ProjectCode = data.F_ProjectCode;
            entity.F_ProjectId = data.F_ProjectId;
            entity.F_ProjectName = data.F_ProjectName;
            entity.F_WorkAddressCode = data.F_WorkAddressCode;
            entity.F_WorkCategory = data.F_WorkCategory;
            entity.F_WorkDate = data.F_WorkDate;
            entity.F_WorkTimeEnd = data.F_WorkTimeEnd;
            entity.F_WorkTimeStart = data.F_WorkTimeStart;
            entity.F_WorkUserId = data.F_WorkUserId;
            entity.F_WorkUserName = data.F_WorkUserName;
            entity.F_WorkType = data.F_WorkType;
            entity.F_WorkAddressFirst = data.F_WorkAddress;
            entity.F_WorkSubsidy = data.F_WorkSubsidy;
            entity.F_LodgingHouse = data.F_LodgingHouse;

            return Content(entity.ToJson());
        }
        /// <summary>
        /// 日志对象新增修改提交
        /// </summary>
        /// <param name="workDailyRecordEntity"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitFormDailyRecord(WorkDailyRecordEntity workDailyRecordEntity, string keyValue)
        {
            #region 去空格(前台问题，暂时找不到解决办法，由后台处理)
            string desc = workDailyRecordEntity.F_Description;
            if (desc.Equals("&nbsp"))
            {
                workDailyRecordEntity.F_Description = "";
            }
            string otherAdd = workDailyRecordEntity.F_OtherAddress;
            if (otherAdd.Equals("&nbsp"))
            {
                workDailyRecordEntity.F_OtherAddress = "";
            }
            #endregion
            //项目id管理
            bool flag = true;
            //ItemsEntity itemEntity = itemApp.GetItemByFullName("其他工作");
            //List<ItemsDetailEntity> list = itemsDetailApp.GetList(itemEntity.F_Id, "");
            string projectId = workDailyRecordEntity.F_ProjectId;
            //foreach (ItemsDetailEntity item in list)
            //{
            //    if (item.F_ItemName.Equals(projectId))
            //    {
            //        flag = false;
            //    }
            //}
            //if (flag)
            //{
            //    ProjectEntity project = projectApp.GetForm(projectId);
            //    if (project == null && !projectId.Equals("&nbsp") && !string.IsNullOrEmpty(projectId))
            //    {
            //        return Error("当前项目不存在，请联系管理员添加项目");
            //    } 
            //}
            ProjectEntity project = projectApp.GetForm(projectId);
            if (project == null || string.IsNullOrEmpty(project.F_Id))
            {
                return Error("当前项目不存在，请联系管理员添加项目");
            } 
            //check工作日期
            string workDateStr = workDailyRecordEntity.F_WorkDate.ToString();
            DateTime workDate;
            DateTime oldDate;
            DateTime.TryParse(workDateStr, out workDate);
            DateTime.TryParse("2010-01-01", out oldDate);
            string worktype = "双休,调休,法定假日";
            if (workDate > DateTime.Now.Date)
            {
                return Error("无法创建未工作的日志");
            }
            
            if(workDate < oldDate){
                return Error("创建日期距离当前时间太久，无法创建");
            }
           
            //工作描述
            workDailyRecordEntity.F_DailyRecord = workDailyRecordEntity.F_DailyRecord.ToString().Trim();
           
            if (workDailyRecordEntity.F_WorkAddress.Equals("在家休息") || worktype.Contains(workDailyRecordEntity.F_WorkType))
            {
                workDailyRecordEntity.F_WorkTimeStart = "\\";
                workDailyRecordEntity.F_WorkTimeEnd = "\\";
            }
            //设置项目（非项目实施的，项目编号为工作类型）
            //ItemsEntity entity = itemApp.GetItemByFullName("其他工作");
            //List<ItemsDetailEntity> itemList = itemsDetailApp.GetList(entity.F_Id, "");
            //string category = "项目实施";
            //if (itemList.Count > 0)
            //{
            //    category = itemList[0].F_ItemName;
            //}
            //if (!workDailyRecordEntity.F_WorkCategory.Equals(category))
            //{
            //    workDailyRecordEntity.F_ProjectId = workDailyRecordEntity.F_WorkCategory;
            //}
           //添加日志
            if (string.IsNullOrEmpty(keyValue) || "".Equals(keyValue))
            {
                //外场说明设置
                workDailyRecordEntity = SetOutfield(workDailyRecordEntity);
                workDailyRecordEntity.F_CreatType = false;
                workDailyRecordEntity.F_PayHours = "0";
                workDailyRecordEntity.F_RestHours = "0";
                workDailyRecordEntity.F_DeductHours = "0";

                if (workDate == DateTime.Now.Date || worktype.Contains(workDailyRecordEntity.F_WorkType))
                {
                    if (string.IsNullOrEmpty(keyValue) || "".Equals(keyValue))
                    {
                        workDailyRecordEntity.F_CurrentDayMark = true;
                    }

                }
            }
            //津贴设置
            if (workDailyRecordEntity.F_WorkSubsidy == 10)
            {
                workDailyRecordEntity.F_WorkSubsidy = 0;
                workDailyRecordEntity.F_MealSubsidy = 10;
            }
            else
            {
                workDailyRecordEntity.F_MealSubsidy = 0;
            }
            //工作时长设置
            SetWorkedHours(workDailyRecordEntity);
            //1、支付工时 2、考核扣除工时
            string weekDate = CommonUtil.GetWeekByTime(workDailyRecordEntity.F_WorkDate.ToString());
            if (weekDate.Equals("星期六") || weekDate.Equals("星期日") || !workDailyRecordEntity.F_WorkType.Equals("正常"))
            {
                workDailyRecordEntity.F_PayHours = "0";
                workDailyRecordEntity.F_DeductHours = "0";
            }
            else
            {
                workDailyRecordEntity.F_PayHours = "8";
                float span = Convert.ToSingle(workDailyRecordEntity.F_WorkedHours) - 8F;
                workDailyRecordEntity.F_DeductHours = span > 0 ? span.ToString() : "0";
            }
            //可调休工时
            if (workDailyRecordEntity.F_WorkType.Equals("加班"))
            {
                workDailyRecordEntity.F_RestHours = workDailyRecordEntity.F_WorkedHours;
            }
            workDailyRecordApp.SubmitForm(workDailyRecordEntity, keyValue);
            //去重
            DeleteDuplicateDailyRecord();
            
            return Success("操作成功。");
        }
        
        /// <summary>
        /// 设置工作时长
        /// </summary>
        /// <param name="workDailyRecordEntity"></param>
        public void SetWorkedHours(WorkDailyRecordEntity workDailyRecordEntity)
        {
            //出勤工时
            if (workDailyRecordEntity.F_WorkTimeStart.Equals("\\"))
            {
                workDailyRecordEntity.F_WorkedHours = "0";
            }
            else
            {
                float startTime = Common.SetNumberByDateTime(workDailyRecordEntity.F_WorkTimeStart);
                float endTime = Common.SetNumberByDateTime(workDailyRecordEntity.F_WorkTimeEnd);
                //1、上班时间在8:30之前，按8:30计算
                startTime = startTime > 8.5 ? startTime : 8.5F;
                //2、8:30—19:30 工时10H）
                if (startTime == 8.5F && endTime == 19.5F)
                {
                    workDailyRecordEntity.F_WorkedHours = "10";
                }
                else
                if (startTime == 8.5F && endTime < 19.5F && endTime >= 17F)
                {
                    workDailyRecordEntity.F_WorkedHours = "8";
                }
                //3、下班时间在19;30以后到次日8：00之前，工作时长为下班时间（过24点，加24）-上班时间-1H（1H为吃饭时间）
                else if (startTime == 8.5F && (endTime > 19.5F || endTime < 8F))
                {
                    //下班时间过24点且小于8点，加24;超过8点即为新的一天
                    endTime = endTime < 8 ? (endTime + 24F) : endTime;
                    workDailyRecordEntity.F_WorkedHours = (endTime - startTime - 1).ToString();
                }
                //5、对迟到早退现象（加班），工作时长为下班时间-上班时间（过13点，减0.5H，过17点半，减1H吃饭时间）
                else if ((startTime > 8.5F || endTime < 19.5F) &&  workDailyRecordEntity.F_WorkType.Equals("加班"))
                {
                    if (endTime >= 13 && endTime <= 17.5)
                    {
                        if (startTime < 13)
                        {
                            workDailyRecordEntity.F_WorkedHours = (endTime - startTime - 0.5).ToString();
                        }
                        else
                        {
                            workDailyRecordEntity.F_WorkedHours = (endTime - startTime).ToString();
                        }
                    }else if(endTime >= 19.5 ){
                        if (startTime < 17.5 && startTime > 13)
                        {
                            workDailyRecordEntity.F_WorkedHours = (endTime - startTime - 0.5).ToString();
                        }
                        else if (startTime < 13)
                        {
                            workDailyRecordEntity.F_WorkedHours = (endTime - startTime - 1).ToString();
                        }
                        else
                        {
                            workDailyRecordEntity.F_WorkedHours = (endTime - startTime).ToString();
                        }
                        
                    }else if(endTime >=17.5 && endTime < 19.5){//这是个特殊的时间段，很坑
                        if (startTime < 17.5 && startTime > 13)
                        {
                            workDailyRecordEntity.F_WorkedHours = (endTime - startTime - 0.5).ToString();
                        }
                        else if (startTime < 13)
                        {
                            float hours = (endTime - startTime - 1) > 8 ? 8:(endTime - startTime - 1);
                            workDailyRecordEntity.F_WorkedHours = hours.ToString();
                        }
                        else
                        {
                            workDailyRecordEntity.F_WorkedHours = (endTime - startTime).ToString();
                        }
                    }
                    else
                    {
                        float span = endTime - startTime;
                        if (span >= 8)
                        {
                            workDailyRecordEntity.F_WorkedHours = "8";
                        }
                        else
                        {
                            workDailyRecordEntity.F_WorkedHours = (endTime - startTime).ToString();
                        }
                    }
                }
                //6、对于迟到早退（正常上下班），工作时长满8小时且下班时间在19：30之前的，计8小时，其他的为上班时间-下班时间-1H
                else if (startTime > 8.5F || endTime < 19.5F)
                {
                    if (endTime <= 19.5 && endTime - startTime > 8)
                    {
                        workDailyRecordEntity.F_WorkedHours = "8";
                    }
                    else if(endTime >= 13 && endTime <= 17.5)
                    {
                        workDailyRecordEntity.F_WorkedHours = (endTime - startTime - 0.5).ToString();
                    }
                    else if (endTime >= 19.5 )
                    {
                        workDailyRecordEntity.F_WorkedHours = (endTime - startTime - 1).ToString();
                    }
                    else
                    {
                        float span = endTime - startTime;
                        if (span >= 8)
                        {
                            workDailyRecordEntity.F_WorkedHours = "8";
                        }
                        else
                        {
                            workDailyRecordEntity.F_WorkedHours = (endTime - startTime).ToString();
                        }
                        
                    }
                }
            }
        }

        /// <summary>
        /// 外场说明
        /// </summary>
        /// <param name="workDailyRecordEntity"></param>
        public WorkDailyRecordEntity SetOutfield(WorkDailyRecordEntity workDailyRecordEntity)
        {
          
            //工作地点
            var workAddress = workDailyRecordEntity.F_WorkAddress.ToString();
            //用户ID
            var workUserId = workDailyRecordEntity.F_WorkUserId.ToString();
            //子公司ID
            var filialeId = userApp.GetForm(workUserId).F_FilialeId ;
            //子公司名称
            var filialeName = itemsDetailApp.GetForm(filialeId).F_ItemName.ToString();
            string[] filialeSuffix = {"公司","子公司","办事处"};
            //子公司城市
             var filialeCity = "上海";
            
            for(var i = 0;i < filialeSuffix.Length;i++){
                if(filialeName.Contains(filialeSuffix[i])){
                     filialeCity = filialeName.Replace(filialeSuffix[i],"");
                }
                
            }
            //如果子公司名称与工作地点相同，外场L01（工作地点的选择是在子公司和项目地中选择）（子公司代表入职地）
            //如果工作地点包含子公司所在城市，外场L02（在子公司所在城市上班，但是不在公司办公室上班的，外场为L02）
            //否则，外场L03
            if (workAddress.Equals(filialeName))
            {
                workDailyRecordEntity.F_WorkAddressCode = "L01";
            }
            else if(workAddress.Contains(filialeCity)){
                workDailyRecordEntity.F_WorkAddressCode = "L02";
            }
            else
            {
                workDailyRecordEntity.F_WorkAddressCode = "L03";
            }
            return workDailyRecordEntity;

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
        /// 根据文件名获取数据预览
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetUploadGridJson(string fileName)
        {
            string file = Request.MapPath("../../Files/Temp/") + fileName;
            var data = KezhiExcel.GetExcelDataTable(file);
            //给dataTable添加列：“F10”
            if (!data.Columns.Contains("F10"))
            {
                data.Columns.Add("F10", typeof(string));
            }
            var newdata = data.Clone();
            var row = data.Rows[2];
            var user = row.ItemArray[7];
            //从第四行开始获取数据（前三行表头）
            for (var i = 3; i < data.Rows.Count; i++)
            {
                
                if (data.Rows[i]["F1"].ToString().Equals("") && data.Rows[i]["F2"].ToString().Equals(""))
                {
                    continue;
                }
                //取出姓名，用于页面展示
                data.Rows[i]["F10"] = user;
                newdata.ImportRow(data.Rows[i]);
            }

           return Content(newdata.ToJson());
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
        /// 导出日志
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="organize"></param>
        /// <param name="filiale"></param>
        /// <returns></returns>
        public ActionResult ExcelDailyRecord(string keyword, DateTime? startTime, DateTime? endTime, string organize, string filiale,string weekflag)
        {
            if (weekflag != null && weekflag.Equals("true"))
            {
                startTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(-1); 
                endTime = Common.GetMonthStartTime(DateTime.Now.Date).AddMinutes(-1);
            }
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            if (!"admin".Equals(LoginInfo.UserCode) && getRole(LoginInfo.UserName).F_FullName.Equals("员工"))
            {
                keyword = LoginInfo.UserName;
            }
            else if ("admin".Equals(LoginInfo.UserCode))
            {
                keyword = null;
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
            List<V_WorkDailyRecordEntity> list = workDailyRecordApp.GetListNoPage(keyword, startTime, endTime, organize, filiale);
            if (list.Count < 1)
            {
                return Error("没有需要导出的数据");
            }
            DataTable dataTable = KezhiExcel.ListToDataTable(list, true);
            //添加星期列
            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                string date = dataTable.Rows[i]["F_WorkDate"].ToString();
                //如果日期为空，不添加星期和日期
                if (date.Equals(""))
                {
                    continue;
                }
                dataTable.Rows[i]["F_WorkDate"] = CommonUtil.ConvertDate(date).ToString("yyyy-MM-dd");
                if (dataTable.Rows[i]["F_WorkAddress"].Equals("其他"))
                {
                    dataTable.Rows[i]["F_WorkAddress"] = dataTable.Rows[i]["F_OtherAddress"];
                    
                }
                if(string.IsNullOrEmpty(dataTable.Rows[i]["F_ProjectCode"].ToString()))
                {

                    dataTable.Rows[i]["F_ProjectCode"] = dataTable.Rows[i]["F_WorkCategory"]; 
                }
            }
            GetWorkDailyRecord(dataTable);
     

            
            return File(KezhiExcel.DataTableToMS(dataTable, "WorkDailyRecord", ""), "application/vnd.ms-excel", "上海科致工作日志" + DateTime.Now.ToShortDateString().ToString() + ".xls");
        }
      
        /// <summary>
        /// 工作日志：删除多与的列，并重排序
        /// </summary>
        /// <param name="dataTable"></param>
        private static void GetWorkDailyRecord(DataTable dataTable)
        {
            //删除多与的列
            dataTable.Columns.Remove("F_Id");
            dataTable.Columns.Remove("F_WorkUserId");
            dataTable.Columns.Remove("F_ProjectId");
            dataTable.Columns.Remove("F_IfLocal");
            dataTable.Columns.Remove("F_WorkType");
            dataTable.Columns.Remove("F_WorkAddressCode");
            dataTable.Columns.Remove("F_PayHours");
            dataTable.Columns.Remove("F_RestHours");
            dataTable.Columns.Remove("F_DeductHours");
            dataTable.Columns.Remove("F_MealSubsidy");
            dataTable.Columns.Remove("F_CreatType");
            dataTable.Columns.Remove("F_EnabledMark");
            dataTable.Columns.Remove("F_DeleteMark");
            dataTable.Columns.Remove("F_CreatorTime");
            dataTable.Columns.Remove("F_CreatorUserId");
            dataTable.Columns.Remove("F_LastModifyTime");
            dataTable.Columns.Remove("F_LastModifyUserId");
            dataTable.Columns.Remove("F_DeleteTime");
            dataTable.Columns.Remove("F_DeleteUserId");
            dataTable.Columns.Remove("F_OrganizeName");
            dataTable.Columns.Remove("F_RoleName");
            dataTable.Columns.Remove("F_CreateorUserName");
            dataTable.Columns.Remove("F_LastModifyUserName");
            dataTable.Columns.Remove("F_DeleteUserName");
            dataTable.Columns.Remove("F_ItemId");
            dataTable.Columns.Remove("F_ProjectManagerName");
            dataTable.Columns.Remove("F_ProjectName");
            dataTable.Columns.Remove("F_ItemName");
            dataTable.Columns.Remove("F_WorkCategory");
            dataTable.Columns.Remove("F_OtherAddress");
            dataTable.Columns.Remove("F_CurrentDayMark");


            //设置列排序
            dataTable.Columns["F_ProjectCode"].SetOrdinal(1);
            dataTable.Columns["F_WorkUserName"].SetOrdinal(3);
            dataTable.Columns["F_WorkDate"].SetOrdinal(0);
            dataTable.Columns["F_WorkAddress"].SetOrdinal(2);
            dataTable.Columns["F_DailyRecord"].SetOrdinal(4);
            dataTable.Columns["F_Description"].SetOrdinal(5);
            dataTable.Columns["F_WorkTimeStart"].SetOrdinal(6);
            dataTable.Columns["F_WorkTimeEnd"].SetOrdinal(7);
            dataTable.Columns["F_WorkedHours"].SetOrdinal(8);
            dataTable.Columns["F_WorkSubsidy"].SetOrdinal(9);
        }
        /// <summary>
        /// 根据文件名获取文件数据并导入数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult UploadExcelData(string fileName)
        {
            string filePath = Request.MapPath("../../Files/Temp/") + fileName;
          
            string strMessage = ImportData(fileName);
            if (ImportResult)
            {
                FileHelper.RemoveFile(filePath);
                return Success(strMessage);
            }
            else
            {
                //数据检查失败，删除临时文件
                FileHelper.RemoveFile(filePath);
                return Error(strMessage);
            }
        }
       
        /// <summary>
        /// 工作日志批量导入
        /// </summary>
        private string ImportData(string fileName)
        {
            string ErrorInfo = string.Empty;
            string file = Request.MapPath("../../Files/Temp/") + fileName;

            List<string> nameList = new List<string>();

            LogEntity logEntity = new LogEntity();
            logEntity.F_ModuleName = "工作日志Excel导入";
            logEntity.F_Type = DbLogType.Import.ToString();

            int TotalCount = 0;
            
            try
            {
                if (!System.IO.File.Exists(file))
                {
                    return "文件已失效，请重新选择文件";
                }
                DataTable dt = KezhiExcel.GetExcelDataTable(file);
                List<WorkDailyRecordEntity> list = new List<WorkDailyRecordEntity>();
                var LoginInfo = OperatorProvider.Provider.GetCurrent();
                ////获取其他工作
                //ItemsEntity entity = itemApp.GetItemByFullName("其他工作");
                //List<ItemsDetailEntity> itemList = itemsDetailApp.GetList(entity.F_Id, "");
                //遍历行数据
                var row = dt.Rows[2];
                var user = row.ItemArray[7];
                for (int i = 3; i <= dt.Rows.Count - 1; i++)
                {
                    if (dt.Rows[i]["F1"].ToString().Equals("") && dt.Rows[i]["F2"].ToString().Equals(""))
                    {
                        continue;
                    }
                    WorkDailyRecordEntity model = new WorkDailyRecordEntity();
                   

                    string st_username = user.ToString();
                    if (string.IsNullOrEmpty(st_username))
                    {
                        return "姓名不能为空";
                    }

                    string st_userid = CommonUtil.getUserId(st_username);
                    if (string.IsNullOrEmpty(st_userid))
                    {
                        return "用户：" + st_username + "不存在";
                    }
                    DateTime dt_workdate = ToDate(dt.Rows[i][0]);
                    if (dt_workdate == null)
                    {
                        return "工作日期不能为空";
                    }
                    if (dt_workdate > DateTime.Now.Date)
                    {
                        return "无法导入未工作的日志，日期：" + dt_workdate.ToString("yyyy-MM-dd") + "超过了本地当前时间";
                    }
                    DateTime oldDate;
                    DateTime.TryParse("2010-01-01", out oldDate);
                    if (dt_workdate < oldDate)
                    {
                        return "工作日期(" + dt_workdate.ToString("yyyy-MM-dd") + ")距离当前时间太久，无法创建";
                    }
                    string st_projectcode = CommonUtil.ToStr(dt.Rows[i][4]);
                    string st_projectid = "";
                    string st_projectname = "";
                    string st_category = "";
                    ProjectEntity project = null;
                    project = CommonUtil.getProjectid(st_projectcode);

                    if (project == null)
                    {
                       return "编号为：" + st_projectcode + "的项目不存在";
                    }
                    else
                    {
                        st_projectname = project.F_ProjectName;
                        st_projectid = project.F_Id;
                        ItemsDetailEntity item = itemsDetailApp.GetItemsByProjectType(project.F_ProjectType);
                        if (item != null)
                        {
                            st_category = item.F_Id;
                        }
                      
                    }
                    string st_workaddress = CommonUtil.ToStr(dt.Rows[i][2]);
                    if (string.IsNullOrEmpty(st_workaddress))
                    {
                        return "工作地点不能为空";
                    }

                    string st_DailyRecord = CommonUtil.ToStr(dt.Rows[i][3]);
                    if (string.IsNullOrEmpty(st_DailyRecord))
                    {
                        return "工作内容不能为空";
                    }
                    string workDate = CommonUtil.ToStr(dt.Rows[i][6]);
                    if (string.IsNullOrEmpty(workDate))
                    {
                        ErrorInfo = "上下班时间不能为空";
                        return ErrorInfo;
                    }
                    string st_worktimestart = "";
                    string st_worktimeend = "";
                    string[] array = getWorkDate(workDate);
                    if (array != null)
                    {
                        st_worktimestart = array[0];
                        st_worktimeend = array[1];
                    }else{
                        return "上下班时间格式不正确";
                    }
                    string st_Description = CommonUtil.ToStr(dt.Rows[i][5]);
                    string st_worktype = "正常";
                    if (st_Description.Contains("加班"))
                    {
                        st_worktype = "加班";
                    }
                    else if (st_Description.Contains("调休"))
                    {
                        st_worktype = "调休";
                    }
                    else if (st_Description.Contains("周末") || st_Description.Contains("周六") || st_Description.Contains("双休") || st_Description.Contains("休息"))
                    {
                        st_worktype = "双休";
                    }
                    string st_workhours = CommonUtil.ToStr(dt.Rows[i][7]);
                    if (string.IsNullOrEmpty(st_workhours) || !Common.CheckIsNumByString(st_workhours))
                    {
                        return "工作时长不能为空且必须是数字";
                    }

                    string st_worksubsidy = CommonUtil.ToStr(dt.Rows[i][8]);
                    if (string.IsNullOrEmpty(st_worksubsidy) || !Common.CheckIsNumByString(st_worksubsidy))
                    {
                        return "津贴不能为空且必须是数字";
                    }

                    //新增
                    model.F_Id = Guid.NewGuid().ToString();
                    model.F_WorkUserId = st_userid;
                    model.F_WorkDate = dt_workdate;
                    model.F_WorkType = st_worktype;
                    model.F_WorkAddress = "其他";
                    model.F_OtherAddress = st_workaddress;
                    model.F_ProjectId = st_projectid;
                    model.F_WorkCategory = st_category;
                    model.F_IfLocal = true;
                    model.F_Description = st_Description;
                    model.F_DailyRecord = st_DailyRecord;
                    model.F_WorkTimeStart = st_worktimestart;
                    model.F_WorkTimeEnd = st_worktimeend;
                    model.F_WorkedHours =st_workhours;
                    model.F_WorkSubsidy = st_worksubsidy.ToIntOrNull();
                    model.F_CreatType = true;
                    model.F_EnabledMark = true;
                    model.F_DeleteMark = false;
                    model.F_CreatorTime = DateTime.Now;
                    model.F_CreatorUserId = LoginInfo.UserId;
                    model.F_PayHours = "0";
                    model.F_RestHours = "0";
                    model.F_DeductHours = "0";
                    model.F_MealSubsidy = 0;
                    model.F_CurrentDayMark = false;
                    //工作时长按照上下班时间计算
                    SetWorkedHours(model);
                    //1、支付工时 2、考核扣除工时
                    string weekDate = CommonUtil.GetWeekByTime(model.F_WorkDate.ToString());
                    if (weekDate.Equals("星期六") || weekDate.Equals("星期日") || !model.F_WorkType.Equals("正常"))
                    {
                        model.F_PayHours = "0";
                        model.F_DeductHours = "0";
                    }
                    else
                    {
                        model.F_PayHours = "8";
                        float span = Convert.ToSingle(model.F_WorkedHours) - 8F;
                        model.F_DeductHours = span > 0 ? span.ToString() : "0";
                    }
                    //可调休工时
                    if (model.F_WorkType.Equals("加班"))
                    {
                        model.F_RestHours = model.F_WorkedHours;
                    }
                    //保存数据
                    if (!LoginInfo.UserCode.Equals("admin"))
                    {
                        string name = getRole(LoginInfo.UserName).F_FullName;
                        if (getRole(LoginInfo.UserName).F_FullName.Equals("员工") && !LoginInfo.UserId.Equals(model.F_WorkUserId))
                        {
                            ErrorInfo = "请导入自己的工作日志";
                            return ErrorInfo;
                        }
                    }
                    list.Add(model);
                    //workDailyRecordApp.SubmitForm(model, "");
                    TotalCount++;
                    if (!nameList.Contains(model.F_WorkUserId))
                    {
                        nameList.Add(model.F_WorkUserId);
                    }
                }
                workDailyRecordApp.InsertList(list);
                //员工导入，只去重自己的；管理员导入，去重全部的
                workDailyRecordApp.DeleteDuplicate(nameList);
                if (ErrorInfo == "")
                {
                    ImportResult = true;

                    logEntity.F_Account = Kezhi.Code.OperatorProvider.Provider.GetCurrent().UserCode;
                    logEntity.F_NickName = Kezhi.Code.OperatorProvider.Provider.GetCurrent().UserName;
                    logEntity.F_Result = true;
                    logEntity.F_Description = "工作日志Excel导入成功";
                    new LogApp().WriteDbLog(logEntity);

                    ErrorInfo = "共成功导入" + TotalCount.ToString() + "条数据！";
                }
                return ErrorInfo;
            }
            catch (Exception ee)
            {
                ImportResult = false;
                ErrorInfo = "导入错误，请重试或者联系管理员！";
                return ErrorInfo;
            }
        }
        /// <summary>
        /// 获取地址
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
        [HttpPost]
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
        /// 根据上下班时间截取上班时间和下班时间
        /// </summary>
        /// <param name="workDate"></param>
        /// <returns></returns>
        public String[] getWorkDate(string workDate)
        {
            if (workDate.Equals("\\"))
            {
                var arr = new String[2];
                arr[0] = "\\";
                arr[1] = "\\";
                return arr;
            }
            try
            {
                if (workDate.Contains("："))
                {
                    workDate = workDate.Replace("：", ":");
                }
                if (workDate.Contains("_"))
                {
                    return workDate.Split('_');
                }
                else if (workDate.Contains("-"))
                {
                    return workDate.Split('-');
                }
                else if (workDate.Contains("—"))
                {
                    return workDate.Split('—');
                }
                else if (workDate.Contains("~"))
                {
                    return workDate.Split('~');
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        /// <summary>
        /// 日志去重
        /// </summary>
        public void DeleteDuplicateDailyRecord()
        {
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            //员工导入，只去重自己的；管理员导入，去重全部的
            List<string> list = new List<string>();
            list.Add(LoginInfo.UserId);
            workDailyRecordApp.DeleteDuplicate(list);

        }
        /// <summary>
        /// 插入前验证
        /// </summary>
        /// <param name="workdate"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public bool GetWorkDaily(string workdate)
        {
           return workDailyRecordApp.GetWorkDailyByWorkDate(workdate);
        }

        public ActionResult UpdateForm()
        {
            return View();
        }
        #region 获取其他对象方法
       

       
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
        /// 地点列表
        /// </summary>
        /// <returns></returns>
        
        #endregion


        #region 工具方法

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
            DateTime dt;
            
            if (v is System.DBNull || v == null)
            {
                return DateTime.Now;
            }
            else
            {
                DateTime.TryParse(v.ToString(), out dt);
                return dt;
            }
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
        #endregion

    }
    
}
