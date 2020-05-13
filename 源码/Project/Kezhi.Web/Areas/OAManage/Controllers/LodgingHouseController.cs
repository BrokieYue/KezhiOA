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
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Kezhi.Web.Areas.OAManage.Controllers
{
    public class LodgingHouseController : ControllerBase
    {

        private bool ImportResult = false;
        private LodgingHouseApp app = new LodgingHouseApp();
        private UserApp userApp = new UserApp();

        /// <summary>
        /// 分页查询宿舍信息
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        /// <summary>
        /// 根据ID查询信息
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ActionResult GetFormJson(string keyword)
        {
            var data = app.GetFormById(keyword);
            return Content(data.ToJson());
        }
        /// <summary>
        /// 查询全部宿舍
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetHouses()
        {
            List<LodgingHouseEntity> list = app.GetLists(null);
           
            LodgingHouseEntity entity = new LodgingHouseEntity();
            entity.F_Id = "0001";
            entity.F_HouseName = "家";

            LodgingHouseEntity entity1 = new LodgingHouseEntity();
            entity1.F_Id = "0002";
            entity1.F_HouseName = "宾馆";

            list.Add(entity);
            list.Add(entity1);
            return Content(list.ToJson());
        }
        /// <summary>
        /// 添加宿舍
        /// </summary>
        /// <param name="lodgingHouseEntity"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(LodgingHouseEntity lodgingHouseEntity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                string houseCode = lodgingHouseEntity.F_HouseCode;
                if (!string.IsNullOrEmpty(houseCode))
                {
                    List<LodgingHouseEntity> entity = app.GetEntityByCode(houseCode);
                    if (entity != null && entity.Count > 0)
                    {
                        return Error("该宿舍已存在！");
                    }
                }
            }

            app.SubmitForm(lodgingHouseEntity, keyValue);
            return Success("操作成功。");
        }

        /// <summary>
        /// 删除宿舍信息
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult DeleteHouse(string keyValue)
        {
            app.Delete(keyValue);
            return Success("删除成功。");
        }
        /// <summary>
        /// 下载模板
        /// </summary>
        /// <returns></returns>
        public ActionResult Excel()
        {

            DataTable table = new DataTable();
            try
            {
                return File(KezhiExcel.TemplateExcelToMs("上海科致宿舍信息模板.xls"), "application/vnd.ms-excel", "上海科致宿舍信息模板.xls");
            }
            catch (Exception ex)
            {
                return Error("下载出错，请重新下载");
            }

        }

        /// <summary>
        /// 展示上传的信息
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
            //从第三行开始取数据，前两行为表头
            for (var i = 2; i < data.Rows.Count; i++)
            {
                string houseCode = data.Rows[i]["F2"].ToString();
                if (string.IsNullOrEmpty(houseCode))
                {
                    continue;
                }
                newdata.ImportRow(data.Rows[i]);
            }
            return Content(newdata.ToJson());
        }


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
        /// 宿舍信息批量导入
        /// </summary>
        private string ImportData(string fileName)
        {
            string ErrorInfo = string.Empty;
            string file = Request.MapPath("../../Files/Temp/") + fileName;

            List<string> housCodes = new List<string>();

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
                List<LodgingHouseEntity> list = new List<LodgingHouseEntity>();
                var LoginInfo = OperatorProvider.Provider.GetCurrent();
     
                //遍历行数据
     
                for (int i = 2; i <= dt.Rows.Count - 1; i++)
                {

                    LodgingHouseEntity model = new LodgingHouseEntity();
                    //宿舍编号
                    string houseCode = dt.Rows[i][1].ToString();
                   
                    //宿舍名称
                    string houseName = CommonUtil.ToStr(dt.Rows[i][2]);
                    if (string.IsNullOrEmpty(houseCode) && string.IsNullOrEmpty(houseName))
                    {
                        break;
                    }
                    else if (string.IsNullOrEmpty(houseCode))
                    {
                        return "宿舍编号不能为空";

                     }
                    else if (string.IsNullOrEmpty(houseName))
                    {
                        return "宿舍名称不能为空";
                    }

                    //新增
                    model.F_Id = Guid.NewGuid().ToString();
                    model.F_HouseCode = houseCode;
                    model.F_HouseName = houseName;
                    model.F_Description = dt.Rows[i][3].ToString();
                    model.F_EnabledMark = true;
                    model.F_DeleteMark = false;
                    model.F_CreatorTime = DateTime.Now;
                    model.F_CreatorUserId = LoginInfo.UserId;
              
                   
                    list.Add(model);
                    housCodes.Add(houseCode);
           
                }
                app.insertList(list);
                //重复导入后去重
                app.DeleteDuplicate(housCodes);
                if (ErrorInfo == "")
                {
                    ImportResult = true;

                    logEntity.F_Account = Kezhi.Code.OperatorProvider.Provider.GetCurrent().UserCode;
                    logEntity.F_NickName = Kezhi.Code.OperatorProvider.Provider.GetCurrent().UserName;
                    logEntity.F_Result = true;
                    logEntity.F_Description = "宿舍信息Excel导入成功";
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
        /// 导出数据
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="projectCode"></param>
        /// <returns></returns>
        public ActionResult ExcelDailyRecord(string keyword, string projectCode)
        {

            List<LodgingHouseEntity> list = app.GetLists(keyword);
            if (list.Count < 1)
            {
                return Error("没有需要导出的数据");
            }
            DataTable dataTable = KezhiExcel.ListToDataTable(list, true);
            for (var i = 0; i < dataTable.Rows.Count; i++)
            {

                dataTable.Rows[i]["F_Id"] = i.ToString();
            }
            HouseRecord(dataTable);
            return File(KezhiExcel.DataTableToMS(dataTable, "HouseMessage", ""), "application/vnd.ms-excel", "上海科致宿舍信息" + DateTime.Now.ToShortDateString().ToString() + ".xls");
        }
        public void HouseRecord(DataTable dataTable)
        {
            //删除多与的列
 
            dataTable.Columns.Remove("F_EnabledMark");
            dataTable.Columns.Remove("F_DeleteMark");
            dataTable.Columns.Remove("F_LastModifyTime");
            dataTable.Columns.Remove("F_LastModifyUserId");
            dataTable.Columns.Remove("F_DeleteTime");
            dataTable.Columns.Remove("F_DeleteUserId");
            dataTable.Columns.Remove("F_CreatorTime");
            dataTable.Columns.Remove("F_CreatorUserId");



            //设置列排序
            dataTable.Columns["F_Id"].SetOrdinal(0);
            dataTable.Columns["F_HouseCode"].SetOrdinal(1);
            dataTable.Columns["F_HouseName"].SetOrdinal(2);
            dataTable.Columns["F_Description"].SetOrdinal(3);
        }
        public ActionResult Import()
        {
            return View();
        }
    }
}