using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kezhi.Web.Areas.FamilyVisageManage
{
    public class FamilyVisageManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "FamilyVisageManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              this.AreaName + "_Default",
              this.AreaName + "/{controller}/{action}/{id}",
              new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
              new string[] { "Kezhi.Web.Areas." + this.AreaName + ".Controllers" }
            );
        }

    }
}
