using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class MedicalsController : ApiController
    {


        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();




        [Route("api/medicals/{id}")]
        public Models.Presentation.MedAssmntViewModel Get(long id)
        {
            var medical = (from m in this._db.Medicals
                           join r in this._db.IncidentReports
                                on m.incidentId equals r.incidentId
                           join u in this._db.Users
                                on m.medStaffId equals u.userId
                           join s in this._db.ReportStatuses
                               on m.statusId equals s.reportStatusId
                           where m.incidentMedicalId == id
                           select new Models.Presentation.MedAssmntViewModel
                           {
                               incidentMedicalId = m.incidentMedicalId,
                               incidentId = m.incidentId,
                               clientName = r.clientName,
                               medStaffId = m.medStaffId,
                               staffName = u.firstName + " " + u.lastName,
                               medAssmntDate = m.medAssmntDate,
                               medAssmnt = m.medAssmnt,
                               createdStamp = m.createdStamp,
                               createdStation = m.createdStation,
                               statusId = m.statusId,
                               currentStatus = s.reportStatusText,
                               isOpen = s.reportIsActive
                           }).SingleOrDefault();

            return medical;
        }


        [Route("api/medicals/report/{id}")]
        public IQueryable<Models.Presentation.MedAssmntViewModel> GetReportMedicals(long id)
        {
            var medicals = from m in this._db.Medicals
                           join u in this._db.Users
                                on m.medStaffId equals u.userId
                           join s in this._db.ReportStatuses
                                on m.statusId equals s.reportStatusId
                           where m.incidentId == id
                           select new Models.Presentation.MedAssmntViewModel
                           {
                               incidentMedicalId = m.incidentMedicalId,
                               incidentId = m.incidentId,
                               medStaffId = m.medStaffId,
                               staffName = u.firstName + " " + u.lastName,
                               medAssmntDate = m.medAssmntDate,
                               medAssmnt = m.medAssmnt,
                               createdStamp = m.createdStamp,
                               statusId = m.statusId,
                               currentStatus = s.reportStatusText,
                               isOpen = s.reportIsActive
                               
                           };

            return medicals;
        }



        [Route("api/medicals/print/{id}")]
        public IQueryable<Models.Presentation.MedAssmntViewModel> Print(long id)
        {
            var medicals = from m in this._db.Medicals
                           join u in this._db.Users
                                on m.medStaffId equals u.userId
                           join s in this._db.ReportStatuses
                                on m.statusId equals s.reportStatusId
                           where m.incidentId == id && s.reportIsActive == false
                           select new Models.Presentation.MedAssmntViewModel
                           {
                               incidentMedicalId = m.incidentMedicalId,
                               incidentId = m.incidentId,
                               medStaffId = m.medStaffId,
                               staffName = u.firstName + " " + u.lastName,
                               medAssmntDate = m.medAssmntDate,
                               medAssmnt = m.medAssmnt,
                               createdStamp = m.createdStamp,
                               statusId = m.statusId,
                               currentStatus = s.reportStatusText,
                               isOpen = s.reportIsActive
                           };

            return medicals;
        }



        public IQueryable<Models.Medical> Print_GetMedicals(long id)
        {
            return this._db.Medicals.Where(m => m.incidentId == id);
        }



        [HttpGet, Route("api/medicals/validate/{id}")]
        public bool ValidateMedicals(long id)
        {
            return this._db.Medicals.Where(m => m.incidentId == id).Any();
        }



       
        
        public long Post([FromBody]Models.Medical medical)
        {
            if (medical.incidentMedicalId == 0)
            {
                this._db.Medicals.Add(medical);
            }
            else
            {
                this._db.Medicals.Attach(medical);
                this._db.Entry(medical).State = System.Data.Entity.EntityState.Modified;
            }

            try
            {
                this._db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);


                MailController mailer = new MailController();
                mailer.SendMail(
                           new List<string>() { "jbowen@cfsbny.org" },
                           "techservices@cfsbny.org",
                           "ERROR CREATING MEDICAL ASSESSMENT: VALIDATION",
                           System.Net.Mail.MailPriority.High,
                           exceptionMessage
                           );

                string currentUser = RequestContext.Principal.Identity.Name;

                mailer.SendExceptionDetail("post:/api/medicals", exceptionMessage, ex.StackTrace, currentUser, medical);


                // Throw a new DbEntityValidationException with the improved exception message.
                throw new System.Data.Entity.Validation.DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
            catch (Exception ex)
            {


                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner Exception: " + ex.InnerException;
                }

                MailController mailer = new MailController();
                mailer.SendMail(
                           new List<string>() { "jbowen@cfsbny.org" },
                           "techservices@cfsbny.org",
                           "ERROR CREATING MEDICAL ASSESSMENT",
                           System.Net.Mail.MailPriority.High,
                           errorMessage
                           );

                string currentUser = RequestContext.Principal.Identity.Name;

                mailer.SendExceptionDetail("post:/api/medicals", errorMessage, ex.StackTrace, currentUser, medical);

                throw new Exception(errorMessage);
            }

            return medical.incidentMedicalId;
        }



        [Route("api/medicals/approve")]
        public void Approve([FromBody]Models.Presentation.ReportSignatureModel signatureInfo)
        {
            string userName = System.Web.HttpContext.Current.User.Identity.Name.Substring(5).ToLower();
            if (userName.ToLower() != signatureInfo.userName.ToLower())
            {
                throw new Exception("Current user information is not synchronized.  Cannot approve report.");
            }

            SessionController session = new SessionController();

            bool userVerified = session.VerifyPassword(userName, signatureInfo.signature);
            session.Dispose();

            if (userVerified)
            {

                Models.Medical medicalAssessment = this._db.Medicals.Where(m => m.incidentMedicalId == signatureInfo.incidentMedicalId).SingleOrDefault();

                if (medicalAssessment != null)
                {
                    // SET STATUS OF ASSESSMENT TO CLOSED
                    medicalAssessment.statusId = 6;
                    this._db.Medicals.Attach(medicalAssessment);
                    this._db.Entry(medicalAssessment).State = System.Data.Entity.EntityState.Modified;



                    // CREATE SIGNATURE RECORD
                    Models.ReportSign medicalSignature = new Models.ReportSign();

                    medicalSignature.incidentId = signatureInfo.incidentId;
                    medicalSignature.incidentMedicalId = signatureInfo.incidentMedicalId;
                    medicalSignature.reportSigType = "M";
                    medicalSignature.reportSigUserId = signatureInfo.currentUser;
                    medicalSignature.staffName = signatureInfo.staffName;
                    medicalSignature.staffTitle = signatureInfo.staffTitle;
                    medicalSignature.approvalStatusId = 3;
                    medicalSignature.reportSigStamp = DateTime.Now;
                    medicalSignature.reportSigStation = signatureInfo.stationName;

                    this._db.ReportSigns.Add(medicalSignature);
                    this._db.SaveChanges();
                }
                else
                {
                    throw new Exception("Medical Assessment could not be found.");
                } // if (medicalAssessment != null)

            }
            else
            {
                throw new Exception("Unable to validate signature.  Please use your current CFS account password to sign.");
            } // if (userVerified)

        }
       
       

        

        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}