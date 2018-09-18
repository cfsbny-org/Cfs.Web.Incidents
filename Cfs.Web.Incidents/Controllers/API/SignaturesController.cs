using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class SignaturesController : ApiController
    {



        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();



        [Route("api/signatures/list/{id}")]
        public IQueryable<Models.Presentation.ReportSignatureModel> GetReportSignatures(long id)
        {
            var signatures = from s in this._db.ReportSigns
                             join t in this._db.ReportSigTypes
                                on s.reportSigType equals t.reportSigTypeId
                             join a in this._db.ApprovalStatuses
                                on s.approvalStatusId equals a.approvalStatusId
                             join u in this._db.Users
                                on s.reportSigUserId equals u.userId
                             where s.incidentId == id && s.reportSigType != "A"
                             orderby s.reportSigStamp
                             select new Models.Presentation.ReportSignatureModel
                             {
                                 incidentId = s.incidentId,
                                 signatureType = t.reportSigTypeText,
                                 staffName = s.staffName,
                                 staffTitle = s.staffTitle,
                                 staffEmail = u.eMail,
                                 statusId = s.approvalStatusId,
                                 signStatusText = a.approvalStatusDesc,
                                 reportSigStamp = s.reportSigStamp
                             };


            return signatures;
        }



        public IQueryable<Models.ReportSign> Get(long id)
        {
            return this._db.ReportSigns.Where(s => s.incidentId == id);
        }



        public IQueryable<Models.Presentation.ReportSignsViewModel> Print(long id)
        {

            DateTime nullDate = DateTime.Parse("1/1/1900");

            var signatures = from s in this._db.ReportSigns
                             where s.incidentId == id
                             select new Models.Presentation.ReportSignsViewModel
                             {
                                 reportSigId = s.reportSigId,
                                 incidentId = s.incidentId,
                                 reportSigType = s.reportSigType,
                                 incidentMedicalId = s.incidentMedicalId,
                                 reportSigUserId = s.reportSigUserId,
                                 staffName = s.staffName,
                                 staffTitle = s.staffTitle,
                                 approvalStatusId = s.approvalStatusId,
                                 approvalComments = s.approvalComments,
                                 reportSigStamp = s.reportSigStamp == null ? nullDate : (DateTime)s.reportSigStamp,
                                 reportSigStation = s.reportSigStation
                             };

            return signatures;
        }


        public IQueryable<Models.ReportSigType> GetTypes()
        {
            return this._db.ReportSigTypes;

        }


        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}