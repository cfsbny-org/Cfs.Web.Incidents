using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class LogController : ApiController
    {

        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();




        public IQueryable<Models.Presentation.LogsViewModel> Get(long id)
        {
            var log = from l in this._db.ReportLogs
                      join u in this._db.Users
                        on l.userId equals u.userId
                      where l.incidentId == id
                      select new Models.Presentation.LogsViewModel
                      {
                          reportLogId = l.reportLogId,
                          incidentId = l.incidentId,
                          userId = l.userId,
                          staffName = u.firstName + " " + u.lastName,
                          logDateTime = l.logDateTime,
                          userStation = l.userStation,
                          logDetails = l.logDetails
                      };


            return log;
        }



        public void Post([FromBody]Models.ReportLog log)
        {
            this._db.ReportLogs.Add(log);
            this._db.SaveChanges();
        }


        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    
    
    }


}