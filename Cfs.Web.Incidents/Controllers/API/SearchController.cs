using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class SearchController : ApiController
    {


        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();



        
        public IQueryable<Models.Presentation.ReportsViewModel> Post([FromBody]Models.Presentation.SearchCriteria value)
        {
            var reports = from r in this._db.IncidentReports
                          join u in this._db.Users
                            on r.userId equals u.userId
                          join p in this._db.IncidentPrograms
                            on r.programId equals p.incidentProgramId
                          join s in this._db.ReportStatuses
                            on r.statusId equals s.reportStatusId
                          where r.clientName.Contains(value.clientName)
                              && r.incidentDate >= value.startDate
                              && r.incidentDate <= value.endDate
                          select new Models.Presentation.ReportsViewModel
                          {
                              incidentId = r.incidentId,
                              createdByName = u.lastName + ", " + u.firstName,
                              clientName = r.clientName,
                              programTitle = p.programTitle,
                              incidentDate = r.incidentDate,
                              isApproximate = r.isApproximate,
                              createdStamp = r.createdStamp,
                              lastModified = r.lastModified,
                              statusId = r.statusId,
                              currentStatus = s.reportStatusText
                          };



            return reports;
        }




        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}