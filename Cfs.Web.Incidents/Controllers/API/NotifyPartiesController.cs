using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class NotifyPartiesController : ApiController
    {



        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();



        public IQueryable<Models.NotifyParty> Get(int id)
        {
            return this._db.NotifyParties.Where(p => p.incidentReportTypeId == id && p.isActive == true);
        }



        public IQueryable<Models.NotifyParty> Print()
        {
            return this._db.NotifyParties;
        }



        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}