using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CEC_SwiftHR.Models.CustomAttributes
{
    public class MultiLanguage : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userLanguages = filterContext.HttpContext.Request.UserLanguages;
            if (filterContext.HttpContext.Session["CEC_Language"] == null)
            {
                if (userLanguages.Count() == 0)
                {
                    filterContext.HttpContext.Session["CEC_Language"] = "en-us";
                }
                else
                {
                    filterContext.HttpContext.Session["CEC_Language"] = userLanguages[0];
                }
            }
            else
            {

            }

            var cultureInfo = new System.Globalization.CultureInfo(filterContext.HttpContext.Session["CEC_Language"].ToString());
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
        }
    }
}