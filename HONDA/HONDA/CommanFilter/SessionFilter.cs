using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HONDA.CommanFilter
{
    public class SessionFilter:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (System.Web.HttpContext.Current.Session["emp_id"] == null)
            {
                filterContext.Result = new RedirectResult("~/home/login");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}