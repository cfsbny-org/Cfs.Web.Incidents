using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cfs.Web.Incidents.Controllers
{
    public class AttachmentsController : Controller
    {
        public ActionResult Photo()
        {
            return View();
        }


        public ActionResult Document()
        {
            return View();
        }


        public ActionResult Edit(long id)
        {
            ViewBag.AttachmentId = id;
            return View();
        }
    }
}