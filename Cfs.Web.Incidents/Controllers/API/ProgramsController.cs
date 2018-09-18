using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class ProgramsController : ApiController
    {



        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();



        // GET api/<controller>
        public IQueryable<Models.IncidentProgram> Get()
        {
            return this._db.IncidentPrograms.Where(p => p.isActive == true).OrderBy(p => p.programTitle);
        }




        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}