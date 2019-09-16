using Kezhi.Application;
using Kezhi.Application.OAManage;
using Kezhi.Application.SystemManage;
using Kezhi.Application.SystemSecurity;
using Kezhi.Code;
using Kezhi.Code.Excel;
using Kezhi.Domain.Entity.OA;
using Kezhi.Domain.Entity.SystemManage;
using Kezhi.Domain.Entity.SystemSecurity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Mvc;

namespace Kezhi.Web.Areas.OAManage.Controllers
{
    public class ProjectController : ControllerBase
    {
        private ProjectApp projectApp = new ProjectApp();
        private AreaApp areaApp = new AreaApp();
        private UserApp userApp = new UserApp();
        private bool ImportResult = false;
        
        /// <summary>
        /// 界面模糊查询功能
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword,string projectStatus)
        {
            var data = new
            {
                rows = projectApp.GetList(pagination, keyword, projectStatus),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        /// <summary>
        /// 地点列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetAddressSelectJson(string layers, string parentid)
        {
            var data = areaApp.GetListGrading(layers, parentid);
            return Content(data.ToJson());
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
        /// 编辑界面根据主键获取项目对象
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = projectApp.GetFormById(keyValue);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 对象新增修改提交
        /// </summary>
        /// <param name="projectEntity"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize(true, @"/SystemManage/ItemsData/Form")]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ProjectEntity projectEntity, string keyValue, string provence,string city)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                string projectCode = projectEntity.F_ProjectCode;
                ProjectEntity entity = projectApp.GetProject(projectCode);
                if (entity != null && !string.IsNullOrEmpty(entity.F_Id))
                {
                    return Error("该项目已存在！");
                }
            }
            if (!string.IsNullOrEmpty(provence) && !provence.Equals("==省级行政区=="))
            {
                projectEntity.F_ProjectAddress = provence + city;
            }
            projectApp.SubmitForm(projectEntity, keyValue);
            return Success("操作成功。");
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
            projectApp.DeleteForm(keyValue);
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
            var newdata = data.Clone();
            for (var i = 2; i < data.Rows.Count; i++)
            {
                newdata.ImportRow(data.Rows[i]);
            }
            return Content(newdata.ToJson());
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
                RemoveFile(filePath);
                return Success(strMessage);
            }
            else
            {
                //数据检查失败，删除临时文件
                RemoveFile(filePath);
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
            var LoginInfo = OperatorProvider.Provider.GetCurrent();
            List<ProjectEntity> list = new List<ProjectEntity>();

            LogEntity logEntity = new LogEntity();
            logEntity.F_ModuleName = "项目信息导入";
            logEntity.F_Type = DbLogType.Import.ToString();

            int TotalCount = 0;
            
            try
            {
                if (!System.IO.File.Exists(file))
                {
                    return "文件已失效，请重新选择文件";
                }
                DataTable dt = KezhiExcel.GetExcelDataTable(file);
                //遍历行数据
                for (int i = 2; i < dt.Rows.Count; i++)
                {

                    ProjectEntity model = new ProjectEntity();
                    string st_projectCode = ToStr(dt.Rows[i][1]);
                    if (string.IsNullOrEmpty(st_projectCode))
                    {
                        return "项目编号不能为空";
                    }

                    string st_projectName = ToStr(dt.Rows[i][2]);
                    if (string.IsNullOrEmpty(st_projectName))
                    {
                        return "项目名称不能为空";
                    }

                    string st_clientContract = ToStr(dt.Rows[i][3]);
                    string st_clientName = ToStr(dt.Rows[i][4]);
                    string st_projectAddress = ToStr(dt.Rows[i][5]);
                    if (string.IsNullOrEmpty(st_projectAddress))
                    {
                        return "项目地点不能为空";
                    }
                    string st_projectType = ToStr(dt.Rows[i][6]);
                    string st_projectManager = ToStr(dt.Rows[i][7]);
                    if (!string.IsNullOrEmpty(st_projectManager))
                    {
                        UserEntity user = userApp.GetUser(st_projectManager);
                        if (user != null && !string.IsNullOrEmpty(user.F_Id))
                        {
                            st_projectManager = user.F_Id;
                        }
                        else
                        {
                            return "项目经理：" + st_projectManager + ",不是公司员工，请先添加！";
                        }
                    }
                    string st_projectStatus = ToStr(dt.Rows[i][8]);
                    string st_projectDescription = ToStr(dt.Rows[i][9]);

                    //新增
                    model.F_Id = Guid.NewGuid().ToString();
                    model.F_ClientContractNO = st_clientContract;
                    model.F_Description = st_projectDescription;
                    model.F_ProjectAddress = st_projectAddress;
                    model.F_ProjectClient = st_clientName;
                    model.F_ProjectCode = st_projectCode;
                    model.F_ProjectManagerId = st_projectManager;
                    model.F_ProjectName = st_projectName;
                    model.F_ProjectStatus = st_projectStatus;
                    model.F_ProjectType = st_projectType;
                    model.F_CreatorUserId = LoginInfo.UserId;
                    model.F_CreatorTime = DateTime.Now;

                    list.Add(model);
                    TotalCount++;
                 
                }
                projectApp.InsertList(list);
                //项目去重
                projectApp.DeleteDuplicate();
                if (ErrorInfo == "")
                {
                    ImportResult = true;

                    logEntity.F_Account = Kezhi.Code.OperatorProvider.Provider.GetCurrent().UserCode;
                    logEntity.F_NickName = Kezhi.Code.OperatorProvider.Provider.GetCurrent().UserName;
                    logEntity.F_Result = true;
                    logEntity.F_Description = "项目信息导入成功";
                    new LogApp().WriteDbLog(logEntity);

                    ErrorInfo = "共成功导入" + TotalCount.ToString() + "条数据！";
                }
                return ErrorInfo;
            }
            catch (Exception ee)
            {
                ImportResult = false;
                ErrorInfo = ee.Message;
                return ErrorInfo;
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
        public ActionResult ExcelDailyRecord(string keyword, string projectStatus)
        {

            List<V_ProjectEntity> list = projectApp.GetLists(keyword, projectStatus);
            if (list.Count < 1)
            {
                return Error("没有需要导出的数据");
            }
            DataTable dataTable = KezhiExcel.ListToDataTable(list, true);
            ProjectRecord(dataTable);
            return File(KezhiExcel.DataTableToMS(dataTable, "ProjectMessage", ""), "application/vnd.ms-excel", "上海科致项目信息" + DateTime.Now.ToShortDateString().ToString() + ".xls");
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
                return File(KezhiExcel.TemplateExcelToMs("上海科致项目模板.xls"), "application/vnd.ms-excel", "上海科致项目模板.xls");
            }
            catch (Exception ex)
            {
                return Error("下载出错，请重新下载");
            }

        }
        /// <summary>
        /// 删除多余的列并重排序
        /// </summary>
        /// <param name="dt"></param>
        public void ProjectRecord(DataTable dataTable)
        {
            //删除多与的列
            dataTable.Columns.Remove("F_Id");
            dataTable.Columns.Remove("F_ProjectProvence");
            dataTable.Columns.Remove("F_ProjectCity");
            dataTable.Columns.Remove("F_ProjectManagerId");
            dataTable.Columns.Remove("F_DeleteMark");
            dataTable.Columns.Remove("F_EnabledMark");
            dataTable.Columns.Remove("F_LastModifyTime");
            dataTable.Columns.Remove("F_LastModifyUserId");
            dataTable.Columns.Remove("F_DeleteTime");
            dataTable.Columns.Remove("F_DeleteUserId");
            dataTable.Columns.Remove("F_CreateorUserName");
            dataTable.Columns.Remove("F_LastModifyUserName");
            dataTable.Columns.Remove("F_DeleteUserName");
            dataTable.Columns.Remove("F_CreatorTime");
            dataTable.Columns.Remove("F_CreatorUserId");


            //设置列排序
            dataTable.Columns["F_ProjectCode"].SetOrdinal(0);
            dataTable.Columns["F_ProjectName"].SetOrdinal(1);
            dataTable.Columns["F_ClientContractNO"].SetOrdinal(2);
            dataTable.Columns["F_ProjectClient"].SetOrdinal(3);
            dataTable.Columns["F_ProjectAddress"].SetOrdinal(5);
            dataTable.Columns["F_ProjectManagerName"].SetOrdinal(6);
            dataTable.Columns["F_ProjectType"].SetOrdinal(4);
            dataTable.Columns["F_ProjectStatus"].SetOrdinal(7);
            dataTable.Columns["F_ProjectTimeStart"].SetOrdinal(8);
            dataTable.Columns["F_ProjectTimeEnd"].SetOrdinal(9);
            dataTable.Columns["F_Description"].SetOrdinal(10);
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// 
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

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult GetProjectJson()
        {
            string[] status = new string[] { "已关闭", "已结束"};
            var data = projectApp.GetListByStatus(status);
            return Content(data.ToJson());
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
        [HttpGet]
        public ActionResult Info()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Import()
        {
            return View();
        }
    }
}
