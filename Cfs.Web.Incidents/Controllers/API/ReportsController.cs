using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class ReportsController : ApiController
    {



        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();


        [Route("api/reports/user/{id}")]
        public IQueryable<Models.Presentation.ReportsViewModel> GetUserReports(long id)
        {
            var reports = from r in this._db.IncidentReports
                          join u in this._db.Users
                            on r.userId equals u.userId
                          join p in this._db.IncidentPrograms
                            on r.programId equals p.incidentProgramId
                          join s in this._db.ReportStatuses
                            on r.statusId equals s.reportStatusId
                          where s.reportIsActive == true
                                && r.userId == id
                                || (
                                    from n in this._db.Notifications
                                    where n.incidentId == r.incidentId
                                            && n.acknowledgeUserId == id
                                    select n.incidentId
                                ).Any()
                          select new Models.Presentation.ReportsViewModel
                          {
                              incidentId = r.incidentId,
                              incidentReportTypeId = r.incidentReportTypeId,
                              clientName = r.clientName,
                              programTitle = p.programTitle,
                              incidentDate = r.incidentDate,
                              isApproximate = r.isApproximate,
                              createdStamp = r.createdStamp,
                              lastModified = r.lastModified,
                              lastModifiedByName = u.lastName + ", " + u.firstName,
                              statusId = r.statusId,
                              currentStatus = s.reportStatusText,
                              medicalCompleted = (from m in this._db.Medicals
                                                  where m.incidentId == r.incidentId
                                                  select m).Any()
                          };



            return reports;
        }



        [Route("api/reports/supervisor/{id}")]
        public IQueryable<Models.Presentation.ReportsViewModel> GetSuperviorReports(long id)
        {
            var reports = from r in this._db.IncidentReports
                          join u in this._db.Users
                            on r.userId equals u.userId
                          join p in this._db.IncidentPrograms
                            on r.programId equals p.incidentProgramId
                          join s in this._db.ReportStatuses
                            on r.statusId equals s.reportStatusId
                          join rs in this._db.ReportSigns
                            on r.incidentId equals rs.incidentId
                          where s.reportIsActive == true && rs.reportSigType == "S" && rs.reportSigUserId == id && rs.approvalStatusId == 1
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
                              currentStatus = s.reportStatusText,
                              medicalCompleted = (from m in this._db.Medicals
                                                  where m.incidentId == r.incidentId
                                                  select m).Any()
                          };



            return reports;
        }


        [Route("api/reports/admin/")]
        public IQueryable<Models.Presentation.ReportsViewModel> GetAdminReports()
        {
            var reports = from r in this._db.IncidentReports
                          join u in this._db.Users
                            on r.userId equals u.userId
                          join p in this._db.IncidentPrograms
                            on r.programId equals p.incidentProgramId
                          join s in this._db.ReportStatuses
                            on r.statusId equals s.reportStatusId
                          join rs in this._db.ReportSigns
                            on r.incidentId equals rs.incidentId
                          where rs.reportSigType == "A" && rs.approvalStatusId == 1
                                && (r.statusId != 6 && r.statusId != 7)
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
                              currentStatus = s.reportStatusText,
                              medicalCompleted = (from m in this._db.Medicals
                                                  where m.incidentId == r.incidentId
                                                  select m).Any()
                          };



            return reports;
        }



        [Route("api/reports/open")]
        public IQueryable<Models.Presentation.ReportsViewModel> GetOpenReports()
        {
            var reports = from r in this._db.IncidentReports
                          join u in this._db.Users
                            on r.userId equals u.userId
                          join p in this._db.IncidentPrograms
                            on r.programId equals p.incidentProgramId
                          join s in this._db.ReportStatuses
                            on r.statusId equals s.reportStatusId
                          //from rs in this._db.ReportSigns.Where( rs => rs.incidentId == r.incidentId).DefaultIfEmpty()
                          //on r.incidentId equals rs.incidentId
                          where s.reportIsActive == true
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



        [Route("api/reports/header/{id}")]
        public Models.Presentation.ReportsViewModel GetReportHeader(long id)
        {
            var reports = (from r in this._db.IncidentReports
                           join cu in this._db.Users
                                on r.userId equals cu.userId
                           join lu in this._db.Users
                             on r.lastModifiedBy equals lu.userId
                           join p in this._db.IncidentPrograms
                             on r.programId equals p.incidentProgramId
                           join s in this._db.ReportStatuses
                             on r.statusId equals s.reportStatusId
                           where r.incidentId == id
                           select new Models.Presentation.ReportsViewModel
                           {
                               incidentId = r.incidentId,
                               userId = r.userId,
                               staffName = r.staffName,
                               clientName = r.clientName,
                               programTitle = p.programTitle,
                               incidentLocation = r.incidentLocation,
                               createdByName = cu.lastName + ", " + cu.firstName,
                               createdStamp = r.createdStamp,
                               incidentDate = r.incidentDate,
                               isApproximate = r.isApproximate,
                               lastModified = r.lastModified,
                               lastModifiedByName = lu.lastName + ", " + lu.firstName,
                               statusId = r.statusId,
                               currentStatus = s.reportStatusText
                           }).FirstOrDefault();



            return reports;
        }



        public IQueryable<Models.IncidentReport> Print_GetIncidentReport(long id)
        {
            return this._db.IncidentReports.Where(r => r.incidentId == id);
        }


        public IQueryable<Models.Presentation.ReportsViewModel> PrintReportHeader(long id)
        {
            var reports = from r in this._db.IncidentReports

                          join p in this._db.IncidentPrograms
                            on r.programId equals p.incidentProgramId
                          join s in this._db.ReportStatuses
                            on r.statusId equals s.reportStatusId
                          where r.incidentId == id
                          select new Models.Presentation.ReportsViewModel
                          {
                              incidentId = r.incidentId,
                              clientName = r.clientName,
                              clientId = r.clientId,
                              clientDob = r.clientDob,
                              clientGender = r.clientGender,
                              staffName = r.staffName,
                              staffTitle = r.staffTitle,
                              programTitle = p.programTitle,
                              reportingAgency = r.reportingAgency,
                              incidentLocation = r.incidentLocation,
                              createdStamp = r.createdStamp,
                              incidentDate = r.incidentDate,
                              statusId = r.statusId
                          };



            return reports;
        }




        [Route("api/reports/open/{reportType}")]
        public IQueryable<Models.Presentation.ReportsViewModel> GetOpenReports(int reportType)
        {
            var reports = from r in this._db.IncidentReports
                          join u in this._db.Users
                            on r.userId equals u.userId
                          join p in this._db.IncidentPrograms
                            on r.programId equals p.incidentProgramId
                          join s in this._db.ReportStatuses
                            on r.statusId equals s.reportStatusId
                          where r.incidentReportTypeId == reportType && s.reportIsActive == true
                          select new Models.Presentation.ReportsViewModel
                          {
                              incidentId = r.incidentId,
                              clientName = r.clientName,
                              programTitle = p.programTitle,
                              incidentDate = r.incidentDate,
                              createdStamp = r.createdStamp,
                              lastModified = r.lastModified,
                              createdByName = u.lastName + ", " + u.firstName,
                              statusId = r.statusId,
                              currentStatus = s.reportStatusText,
                              medicals = (from m in this._db.Medicals
                                          join st in this._db.ReportStatuses
                                                on m.statusId equals st.reportStatusId
                                          where m.incidentId == r.incidentId
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
                                              currentStatus = st.reportStatusText,
                                              isOpen = st.reportIsActive
                                          })

                          };



            return reports;
        }




        [HttpPost, Route("api/reports/approve")]
        public void FinalApprove([FromBody]Models.Presentation.ReportSignatureModel signatureInfo)
        {
            Models.IncidentReport report = this._db.IncidentReports.Where(r => r.incidentId == signatureInfo.incidentId).SingleOrDefault();

            if (report != null)
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

                    Models.ReportSign staffSignature = new Models.ReportSign();

                    staffSignature.incidentId = signatureInfo.incidentId;
                    staffSignature.incidentMedicalId = 0;
                    staffSignature.reportSigType = "E";
                    staffSignature.reportSigUserId = signatureInfo.currentUser;
                    staffSignature.staffName = signatureInfo.staffName;
                    staffSignature.staffTitle = signatureInfo.staffTitle;
                    staffSignature.approvalStatusId = 3;
                    staffSignature.reportSigStamp = DateTime.Now;
                    staffSignature.reportSigStation = signatureInfo.stationName;

                    this._db.ReportSigns.Add(staffSignature);



                    StaffController staffs = new StaffController();
                    Models.User supervisor = staffs.GetStaffSupervisor(signatureInfo.currentUser);
                    staffs.Dispose();


                    Models.ReportSign supervisorSignature = new Models.ReportSign();

                    supervisorSignature.incidentId = signatureInfo.incidentId;
                    supervisorSignature.incidentMedicalId = 0;
                    supervisorSignature.reportSigType = "S";
                    supervisorSignature.reportSigUserId = supervisor.userId;
                    supervisorSignature.staffName = supervisor.firstName + " " + supervisor.lastName;
                    supervisorSignature.staffTitle = supervisor.jobTitle;
                    supervisorSignature.approvalStatusId = 1;

                    this._db.ReportSigns.Add(supervisorSignature);



                    // WRITE CHANGES TO LOG

                    Models.ReportLog log = new Models.ReportLog();

                    log.incidentId = signatureInfo.incidentId;
                    log.userId = signatureInfo.currentUser;
                    log.userStation = signatureInfo.stationName;
                    log.logDateTime = DateTime.Now;
                    log.logDetails = "Report signed by staff.";

                    this._db.ReportLogs.Add(log);



                    // EMAIL SUPERVISOR!!!! (INCLUDE ADMINS?)


                    MailController mailer = new MailController();
                    StringBuilder messageBody = new StringBuilder();
                    messageBody.Append("<p>A new incident report for <b>" + report.clientName + "</b> has been posted by " + report.staffName + ".</p>");
                    messageBody.Append("<p><a href=\"http://cfs-incidents/report/residential/" + report.incidentId.ToString() + "\">Click here to view the report.</a></p>");

                    mailer.SendMail(
                            new List<string>() { supervisor.eMail, "residentialincidents@cfsbny.org" },
                            "techservices@cfsbny.org",
                            "Incident Report Posted",
                            System.Net.Mail.MailPriority.High,
                            messageBody
                            );

                    mailer.Dispose();





                    report.statusId = signatureInfo.statusId;
                    report.currentUser = signatureInfo.currentUser;
                    report.lastModified = DateTime.Now;
                    report.lastModifiedBy = signatureInfo.currentUser;

                    this._db.IncidentReports.Attach(report);
                    this._db.Entry(report).State = System.Data.Entity.EntityState.Modified;






                    this._db.SaveChanges();
                }
                else
                {
                    throw new Exception("Unable to validate signature.  Please use your current CFS account password to sign.");
                }

            }

        }




        [HttpPost, Route("api/reports/supervisor/approve")]
        public void SupervisorFinalApprove([FromBody]Models.Presentation.ReportSignatureModel signatureInfo)
        {
            Models.IncidentReport report = this._db.IncidentReports.Where(r => r.incidentId == signatureInfo.incidentId).SingleOrDefault();
            string logDetails = string.Empty;


            if (report != null)
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
                    // GET SUPERVISOR SIGNATURE RECORD
                    Models.ReportSign supervisorSignature = this._db.ReportSigns.Where(
                        s => s.incidentId == signatureInfo.incidentId
                        && s.reportSigType == "S"
                        && s.reportSigUserId == signatureInfo.currentUser).SingleOrDefault();

                    if (supervisorSignature == null)
                    {
                        throw new Exception("Could not find signature record.  Cannot approve report.");
                    }
                    else
                    {
                        supervisorSignature.approvalStatusId = signatureInfo.approvalStatusId;
                        supervisorSignature.reportSigStamp = DateTime.Now;
                        supervisorSignature.reportSigStation = signatureInfo.stationName;
                        supervisorSignature.approvalComments = signatureInfo.approvalComments;

                        this._db.ReportSigns.Attach(supervisorSignature);
                        this._db.Entry(supervisorSignature).State = System.Data.Entity.EntityState.Modified;



                        if (signatureInfo.approvalStatusId == 3) // SUPERVISOR APPROVED
                        {

                            // SUPERVISOR APPROVES, REPORT SENT TO ADMINS FOR REVIEW
                            Models.ReportSign adminSignature = new Models.ReportSign();

                            adminSignature.incidentId = signatureInfo.incidentId;
                            adminSignature.incidentMedicalId = 0;
                            adminSignature.reportSigType = "A";
                            adminSignature.reportSigUserId = 0;
                            adminSignature.staffName = "Administrator";
                            adminSignature.staffTitle = "Administrator";
                            adminSignature.approvalStatusId = 1;

                            this._db.ReportSigns.Add(adminSignature);

                            // EMAIL ADMINS!!!! (INCLUDE ADMINS?)

                            logDetails = "Supervisor approved report.";
                        }
                        else
                        {
                            // NOTIFY EMPLOYEE REPORT REJECTED


                            logDetails = "Supervisor rejected report. Comments: " + signatureInfo.approvalComments;

                        } // if (signatureInfo.approvalStatusId == 3) 


                        // UPDATE REPORT STATUS 

                        report.statusId = signatureInfo.statusId;
                        report.currentUser = signatureInfo.currentUser;
                        report.lastModified = DateTime.Now;
                        report.lastModifiedBy = signatureInfo.currentUser;

                        this._db.IncidentReports.Attach(report);
                        this._db.Entry(report).State = System.Data.Entity.EntityState.Modified;


                        // WRITE CHANGES TO LOG

                        Models.ReportLog log = new Models.ReportLog();

                        log.incidentId = signatureInfo.incidentId;
                        log.userId = signatureInfo.currentUser;
                        log.userStation = signatureInfo.stationName;
                        log.logDateTime = DateTime.Now;
                        log.logDetails = logDetails;

                        this._db.ReportLogs.Add(log);



                        // IF JUSTICE CENTER CALLED, NOTIFY CORPORATE COMPLIANCE


                        // notifyPartyId = 8 (Justice Center)
                        bool jcCalled = this._db.Notifications.Where(n => n.incidentId == signatureInfo.incidentId && n.notifyPartyId == 8).Any();

                        if (jcCalled)
                        {
                            Models.Notification ccNotification = new Models.Notification();

                            ccNotification.incidentId = signatureInfo.incidentId;
                            ccNotification.notifyPartyId = 37;  // Corporate Compliance
                            ccNotification.notifyDateTime = DateTime.Now;
                            ccNotification.notifyContact = "CFS Corporate Compliance";
                            ccNotification.notifyMethod = "E-Mail";
                            ccNotification.notifyStaffId = 0;
                            ccNotification.isAcknowledged = 1;
                            ccNotification.acknowledgeUserId = 0;

                            this._db.Notifications.Add(ccNotification);

                            MailController mailer = new MailController();

                            List<string> sendTos = new List<string>();
                            sendTos.Add("CorpCompIncidents@cfsbny.org");

                            StringBuilder msg = new StringBuilder();
                            msg.Append("<h1>Incident Report Notification</h1>");
                            msg.Append("<p>An incident report has been created for client " + report.clientName + " by " + report.staffName);
                            msg.Append(", and the Justice Center was called.</p>");

                            mailer.SendMail(sendTos, "techservices@cfsbny.org", "Incident Reports: Justice Center Called", System.Net.Mail.MailPriority.Normal, msg);
                        }



                        this._db.SaveChanges();

                    }  // if (supervisorSignature == null)





                }
                else
                {
                    throw new Exception("Unable to validate signature.  Please use your current CFS account password to sign.");
                }  // if (userVerified)
            }
        }



        [HttpPost, Route("api/reports/admin/lock")]
        public void LockReport([FromBody]Models.Presentation.ReportStatusChangeModel reportStatus)
        {
            var report = this._db.IncidentReports.Where(r => r.incidentId == reportStatus.incidentId).SingleOrDefault();

            if (report != null)
            {
                report.statusId = 5;  // Under Aministrative Review
                report.currentUser = reportStatus.currentUser;
                report.lastModifiedBy = reportStatus.lastModifiedBy;
                report.lastModified = DateTime.Now;

                this._db.IncidentReports.Attach(report);
                this._db.Entry(report).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();
            }
            else
            {
                throw new Exception("Report could not be found.");
            }
        }




        [HttpPost, Route("api/reports/admin/approve")]
        public void AdminFinalApprove([FromBody]Models.Presentation.ReportSignatureModel signatureInfo)
        {
            Models.IncidentReport report = this._db.IncidentReports.Where(r => r.incidentId == signatureInfo.incidentId).SingleOrDefault();

            if (report != null)
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
                    
                    report.statusId = signatureInfo.statusId;
                    report.lastModified = DateTime.Now;
                    report.lastModifiedBy = signatureInfo.currentUser;

                    this._db.SaveChanges();


                    PrintController printer = new PrintController();
                    printer.SaveToEbook(report, signatureInfo.ebookFolder);

                    printer.Dispose();


                }
                else
                {
                    throw new Exception("Unable to validate signature.  Please use your current CFS account password to sign.");
                }
            }
        }






        public long Post([FromBody]Models.IncidentReport report)
        {
            if (report.userId == 0)
            {
                SessionController session = new SessionController();
                var user = session.Get();

                report.userId = user.userId;
                report.createdStation = user.stationInfo;
                report.currentUser = user.userId;

                session.Dispose();
            }

            if (report.incidentId == 0)
            {
                // CREATE REPORT
                this._db.IncidentReports.Add(report);



            }
            else
            {
                this._db.IncidentReports.Attach(report);
                this._db.Entry(report).State = System.Data.Entity.EntityState.Modified;
            }

            try
            {
                this._db.SaveChanges();


                // WRITE TO REPORT LOG
                Models.ReportLog log = new Models.ReportLog();
                log.incidentId = report.incidentId;
                log.userId = report.userId;
                log.userStation = report.createdStation;
                log.logDateTime = DateTime.Now;
                log.logDetails = "Report created.";

                LogController logController = new LogController();
                logController.Post(log);
                logController.Dispose();


                // NOTIFY
                MailController mailer = new MailController();



                StringBuilder messageBody = new StringBuilder();

                messageBody.Append("<p>A new incident report for <b>" + report.clientName + "</b> has been created by " + report.staffName + ".</p>");
                messageBody.Append("<p><a href=\"http://cfs-incidents/report/residential/" + report.incidentId.ToString() + "\">Click here to view the report.</a></p>");

                if (report.incidentReportTypeId == 1)
                {
                    mailer.SendMail(
                            new List<string>() { "residentialincidents@cfsbny.org" },
                            "techservices@cfsbny.org",
                            "New Incident Report",
                            System.Net.Mail.MailPriority.High,
                            messageBody
                            );
                }
                else
                {
                    mailer.SendMail(
                            new List<string>() { "clinicincidents@cfsbny.org" },
                            "techservices@cfsbny.org",
                            "New Incident Report",
                            System.Net.Mail.MailPriority.High,
                            messageBody
                            );
                }

                mailer.Dispose();

                return report.incidentId;

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
                           "ERROR CREATING INCIDENT: VALIDATION",
                           System.Net.Mail.MailPriority.High,
                           exceptionMessage
                           );

                string currentUser = RequestContext.Principal.Identity.Name;

                mailer.SendExceptionDetail("post:/api/reports", exceptionMessage, ex.StackTrace, currentUser, report);


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
                           "ERROR CREATING INCIDENT",
                           System.Net.Mail.MailPriority.High,
                           errorMessage
                           );

                string currentUser = RequestContext.Principal.Identity.Name;

                mailer.SendExceptionDetail("post:/api/reports", errorMessage, ex.StackTrace, currentUser, report);

                throw new Exception(errorMessage);
            }


        }


        [Route("api/reports/status")]
        public void PostReportStatus([FromBody]Models.Presentation.ReportStatusChangeModel statusInfo)
        {
            Models.IncidentReport report = this._db.IncidentReports.Where(r => r.incidentId == statusInfo.incidentId).SingleOrDefault();

            if (report != null)
            {
                report.statusId = statusInfo.statusId;
                report.currentUser = statusInfo.currentUser;
                report.lastModified = statusInfo.lastModified;
                report.lastModifiedBy = statusInfo.lastModifiedBy;

                this._db.IncidentReports.Attach(report);
                this._db.Entry(report).State = System.Data.Entity.EntityState.Modified;

                this._db.SaveChanges();
            }



        }




        [HttpGet, Route("api/reports/void/{id}")]
        public void VoidReport(long id)
        {

            var report = this._db.IncidentReports.Where(r => r.incidentId == id).SingleOrDefault();

            if (report != null)
            {

                SessionController session = new SessionController();
                var user = session.Get();

                
                report.statusId = 7;
                report.lastModified = DateTime.Now;
                report.lastModifiedBy = user.userId;


                session.Dispose();

                this._db.IncidentReports.Attach(report);
                this._db.Entry(report).State = System.Data.Entity.EntityState.Modified;

                this._db.SaveChanges();
            }
        }




        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }


    }
}