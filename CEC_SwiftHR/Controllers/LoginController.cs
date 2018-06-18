using CEC_SwiftHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CEC_SwiftHR.Controllers
{
    public class LoginController : Controller
    {
        public NewEmployeeEntities db = new NewEmployeeEntities();
        // GET: Login
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        // bool 後面接到的前端name可以給=false,以防當bool不加?(可以允許空值)時出錯//
        public ActionResult Index(string username,string password, bool remember = false)
        {
            var logins = db.Logins.ToList();
          
            foreach (var item in logins)
            {
                if (item.Username == username && item.Password == password)
                {
                    Session["Login"] = username;
                    return RedirectToAction("Index", "Home");
                }
                
            }
            TempData["error"] = "帳號密碼有誤";
            return RedirectToAction("Index", "Login");

            //var loginname = login.Username;
            //var loginpassword = login.Password;
            //if (username == login.Username && password  == login.Password)
            //{

            //}
            //else
            //{
            //    ModelState.AddModelError("password", "帳密錯誤");
            //    return View("Index", "Login");
            //}


        }
    }
}