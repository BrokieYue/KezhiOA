using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kezhi.Web.Areas.OA.Views.Project
{
    public class WorkDailyRecordController : ControllerBase
    {
        //
        // GET: /OA/WorkDailyRecord/

        [HttpGet]
        public ActionResult Info()
        {
            return View();
        }

    }
}
