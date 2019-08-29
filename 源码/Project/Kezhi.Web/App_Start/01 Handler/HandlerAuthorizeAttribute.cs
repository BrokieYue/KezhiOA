using Kezhi.Application.SystemManage;
using Kezhi.Code;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kezhi.Web
{
    public class HandlerAuthorizeAttribute : ActionFilterAttribute
    {
        public bool Ignore { get; set; }
        public HandlerAuthorizeAttribute(bool ignore = true)
        {
            Ignore = ignore;
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (OperatorProvider.Provider.GetCurrent().IsSystem)
                {
                    return;
                }
                if (Ignore == false)
                {
                    return;
                }
                if (!this.ActionAuthorize(filterContext))
                {
                    StringBuilder sbScript = new StringBuilder();
                    sbScript.Append("<script type='text/javascript'>alert('很抱歉！您的权限不足，访问被拒绝！');</script>");
                    filterContext.Result = new ContentResult() { Content = sbScript.ToString() };
                    return;
                }
            }
            catch (Exception  ex)
            {
                StringBuilder sbScript = new StringBuilder();
                sbScript.Append("<script type='text/javascript'>alert('很抱歉！会话时间已超时，请重新登录！');</script>");//2018-7-16
                filterContext.Result = new ContentResult() { Content = sbScript.ToString() };
                return;
            }
        }
        private bool ActionAuthorize(ActionExecutingContext filterContext)
        {
            var operatorProvider = OperatorProvider.Provider.GetCurrent();
            var roleId = operatorProvider.RoleId;
            var moduleId = WebHelper.GetCookie("nfine_currentmoduleid");
            var action = HttpContext.Current.Request.ServerVariables["SCRIPT_NAME"].ToString();
            return new RoleAuthorizeApp().ActionValidate(roleId, moduleId, action);
        }
    }
}