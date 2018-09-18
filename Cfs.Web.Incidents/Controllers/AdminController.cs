using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cfs.Web.Incidents.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Open()
        {
            return View();
        }



        public ActionResult Agendas()
        {
            return View();
        }


        public ActionResult Search()
        {
            return View();
        }


        public ActionResult Help()
        {
            return View();
        }


        public ActionResult Review(long id)
        {
            string referrer = string.Empty;

            if (Request.UrlReferrer != null)
            {
                referrer = Request.UrlReferrer.ToString();
            }

            ViewBag.Referrer = referrer;
            ViewBag.ReportId = id;
            return View();
        }


        public ActionResult Diary()
        {
            return View();
        }


        public ActionResult FinalApprove()
        {
            return View();
        }



        public ActionResult Notifiers()
        {
            return View();
        }



        public ActionResult AllReports()
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