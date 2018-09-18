using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cfs.Web.Incidents.Controllers
{
    public class TypesController : Controller
    {
        
        public ActionResult Reportable()
        {
            return View();
        }



        public ActionResult Significant()
        {
            return View();
        }


        public ActionResult Internal()
        {
            return View();
        }


        public ActionResult Clinical()
        {
            return View();
        }


        public ActionResult AdditionalInfo(long id)
        {
            ViewBag.ReportIncidentTypeId = id;
            return View();
        }
    }
}