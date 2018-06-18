using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CEC_SwiftHR.Controllers
{
    public class LogOutController : Controller
    {
        // GET: LogOut
        public ActionResult Index()
        {
            //HttpContext.Session.Clear();
            Session["Login"] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}