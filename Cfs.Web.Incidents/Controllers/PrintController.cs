using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Reports = Cfs.Web.Incidents.Content.Reports;

namespace Cfs.Web.Incidents.Controllers
{
    public class PrintController : Controller
    {


        public FileStreamResult OpenIncidents()
        {
            Reports.OpenReports report = new Reports.OpenReports();
            report.Load();

            API.ReportsController reportsController = new API.ReportsController();

            report.SetDataSource(reportsController.GetUserReports(80).ToList());

            reportsController.Dispose();

            Stream reportStream = report.ExportToStream(ExportFormatType.PortableDocFormat);

            report.Close();
            report.Dispose();

            return new FileStreamResult(reportStream, "application/pdf");
        }


        public FileStreamResult Test()
        {
            Reports.TestReport report = new Reports.TestReport();
            report.Load();

            SetReportDatabaseLogon(report);

            Stream reportStream = report.ExportToStream(ExportFormatType.PortableDocFormat);

            report.Close();
            report.Dispose();

            return new FileStreamResult(reportStream, "application/pdf");
        }




        public FileStreamResult Residential(long id)
        {

            
            try
            {
                Reports.ResidentialReport report = this.ResidentialReport(id);


                Stream reportStream = report.ExportToStream(ExportFormatType.PortableDocFormat);

                report.Close();
                report.Dispose();

                return new FileStreamResult(reportStream, "application/pdf");
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;

                if (ex.InnerException != null)
                {
                    errorMessage += "; " + ex.InnerException.Message;
                }

                errorMessage += Environment.NewLine;

                throw new Exception(errorMessage);
            }
        }



        public void SaveToEbook(Models.IncidentReport report, string path)
        {
            

            string document = @"\\844dc2\Conners\";

            if (report.isApproximate)
            {
                document += path + @"\Incident Report " + report.incidentDate.ToString("yyyy-MM") + ".pdf";
            }
            else
            {
                document += path + @"\Incident Report " + report.incidentDate.ToString("yyyy-MM-dd") + ".pdf";
            }



            try
            {
                Reports.ResidentialReport reportDocument = this.ResidentialReport(report.incidentId);

                reportDocument.ExportToDisk(ExportFormatType.PortableDocFormat, document);

                reportDocument.Close();
                reportDocument.Dispose();

                
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += ex.InnerException.Message;
                }
                throw new Exception(document + Environment.NewLine + errorMessage);
            }
        }



