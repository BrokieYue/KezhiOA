using Kezhi.Application.OAManage;
using Kezhi.Application.SystemManage;
using Kezhi.Code;
using Kezhi.Domain.Entity.OA;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Kezhi.Web.Areas.OAManage.Controllers
{
    public class ProjectController : ControllerBase
    {
        private ProjectApp projectApp = new ProjectApp();
        private AreaApp areaApp = new AreaApp();
        private UserApp userApp = new UserApp();
        
        /// <summary>
        /// 界面模糊查询功能
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
                rows = projectApp.GetList(pagination, keyword),
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
            projectEntity.F_ProjectAddress = provence + city;
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
        [HttpGet]
        public ActionResult Info()
        {
            return View();
        }

    }
}
