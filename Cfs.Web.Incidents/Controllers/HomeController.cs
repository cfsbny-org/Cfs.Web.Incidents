using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cfs.Web.Incidents.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            //ViewBag.CurrentUser = User.Identity.Name;
            return View();
        }



        public ActionResult Sandbox()
        {
            string userName = System.Web.HttpContext.Current.User.Identity.Name.Substring(5).ToLower();

            ViewBag.UserName = userName;

            return View();
        }

        
    }
}