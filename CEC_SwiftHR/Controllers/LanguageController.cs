using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CEC_SwiftHR.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult Change(string languageCode)
        {
            Session["CEC_Language"] = languageCode;


            return RedirectToAction("Index","Home");
        }
    }
}