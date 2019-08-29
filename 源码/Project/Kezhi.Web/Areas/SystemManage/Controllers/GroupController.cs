using Kezhi.Application.SystemManage;
using Kezhi.Code;
using Kezhi.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kezhi.Web.Areas.SystemManage.Controllers
{
    public class GroupController : ControllerBase
    {
        private GroupApp groupApp = new GroupApp();
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGroupInfoJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = groupApp.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = groupApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(GroupEntity entity, string keyValue)
        {
            groupApp.SubmitForm(entity, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            groupApp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

    }
}
