using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cfs.Web.Incidents.Controllers
{
    public class NotificationsController : Controller
    {
        // GET: Notifications
        public ActionResult Index(int reportTypeId, long notificationId)
        {
            ViewBag.ReportTypeId = reportTypeId;
            ViewBag.NotificationId = notificationId;
            return View();
        }


       
    }
}