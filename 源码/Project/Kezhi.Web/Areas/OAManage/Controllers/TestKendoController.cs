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
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Kezhi.Web.Areas.OAManage.Controllers
{

    public class TestKendoController : ControllerBase
    {
        private UserApp userApp = new UserApp();
        private WorkDailyRecordApp workApp = new WorkDailyRecordApp();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult getUser()
        {
            //Pagination pagination = new Pagination();
            //pagination.page = 2;
            //pagination.records
            var userList = userApp.GetList();
            //var data = new
            //{

            //    //rows = userList,
            //    //total = pagination.total,
            //    //page = pagination.page,
            //    //records = pagination.records
            //};
            test();
           return Content(userList.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult getWork(string keyvalue)
        {
            DateTime st = Convert.ToDateTime("2018-01-01");
            DateTime et = Convert.ToDateTime("2020-01-01");
            var worklist = workApp.GetListNoPage(keyvalue, st, et, null, null);
            return Content(worklist.ToJson());

        }

        public void test()
        {
            


        }

    }
    
}
