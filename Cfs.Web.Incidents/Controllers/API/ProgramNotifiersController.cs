using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class ProgramNotifiersController : ApiController
    {



        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();



        [Route("api/programnotifiers/program/{id}")]
        public IQueryable<Models.Presentation.ProgramNotifiersModel> GetNotifiersByProgram(int id)
        {
            var notifiers = from pn in this._db.ProgramNotifiers
                            join p in this._db.IncidentPrograms
                                    on pn.programId equals p.incidentProgramId
                            join n in this._db.NotifyParties
                                    on pn.notifyPartyId equals n.notifyPartyId
                            from u in this._db.Users.Where(u => u.userId == pn.userId).DefaultIfEmpty()
                            where pn.programId == id
                            orderby p.programTitle, n.notifyPartyText
                            select new Models.Presentation.ProgramNotifiersModel
                            {
                                programStaffId = pn.programStaffId,
                                programId = pn.programId,
                                programTitle = p.programTitle,
                                notifyPartyId = pn.notifyPartyId,
                                notifyPartyText = n.notifyPartyText,
                                userId = pn.userId,
                                staffName = pn.userId == null ? pn.distEmail : u.firstName + " " + u.lastName,
                                emailAddress = pn.userId == null ? pn.distEmail : u.eMail,
                                smsNumber = pn.smsNumber,
                                isRemovable = pn.isRemovable
                            };


            return notifiers;
        }


        public IQueryable<Models.Presentation.ProgramNotifiersModel> Get(int id)
        {
            var notifiers = from pn in this._db.ProgramNotifiers
                            join p in this._db.IncidentPrograms
                                    on pn.programId equals p.incidentProgramId
                            join n in this._db.NotifyParties
                                    on pn.notifyPartyId equals n.notifyPartyId
                            from u in this._db.Users.Where(u => u.userId == pn.userId).DefaultIfEmpty()
                            where pn.programId == id
                            select new Models.Presentation.ProgramNotifiersModel
                            {
                                programStaffId = pn.programStaffId,
                                programId = pn.programId,
                                programTitle = p.programTitle,
                                notifyPartyId = pn.notifyPartyId,
                                notifyPartyText = n.notifyPartyText,
                                userId = pn.userId,
                                staffName = pn.userId == null ? pn.distEmail : u.firstName + " " + u.lastName,
                                emailAddress = pn.userId == null ? pn.distEmail : u.eMail,
                                smsNumber = pn.smsNumber,
                                isRemovable = pn.isRemovable
                            };

            
            return notifiers;
        }

        



        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}