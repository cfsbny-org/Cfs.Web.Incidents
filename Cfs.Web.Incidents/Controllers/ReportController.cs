using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cfs.Web.Incidents.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index(long id)
        {
            return View();
        }


        public ActionResult Search()
        {
            return View();
        }



        public ActionResult Create()
        {
            return View();
        }



        public ActionResult Residential(long id)
        {
            ViewBag.ReportId = id;
            return View();
        }


        public ActionResult NonResidential(long id)
        {
            ViewBag.ReportId = id;
            return View();
        }



        public ActionResult FinalApprove()
        {
            return View();
        }



        public ActionResult Show(long id)
        {
            ViewBag.ReportId = id;
            return View();
        }
       
    }
}