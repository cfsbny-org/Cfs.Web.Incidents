using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cfs.Web.Incidents.Controllers
{
    public class MedicalsController : Controller
    {
        // GET: Medicals
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Help()
        {
            return View();
        }



        public ActionResult New(long id)
        {
            ViewBag.ReportId = id;
            return View();
        }


        public ActionResult Edit(long id)
        {
            ViewBag.MedicalId = id;
            return View();
        }

        public ActionResult FinalApprove()
        {
            return View();
        }


        public ActionResult View(long id)
        {
            ViewBag.MedicalId = id;
            return View();
        }
    }
}