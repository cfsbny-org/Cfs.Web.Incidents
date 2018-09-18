using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class DetailsController : ApiController
    {

        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();



        

        
        public Models.IncidentDetail Get(long id)
        {
            return this._db.IncidentDetails.Where(d => d.incidentId == id).SingleOrDefault();
        }



        [HttpGet, Route("api/details/print/{id}")]
        public IQueryable<Models.IncidentDetail> Print(long id)
        {
            return this._db.IncidentDetails.Where(d => d.incidentId == id);
        }



        
        [HttpGet, Route("api/details/saved/{id}")]
        public bool ValidateReportSaved(long id)
        {
            return this._db.IncidentDetails.Where(d => d.incidentId == id).Any();
        }
        


        
        public long Post([FromBody]Models.IncidentDetail details)
        {

            bool existingDetailsRecordExsits = this._db.IncidentDetails.Where(d => d.incidentId == details.incidentId).Any();

            if (details.incidentDetailId == 0)
            {
                if (existingDetailsRecordExsits)
                {
                    // UPDATE EXISTING RECORD
                    this._db.IncidentDetails.Attach(details);
                    this._db.Entry(details).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    // ADD NEW RECORD
                    this._db.IncidentDetails.Add(details);
                }
            }
            else
            {
                // UPDATE EXISTING RECORD
                this._db.IncidentDetails.Attach(details);
                this._db.Entry(details).State = System.Data.Entity.EntityState.Modified;
            }
            this._db.SaveChanges();

            return details.incidentDetailId;
        }

        
        


        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}