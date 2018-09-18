using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class RestraintsController : ApiController
    {


        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();


       

        
        public IQueryable<Models.Restraint> Get(long id)
        {
            return this._db.Restraints.Where(r => r.incidentId == id);
        }


        [HttpGet, Route("api/restraints/validate/{id}")]
        public bool Validate(long id)
        {
            return this._db.Restraints.Any(r => r.incidentId == id);
        }



        
        public void Post([FromBody]Models.Restraint restraint)
        {
            if (restraint.IncidentRestraintId == 0)
            {
                this._db.Restraints.Add(restraint);
            }
            else
            {
            }

            this._db.SaveChanges();
        }

     

        
        public void Delete([FromBody]Models.Restraint restraint)
        {
            this._db.Restraints.Attach(restraint);
            this._db.Entry(restraint).State = System.Data.Entity.EntityState.Deleted;
            this._db.SaveChanges();
        }


        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}