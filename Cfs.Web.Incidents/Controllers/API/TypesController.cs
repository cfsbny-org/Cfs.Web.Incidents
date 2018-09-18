using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class TypesController : ApiController
    {


        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();


        public IQueryable<Models.IncidentType> Get()
        {
            return this._db.IncidentTypes;
        }


        public IQueryable<Models.ReportIncident> Print_GetReportIncidents(long id)
        {
            return this._db.ReportIncidents.Where(r => r.incidentId == id);
        }

        

        [Route("api/types/{reportId}/{reportType}/{typeCategory}")]
        public IQueryable<Models.IncidentType> GetTypesForReport(long reportId, int reportType, int typeCategory)
        {



            var types = from t in this._db.IncidentTypes
                        where t.incidentReportTypeId == reportType
                                && t.incidentCategoryId == typeCategory
                                && !((from i in this._db.ReportIncidents
                                      where i.incidentId == reportId
                                      select i.incidentTypeId).Contains(t.incidentTypeId))
                        select t;

            return types;
        }



        [Route("api/types/selected/{reportId}/{typeCategory}")]
        public IQueryable<Models.Presentation.IncidentTypesRportModel> GetReportIncidents(long reportId, int typeCategory)
        {
            var types = from i in this._db.ReportIncidents
                        join t in this._db.IncidentTypes
                            on i.incidentTypeId equals t.incidentTypeId
                        where i.incidentId == reportId && t.incidentCategoryId == typeCategory
                        select new Models.Presentation.IncidentTypesRportModel
                        {
                            reportIncidentTypeId = i.reportIncidentTypeId,
                            incidentId = i.incidentId,
                            incidentTypeId = i.incidentTypeId,
                            incidentTypeText = t.incidentTypeText,
                            incidentInfo = i.incidentInfo,
                            needsInfo = t.needsInfo,
                            needsInfoLabel = t.needsInfoLabel
                        };

            return types;
        }



        [Route("api/types/selected/all/{id}")]
        public IQueryable<Models.Presentation.IncidentTypesRportModel> GetReportIncidents(long id)
        {
            var types = from i in this._db.ReportIncidents
                        join t in this._db.IncidentTypes
                            on i.incidentTypeId equals t.incidentTypeId
                        where i.incidentId == id 
                        select new Models.Presentation.IncidentTypesRportModel
                        {
                            reportIncidentTypeId = i.reportIncidentTypeId,
                            incidentId = i.incidentId,
                            incidentTypeId = i.incidentTypeId,
                            incidentTypeText = t.incidentTypeText,
                            incidentInfo = i.incidentInfo,
                            needsInfo = t.needsInfo,
                            needsInfoLabel = t.needsInfoLabel
                        };

            return types;
        }


        [Route("api/report/incident/{id}")]
        public Models.Presentation.IncidentTypesRportModel GetReportIncident(long id)
        {
            var incident = (from i in this._db.ReportIncidents
                           join t in this._db.IncidentTypes
                               on i.incidentTypeId equals t.incidentTypeId
                           where i.reportIncidentTypeId == id
                           select new Models.Presentation.IncidentTypesRportModel
                           {
                               reportIncidentTypeId = i.reportIncidentTypeId,
                               incidentId = i.incidentId,
                               incidentTypeId = i.incidentTypeId,
                               incidentTypeText = t.incidentTypeText,
                               incidentInfo = i.incidentInfo,
                               needsInfo = t.needsInfo,
                               needsInfoLabel = t.needsInfoLabel
                           }).SingleOrDefault();

            return incident;
        }




        [HttpGet, Route("api/types/validate/{id}")]
        public bool ValidateReportTypes(long id)
        {
            var reportTypes = (from r in this._db.ReportIncidents
                               join t in this._db.IncidentTypes
                                 on r.incidentTypeId equals t.incidentTypeId
                               where r.incidentId == id &&
                                   (t.incidentCategoryId == 1 || t.incidentCategoryId == 2 || t.incidentCategoryId == 3)
                               select r.reportIncidentTypeId).Count();

            if (reportTypes == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
            
        }




        public void Post([FromBody]List<Models.ReportIncident> incidents)
        {

            var incidentTypes = this._db.IncidentTypes.Where(t => t.incidentReportTypeId == 1).ToList();


            foreach (Models.ReportIncident incident in incidents)
            {
                this._db.ReportIncidents.Add(incident);
                this._db.SaveChanges();

                var incidentType = incidentTypes.Where(t => t.incidentTypeId == incident.incidentTypeId).SingleOrDefault();

                if (incidentType != null)
                {
                    if (incidentType.incidentCategoryId == 1 || incidentType.incidentCategoryId == 2) // REPORTABLE TO JC
                    {

                        ReportsController reportsController = new ReportsController();
                        Models.Presentation.ReportsViewModel report = reportsController.GetReportHeader(incident.incidentId);
                        reportsController.Dispose();


                        Controllers.MailController mailer = new MailController();

                        List<string> sendTos = new List<string>();
                        StringBuilder msg = new StringBuilder();

                        sendTos.Add("CorpCompIncidents@cfsbny.org");

                        Models.Notification notification = new Models.Notification();

                        notification.incidentId = incident.incidentId;
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
                        msg.Append("<p><a href=\"http://cfs-incidents/Admin/Review/" + report.incidentId.ToString() + "\">Click here to view the report.</a></p>");


                        //sendTos.Add(notifier.emailAddress);  
                        //sendTos.Add("jbowen@cfsbny.org"); //MTS - commented out March '18
                        mailer.SendMail(sendTos, "techservices@cfsbny.org", "Incident Report Compliance Notification: " + report.clientName, System.Net.Mail.MailPriority.High, msg);
                    }
                }
            }
        }


        [HttpPost, Route("api/types/incidents/update")]
        public void PostUpdate([FromBody]Models.ReportIncident incident)
        {
            this._db.ReportIncidents.Attach(incident);
            this._db.Entry(incident).State = System.Data.Entity.EntityState.Modified;
            this._db.SaveChanges();
        }



        public void Delete([FromBody]Models.ReportIncident incident)
        {
            this._db.ReportIncidents.Attach(incident);
            this._db.Entry(incident).State = System.Data.Entity.EntityState.Deleted;
            this._db.SaveChanges();
        }


        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}