using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cfs.Web.Incidents.Controllers
{
    public class SupervisorController : Controller
    {
        // GET: Supervisor
        public ActionResult Review(string id)
        {
            ViewBag.ReportId = id;
            return View();
        }


        public ActionResult FinalApprove()
        {
            return View();
        }
    }
}