        private Content.Reports.ResidentialReport ResidentialReport(long id)
        {
            API.ReportsController reportsController = new API.ReportsController();
            API.ProgramsController programsController = new API.ProgramsController();
            API.TypesController typesController = new API.TypesController();
            API.RestraintsController restraintsController = new API.RestraintsController();
            API.DetailsController detailsController = new API.DetailsController();
            API.MedicalsController medicalsController = new API.MedicalsController();
            API.SignaturesController signaturesController = new API.SignaturesController();
            API.UsersController usersContoller = new API.UsersController();
            API.StaffController staffController = new API.StaffController();
            API.NotificationsController notificationsController = new API.NotificationsController();
            API.NotifyPartiesController notifyPartiesController = new API.NotifyPartiesController();
            API.CommentsController commentsController = new API.CommentsController();
            API.AttachmentsController attachmentsController = new API.AttachmentsController();

            var reportHeader = Models.Converters.ListToDataSet.ToDataTable<Models.IncidentReport>(reportsController.Print_GetIncidentReport(id).ToList());
            var programs = Models.Converters.ListToDataSet.ToDataTable<Models.IncidentProgram>(programsController.Get().ToList());
            var incidentTypes = Models.Converters.ListToDataSet.ToDataTable<Models.IncidentType>(typesController.Get().ToList());
            var reportIncidents = Models.Converters.ListToDataSet.ToDataTable<Models.ReportIncident>(typesController.Print_GetReportIncidents(id).ToList());
            var restraints = Models.Converters.ListToDataSet.ToDataTable<Models.Restraint>(restraintsController.Get(id).ToList());
            var incidentDetails = Models.Converters.ListToDataSet.ToDataTable<Models.IncidentDetail>(detailsController.Print(id).ToList());
            var medicals = Models.Converters.ListToDataSet.ToDataTable<Models.Medical>(medicalsController.Print_GetMedicals(id).ToList());
            var reportSignatures = Models.Converters.ListToDataSet.ToDataTable<Models.Presentation.ReportSignsViewModel>(signaturesController.Print(id).ToList());
            var reportSigTypes = Models.Converters.ListToDataSet.ToDataTable<Models.ReportSigType>(signaturesController.GetTypes().ToList());
            var users = Models.Converters.ListToDataSet.ToDataTable<Models.User>(usersContoller.Get().ToList());
            var incidentStaff = Models.Converters.ListToDataSet.ToDataTable<Models.IncidentStaff>(staffController.Get(id).ToList());
            var notifications = Models.Converters.ListToDataSet.ToDataTable<Models.Notification>(notificationsController.Print(id).ToList());
            var notifyParties = Models.Converters.ListToDataSet.ToDataTable<Models.NotifyParty>(notifyPartiesController.Print().ToList());
            var adminComments = Models.Converters.ListToDataSet.ToDataTable<Models.AdminComment>(commentsController.Print(id).ToList());
            var attachments = Models.Converters.ListToDataSet.ToDataTable<Models.IncidentAttachment>(attachmentsController.Print(id).ToList());



            reportsController.Dispose();
            programsController.Dispose();
            typesController.Dispose();
            restraintsController.Dispose();
            detailsController.Dispose();
            medicalsController.Dispose();
            signaturesController.Dispose();
            usersContoller.Dispose();
            staffController.Dispose();
            notificationsController.Dispose();
            notifyPartiesController.Dispose();
            commentsController.Dispose();
            attachmentsController.Dispose();


            Reports.ResidentialReport report = new Reports.ResidentialReport();
            report.Load();


            report.Database.Tables["IncidentReport"].SetDataSource(reportHeader);
            report.Database.Tables["IncidentPrograms"].SetDataSource(programs);

            var reportableIncidentsSubreport = report.Subreports["ReportableIncidentsSubreport"];
            var significantIncidentsSubreport = report.Subreports["SignificantIncidentsSubreport"];
            var internalEventsSubreport = report.Subreports["InternalEventsSubreport"];
            var restraintsSubreport = report.Subreports["RestraintsSubreport"];
            var incidentDetailsSubreport = report.Subreports["IncidentDetailsSubreport"];
            var notificationsSubreport = report.Subreports["NotificationsSubreport"];
            var medicalsSubreport = report.Subreports["MedicalsSubreport"];
            var staffSubreport = report.Subreports["StaffSubreport"];
            var adminCommentsSubreport = report.Subreports["AdminCommentsSubreport"];
            var signaturesSubreport = report.Subreports["SignaturesSubreport"];
            var attachmentsSubreport = report.Subreports["AttachmentsSubreport"];

            reportableIncidentsSubreport.Database.Tables["IncidentTypes"].SetDataSource(incidentTypes);
            reportableIncidentsSubreport.Database.Tables["ReportIncidents"].SetDataSource(reportIncidents);

            significantIncidentsSubreport.Database.Tables["IncidentTypes"].SetDataSource(incidentTypes);
            significantIncidentsSubreport.Database.Tables["ReportIncidents"].SetDataSource(reportIncidents);

            internalEventsSubreport.Database.Tables["IncidentTypes"].SetDataSource(incidentTypes);
            internalEventsSubreport.Database.Tables["ReportIncidents"].SetDataSource(reportIncidents);

            restraintsSubreport.Database.Tables["Restraints"].SetDataSource(restraints);

            incidentDetailsSubreport.Database.Tables["IncidentDetails"].SetDataSource(incidentDetails);

            notificationsSubreport.Database.Tables["Notifications"].SetDataSource(notifications);
            notificationsSubreport.Database.Tables["NotifyParties"].SetDataSource(notifyParties);
            notificationsSubreport.Database.Tables["Users"].SetDataSource(users);

            medicalsSubreport.Database.Tables["Medicals"].SetDataSource(medicals);
            medicalsSubreport.Database.Tables["Users"].SetDataSource(users);
            medicalsSubreport.Database.Tables["ReportSigns"].SetDataSource(reportSignatures);

            staffSubreport.Database.Tables["IncidentStaff"].SetDataSource(incidentStaff);

            adminCommentsSubreport.Database.Tables["AdminComments"].SetDataSource(adminComments);
            adminCommentsSubreport.Database.Tables["Users"].SetDataSource(users);

            signaturesSubreport.Database.Tables["ReportSigns"].SetDataSource(reportSignatures);
            signaturesSubreport.Database.Tables["ReportSigTypes"].SetDataSource(reportSigTypes);

            attachmentsSubreport.Database.Tables["IncidentAttachments"].SetDataSource(attachments);

            report.SetParameterValue("incidentId", id);

            return report;
        }


        


        private void SetReportDatabaseLogon(ReportDocument report)
        {

            ConnectionInfo ci = new ConnectionInfo();

            ci.ServerName = "cfs-intranet";
            ci.DatabaseName = "CFS_Master";
            
            ci.IntegratedSecurity = true;

            Tables tables = report.Database.Tables;

            foreach (Table table in tables)
            {
                TableLogOnInfo tableLogonInfo = new TableLogOnInfo();

                tableLogonInfo = table.LogOnInfo;
                tableLogonInfo.ConnectionInfo = ci;
                table.ApplyLogOnInfo(tableLogonInfo);

            }
        }




       

    }
}