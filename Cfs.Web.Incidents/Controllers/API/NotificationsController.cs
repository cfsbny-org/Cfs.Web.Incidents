using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class NotificationsController : ApiController
    {


        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();



        public IQueryable<Models.Notification> Print(long id)
        {
            return this._db.Notifications.Where(n => n.incidentId == id);
        }


        
        
        public IQueryable<Models.Presentation.NotificationsViewModel> Get(long id)
        {
            var notifications = from n in this._db.Notifications
                                join p in this._db.NotifyParties
                                    on n.notifyPartyId equals p.notifyPartyId
                                from u in this._db.Users.Where(u => u.userId == n.notifyStaffId).DefaultIfEmpty()
                                where n.incidentId == id
                                select new Models.Presentation.NotificationsViewModel
                                {
                                    notificationId = n.notificationId,
                                    incidentId = n.incidentId,
                                    notifyPartyId = n.notifyPartyId,
                                    notifyPartyText = p.notifyPartyText,
                                    notifyDateTime = n.notifyDateTime,
                                    notifyContact = n.notifyContact,
                                    notifyMethod = n.notifyMethod,
                                    notifyComments = n.notifyComments,
                                    notifyStaffId = n.notifyStaffId,
                                    notifyStaffName = n.notifyStaffId == 0 ? "System Auto-Notifcation" : u.lastName + ", " + u.firstName,
                                    isAcknowledged = n.isAcknowledged,
                                    acknowledgeUserId = n.acknowledgeUserId,
                                    acknowledgedStamp = n.acknowledgedStamp,
                                    acknowledgedStation = n.acknowledgedStation,
                                    notifyInfo = n.notifyInfo
                                };

            return notifications;
        }


        [Route("api/notifications/edit/{id}")]
        public Models.Notification GetSingle(long id)
        {
            var notification = this._db.Notifications.Where(n => n.notificationId == id).SingleOrDefault();

            if (notification == null)
            {
                notification = new Models.Notification();
            }

            return notification;
        }



        [HttpGet, Route("api/notifications/validate/{id}")]
        public bool ValidateNotifications(long id)
        {
            var types = (from i in this._db.ReportIncidents
                         join t in this._db.IncidentTypes
                                 on i.incidentTypeId equals t.incidentTypeId
                         where i.incidentId == id
                                && (t.incidentCategoryId == 1 || t.incidentCategoryId == 2)
                         select i).Any();

            var jcNotes = (from n in this._db.Notifications
                           where n.incidentId == id
                                 && n.notifyPartyId == 8 // Justice Center
                           select n).Any();

            bool isValid = true;


            if (!types)  // NO TYPES REQUIRING JC CALL
            {
                isValid = true;
            }
            else if (types && jcNotes)  // TYPES REQUIRING JC CALL EXIST AND JC NOTES EXIST
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }

            return isValid;
        }


       


        // POST api/<controller>
        public void Post([FromBody]Models.Notification notification)
        {
            if (notification.notificationId == 0)
            {
                this._db.Notifications.Add(notification);
            }
            else
            {
                this._db.Notifications.Attach(notification);
                this._db.Entry(notification).State = System.Data.Entity.EntityState.Modified;
            }

            this._db.SaveChanges();


            if ( notification.notifyPartyId == 8) // JUSTICE CENTER
            {
                ReportsController reportsController = new ReportsController();
                Models.Presentation.ReportsViewModel report = reportsController.GetReportHeader(notification.incidentId);
                reportsController.Dispose();


                Controllers.MailController mailer = new MailController();

                List<string> sendTos = new List<string>();
                StringBuilder msg = new StringBuilder();

                sendTos.Add("CorpCompIncidents@cfsbny.org");

                Models.Notification complianceNotification = new Models.Notification();

                complianceNotification.incidentId = notification.incidentId;
                complianceNotification.notifyPartyId = 37;
                complianceNotification.notifyDateTime = DateTime.Now;
                complianceNotification.notifyContact = "CFS Compliance";
                complianceNotification.notifyMethod = "E-Mail";
                complianceNotification.notifyStaffId = 0;
                complianceNotification.notifyComments = "Automatic E-Mail sent to compliance by incident report system.";
                complianceNotification.isAcknowledged = 1;
                complianceNotification.acknowledgeUserId = 0;

                this._db.Notifications.Add(complianceNotification);
                this._db.SaveChanges();

                msg.Append("<h1>Incident Report: Compliance Notification</h1>");
                msg.Append("<p>A new incident report for client " + report.clientName + " by " + report.staffName + " has been created and is being forwarded to compliance.</p>");
                msg.Append("<p><a href=\"http://cfs-incidents/admin/review/" + report.incidentId.ToString() + "\">Click here to view the report.</a></p>");


                //sendTos.Add(notifier.emailAddress);  
                //sendTos.Add("jbowen@cfsbny.org"); //MTS - commented out March '18
                mailer.SendMail(sendTos, "techservices@cfsbny.org", "Incident Report Compliance Notification: Justice Center Called", System.Net.Mail.MailPriority.High, msg);
            }
        }



        [Route("api/notifications/automatic")]
        public void PostAutoNotifiers([FromBody]Models.Presentation.ProgramNotifiersModel[] notifiers)
        {


            Controllers.MailController mailer = new MailController();

            long incidentId = 0;
            incidentId = notifiers.FirstOrDefault().incidentId;



            ReportsController reportsController = new ReportsController();
            Models.Presentation.ReportsViewModel report = reportsController.GetReportHeader(incidentId);
            reportsController.Dispose();


            List<string> sendTos = new List<string>();
            StringBuilder msg = new StringBuilder();

            //sendTos.Add(notifier.emailAddress);  
            sendTos.Add("jbowen@cfsbny.org");

            msg.Append("<h1>New Incident Report</h1>");
            msg.Append("<p>A new incident report has been created for client " + report.clientName + " by " + report.staffName + ".</p>");
            msg.Append("<p><a href=\"http://cfs-incidents/report/residential/" + report.incidentId.ToString() + "\">Click here to view the report.</a></p>");
            msg.Append("<p><a href=\"http://cfs-incidents/Medicals\">Click here to create a medical review.</a></p>");

            // COMMENT OUT BEFORE LIVE
            //msg.Append("<p>To be included:<br />");


            foreach (var notifier in notifiers)
            {

                //msg.Append(notifier.emailAddress + "<br />");

                sendTos.Add(notifier.emailAddress.ToLower().Trim());

                Models.Notification notification = new Models.Notification();

                notification.incidentId = incidentId;
                notification.notifyPartyId = notifier.notifyPartyId;
                notification.notifyDateTime = DateTime.Now;
                notification.notifyContact = notifier.staffName;
                notification.notifyMethod = "E-Mail";
                notification.notifyStaffId = notifier.userId == null ? 0 : (long)notifier.userId;
                notification.notifyComments = "Automatic E-Mail sent to " + notifier.emailAddress.ToLower().Trim() + " by incident report system.";
                notification.isAcknowledged = 1;
                notification.acknowledgeUserId = notifier.userId == null ? 0 : (long)notifier.userId;

                this._db.Notifications.Add(notification);
            }


            this._db.SaveChanges();


            // COMMENT OUT BEFORE LIVE
            //msg.Append("</p>");
            

            // ADD REPORT DETAILS AND LINK TO REPORT


            // ACKNOWLEDGEMENT LINK
            msg.Append("<p><a href='/'></a></p>");


            mailer.SendMail(sendTos, "techservices@cfsbny.org", "New Incident Report: " + report.clientName, System.Net.Mail.MailPriority.High, msg);




            mailer.Dispose();

        }




        [HttpPost, Route("api/notifications/compliance")]
        public void SendNotificationToCompliance([FromBody]long value)
        {
            ReportsController reportsController = new ReportsController();
            Models.Presentation.ReportsViewModel report = reportsController.GetReportHeader(value);
            reportsController.Dispose();


            Controllers.MailController mailer = new MailController();

            List<string> sendTos = new List<string>();
            StringBuilder msg = new StringBuilder();

            sendTos.Add("CorpCompIncidents@cfsbny.org");

            Models.Notification notification = new Models.Notification();

            notification.incidentId = value;
            notification.notifyPartyId = 37;
            notification.notifyDateTime = DateTime.Now;
            notification.notifyContact = "CFS Compliance";
            notification.notifyMethod = "E-Mail";
            notification.notifyStaffId = 0;
            notification.notifyComments = "Automatic E-Mail sent to compliance by incident report system.";
            notification.isAcknowledged = 1;
            notification.acknowledgeUserId = 0;

            this._db.Notifications.Add(notification);
            this._db.SaveChanges();

            msg.Append("<h1>Incident Report: Compliance Notification</h1>");
            msg.Append("<p>A new incident report for client " + report.clientName + " by " + report.staffName + " has been created and is being forwarded to compliance.</p>");
            msg.Append("<p><a href=\"http://cfs-incidents/admin/review/" + report.incidentId.ToString() + "\">Click here to view the report.</a></p>");
            

            //sendTos.Add(notifier.emailAddress);  
            //sendTos.Add("jbowen@cfsbny.org"); //MTS - commented out March '18
            mailer.SendMail(sendTos, "techservices@cfsbny.org", "Incident Report Compliance Notification: " + report.clientName, System.Net.Mail.MailPriority.High, msg);
        }




        // DELETE api/<controller>/5
        public void Delete(long id)
        {
            var notification = this._db.Notifications.Where(n => n.notificationId == id).SingleOrDefault();
            this._db.Notifications.Attach(notification);
            this._db.Entry(notification).State = System.Data.Entity.EntityState.Deleted;
            this._db.SaveChanges();
        }



        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }

    }
}