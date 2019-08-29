using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kezhi.Web.Attachment
{
    public partial class Attachment : System.Web.UI.Page
    {
        private const string _DIR = "/Files/Temp/";
        protected void Page_Load(object sender, EventArgs e)
        {
            string action = Request.QueryString["action"];
            ResModel model = new ResModel();
            model.result = "ok";
            try
            {
                if ("upload".Equals(action))
                {
                    FileUpload(model);
                }
            }
            catch (Exception ex)
            {
                model.result = "err";
                model.message = ex.Message;
                Response.Write(Serializer(model));
                return;
            }

            Response.Write(Serializer(model));
            return;
        }

        private void FileUpload(ResModel model)
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            string path = Request.MapPath(_DIR);
            //如果文件夹不存在，创建
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            // 虽然是循环，但是一次只能上传一个文件
            for (int i = 0; i < files.Count; i++)
            {
                string fileId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                //string tempName = GetFileExName(fileId, files[i].FileName);
                string tempName = System.IO.Path.GetFileName(files[i].FileName);
                
                string fileName = path + tempName;
                //如果文件存在，删除
                if (System.IO.File.Exists(fileName))
                {
                    RemoveFile(fileName);
                }
                if ("exNameErr".Equals(tempName))
                {
                    Exception e = new Exception("文件类型错误");
                    throw e;
                }

               
                model.fileName = tempName;
                //Request.MapPath("/Upload/Materiel/") + System.IO.Path.GetFileName(fileName)
                files[i].SaveAs(fileName);
            }
        }

        /// <summary>
        /// 取得文件扩展名
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetFileExName(string fileId, string fileName)
        {
            string[] temp = fileName.Split('.');

            if (temp.Length > 0)
            {
                string exName = temp[temp.Length - 1].ToLower();

                if (!"xls".Equals(exName))
                {
                    return "exNameErr";
                }
                return fileId + "." + exName;
            }

            return string.Empty;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        private string Serializer(object obj)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            return javaScriptSerializer.Serialize(obj);
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
                if (!File.Exists(fileName))
                {
                    return true;
                }
                File.Delete(fileName);
            }
            catch
            {
                return false;
            }

            return true;
        }

        private class ResModel
        {
            public string result { get; set; }
            public string message { get; set; }
            public string fileName { get; set; }
        }
    }
}