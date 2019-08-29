using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kezhi.Domain.Entity.SystemManage;
using Kezhi.Application.SystemManage;
using Kezhi.Code;
using Kezhi.Code.Excel;
//using Kezhi.Domain.Entity.BomManage;
//using Kezhi.Application.BomManage;

namespace Kezhi.Web.Areas.SystemManage.Controllers
{
    public class UploadExcelController : ControllerBase
    {
        private bool ImportResult = false;

        private UserApp server = new UserApp();
        public ActionResult UpLoadUserInfo()
        {
            return View();
        }


        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult UploadExcelStationInfoData(string fileName)
        {
            string filePath = Request.MapPath("../../Files/Temp/") + fileName;
            string strMessage = ImportStationInfoData(fileName);
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

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetUploadGridJson(string fileName)
        {
            string file = Request.MapPath("../../Files/Temp/") + fileName;
            var data = KezhiExcel.GetExcelDataTable(file);
            return Content(data.ToJson());
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
        /// 批量导入（new style）
        /// </summary>
        private string ImportStationInfoData(string fileName)
        {
            string ErrorInfo = string.Empty;
            string file = Request.MapPath("../../Files/Temp/") + fileName;
            UserEntity model = new UserEntity();
            OrganizeApp organizeApp = new OrganizeApp();
            var departdata = organizeApp.GetList();//部门
            DutyApp dutyApp = new DutyApp();
            var positiondata = dutyApp.GetList();//岗位
            int TotalCount = 0;
            try
            {
                DataTable dt = KezhiExcel.GetExcelDataTable(file);

                //遍历行数据

                //foreach (DataRow dr in dt.Rows)
                for (int i = 0; i <= dt.Rows.Count-1; i++)
                {
                   
                    string fid = ToStr(dt.Rows[i][0]);
                    if (!string.IsNullOrWhiteSpace(fid))
                    {
                        //修改
                        model.F_Id = fid;
                        model.F_Account = ToStr(dt.Rows[i][1]);
                        model.F_RealName = ToStr(dt.Rows[i][2]);
                        model.F_Gender = (ToStr(dt.Rows[i][3])=="男")?true:false;
                        var Organizeitem = departdata.FirstOrDefault(item => item.F_ParentId=="0");
                        model.F_OrganizeId = Organizeitem.F_Id;
                        var departitme = departdata.FirstOrDefault(item => item.F_FullName == ToStr(dt.Rows[i][4]));
                        if (departitme == null)
                            model.F_DutyId = "";
                        else
                        {
                            model.F_DutyId = departitme.F_Id;
                        }
                        model.F_DepartmentId = departitme.F_Id;
                        var positionitme = positiondata.FirstOrDefault(item => item.F_FullName == ToStr(dt.Rows[i][5]) && item.F_OrganizeId == model.F_DepartmentId);
                        if (positionitme == null)
                            model.F_DutyId = "";
                        else
                        {
                            model.F_DutyId = positionitme.F_Id;
                        }
                        model.F_MobilePhone = ToStr(dt.Rows[i][6]);
                        model.UserCode = ToStr(dt.Rows[i][7]);
                        model.F_LastModifyTime = DateTime.Now;
                        model.F_LastModifyUserId = Kezhi.Code.OperatorProvider.Provider.GetCurrent().UserId;

                        //保存工位数据
                        int n = server.UpdateUserInfo(model);
                        TotalCount++;
                    }
                    else { 
                       //新增
                        model.F_Id = Guid.NewGuid().ToString();
                        model.F_Account = ToStr(dt.Rows[i][1]);
                        model.F_RealName = ToStr(dt.Rows[i][2]);
                        model.F_Gender = (ToStr(dt.Rows[i][3]) == "男") ? true : false;
                        var Organizeitem = departdata.FirstOrDefault(item => item.F_ParentId == "0");
                        model.F_OrganizeId = Organizeitem.F_Id;
                        var departitme = departdata.FirstOrDefault(item => item.F_FullName == ToStr(dt.Rows[i][4]));
                        if (departitme == null)
                            model.F_DutyId = "";
                        else
                        {
                            model.F_DutyId = departitme.F_Id;
                        }
                        model.F_DepartmentId = departitme.F_Id;
                        var positionitme = positiondata.FirstOrDefault(item => item.F_FullName == ToStr(dt.Rows[i][5]) && item.F_OrganizeId == model.F_DepartmentId);
                        if (positionitme == null)
                            model.F_DutyId = "";
                        else {
                            model.F_DutyId = positionitme.F_Id;
                        }
                        
                        model.F_MobilePhone = ToStr(dt.Rows[i][6]);
                        model.UserCode = ToStr(dt.Rows[i][7]);
                        model.F_EnabledMark = true;
                        model.F_CreatorTime = DateTime.Now;
                        model.F_CreatorUserId = Kezhi.Code.OperatorProvider.Provider.GetCurrent().UserId;

                        //保存工位数据
                        int n = server.ImportUserInfo(model);
                        UserLogOnEntity userLogOnEntity = new UserLogOnEntity();
                        userLogOnEntity.F_Id = model.F_Id;
                        userLogOnEntity.F_UserId = model.F_Id;
                        userLogOnEntity.F_UserSecretkey = Md5.md5(Common.CreateNo(), 16).ToLower();
                        userLogOnEntity.F_UserPassword = Md5.md5(DESEncrypt.Encrypt(Md5.md5("0000", 32).ToLower(), userLogOnEntity.F_UserSecretkey).ToLower(), 32).ToLower();

                        server.InsertUserLogin(userLogOnEntity);

                        TotalCount++;
                    }
                    

                }
                if (ErrorInfo == "")
                {
                    ImportResult = true;
                    ErrorInfo = "共成功导入" + TotalCount.ToString() + "条数据";
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

    }
}
