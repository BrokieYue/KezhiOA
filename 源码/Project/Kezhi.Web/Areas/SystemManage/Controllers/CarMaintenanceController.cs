/*******************************************************************************
 * Copyright © 2016 科致电气
 * Author: GeneralCheng
 * Description: 长安渝北工厂焊装车间WIMS系统
 * Website：www.kezhi-electric.com
*********************************************************************************/
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemManage.Controllers
{
    public class CarMaintenanceController : ControllerBase
    {
        private CarMaintenanceAPP carmaintenanceapp = new CarMaintenanceAPP();

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = carmaintenanceapp.GetList(keyword);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = carmaintenanceapp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(CarMaintenanceEntity carMaintenanceEntity, string keyValue)
        {
            int kv = 0;
            if (keyValue.Length > 0)
            {
                kv = int.Parse(keyValue);
            }
            carmaintenanceapp.SubmitForm(carMaintenanceEntity,  kv);
            return Success("操作成功。");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(int keyValue)
        {
            carmaintenanceapp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

    }
}
