using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class AttachmentsController : ApiController
    {


        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();


        public Models.IncidentAttachment Get(long id)
        {
            return this._db.IncidentAttachments.Where(a => a.incidentAttachmentId == id).SingleOrDefault();
        }

       
        public IQueryable<Models.IncidentAttachment> Print(long id)
        {
            return this._db.IncidentAttachments.Where(a => a.incidentId == id);
        }



        
        public void Post([FromBody]Models.IncidentAttachment attachment)
        {
            this._db.IncidentAttachments.Attach(attachment);
            this._db.Entry(attachment).State = System.Data.Entity.EntityState.Modified;
            this._db.SaveChanges();
        }

       

        // DELETE api/<controller>/5
        public void Delete(long id)
        {
            var attachment = this._db.IncidentAttachments.Where(a => a.incidentAttachmentId == id).SingleOrDefault();
            attachment.isVoided = true;

            this._db.IncidentAttachments.Attach(attachment);
            this._db.Entry(attachment).State = System.Data.Entity.EntityState.Modified;
            this._db.SaveChanges();
        }



        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}