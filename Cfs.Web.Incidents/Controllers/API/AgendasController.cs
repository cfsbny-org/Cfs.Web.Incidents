using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Http;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;


namespace Cfs.Web.Incidents.Controllers.API
{
    public class AgendasController : ApiController
    {

        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();


        public string Post([FromBody]Models.Presentation.AgendaPostModel agenda)
        {
            string filePath = @"\\844dc2\Residential Incidents\";
            string fileName = string.Format("{0}{1}.docx", filePath, agenda.agendaTitle);

            //MemoryStream documentStream = new MemoryStream();

            
            using (WordprocessingDocument agendaDocument = WordprocessingDocument.Create(fileName, WordprocessingDocumentType.Document))
            //using (WordprocessingDocument agendaDocument = WordprocessingDocument.Create(documentStream, WordprocessingDocumentType.Document))
            {

                var reports = from r in this._db.IncidentReports
                              join u in this._db.Users
                                on r.userId equals u.userId
                              join p in this._db.IncidentPrograms
                                on r.programId equals p.incidentProgramId
                              join s in this._db.ReportStatuses
                                on r.statusId equals s.reportStatusId
                              from d in this._db.IncidentDetails.Where(d => d.incidentId == r.incidentId).DefaultIfEmpty()
                              where r.incidentReportTypeId == 1
                                    && r.incidentDate >= agenda.fromDate
                                    && r.incidentDate <= agenda.toDate
                              select new
                              {
                                  incidentId = r.incidentId,
                                  clientName = r.clientName,
                                  clientDob = r.clientDob,
                                  programTitle = p.programTitle,
                                  reportingAgency = r.reportingAgency,
                                  incidentDate = r.incidentDate,
                                  createdStamp = r.createdStamp,
                                  lastModified = r.lastModified,
                                  createdByName = u.firstName + " " + u.lastName,
                                  statusId = r.statusId,
                                  currentStatus = s.reportStatusText,
                                  incidentDetails = d.incidentDetails,
                                  staffs = (
                                                from st in this._db.IncidentStaffs
                                                join e in this._db.Users
                                                        on st.userId equals e.userId
                                                where st.incidentId == r.incidentId
                                                select new
                                                {
                                                    staffName = e.firstName + " " + e.lastName
                                                }
                                            )
                              };




                MainDocumentPart root = agendaDocument.AddMainDocumentPart();

                root.Document = new Document();
                Body body = root.Document.AppendChild(new Body());


                // PAGE MARGINS
                SectionProperties sectionProperties = new SectionProperties();
                PageMargin pageMargin = new PageMargin() { Top = 720, Right = (UInt32Value)720U, Bottom = 720, Left = (UInt32Value)720U, Header = (UInt32Value)720U, Footer = (UInt32Value)720U, Gutter = (UInt32Value)0U };
                sectionProperties.Append(pageMargin);
                root.Document.Body.Append(sectionProperties);

                // DOCUMENT STYLES
                StyleDefinitionsPart styleDefinitionsPart = root.AddNewPart<StyleDefinitionsPart>();
                Styles styles = new Styles();
                styles.Save(styleDefinitionsPart);

                Style style = new Style()
                {
                    Type = StyleValues.Paragraph,
                    StyleId = "AgendaStyle",
                    CustomStyle = true
                };
                StyleName styleHeading1 = new StyleName() { Val = "Heading1" };
                style.Append(styleHeading1);
                StyleRunProperties styleRunPropertiesHeading1 = new StyleRunProperties();
                styleRunPropertiesHeading1.Append(new Bold());
                styleRunPropertiesHeading1.Append(new RunFonts() { Ascii = "Calibri" });
                styleRunPropertiesHeading1.Append(new FontSize() { Val = "24" });  // Sizes are in half-points. Oy!
                style.Append(styleRunPropertiesHeading1);
                styles.Append(style);


                foreach (var report in reports)
                {
                    //Paragraph para = body.AppendChild(new Paragraph());
                    //Run run = para.AppendChild(new Run());

                    // CLIENT NAME HEADER
                    Paragraph clientNamePara = body.AppendChild(new Paragraph());
                    clientNamePara.AppendChild(new Run(new Text(report.clientName)));
                    clientNamePara.ParagraphProperties = new ParagraphProperties(new ParagraphStyleId() { Val = "Heading1" });


                    // REPORT DETAILS
                    Paragraph infoPara = body.AppendChild(new Paragraph());
                    Run infoRun = infoPara.AppendChild(new Run());

                    infoRun.AppendChild(new Text(string.Format("Program: {0}", report.programTitle)));
                    infoRun.AppendChild(new Break());
                    infoRun.AppendChild(new Text(string.Format("Reporting Agency: {0}", report.reportingAgency)));
                    infoRun.AppendChild(new Break());
                    infoRun.AppendChild(new Text(string.Format("Date of Incident: {0}", report.incidentDate.ToShortDateString())));
                    infoRun.AppendChild(new Break());
                    infoRun.AppendChild(new Text(string.Format("Staff: {0}", report.createdByName)));


                    // INCIDENT DETAILS
                    Paragraph detailsPara = body.AppendChild(new Paragraph());
                    Run detailsRun = detailsPara.AppendChild(new Run());

                    detailsRun.AppendChild(new Text("Details of Incident: "));
                    detailsRun.AppendChild(new Break());

                    if (report.incidentDetails == string.Empty)
                    {
                        detailsRun.AppendChild(new Text("<No details given.  Report incomplete.>"));
                    }
                    else
                    {
                        detailsRun.AppendChild(new Text(report.incidentDetails));
                    }



                    // ADDITIONAL STAFF INVOLVED
                    Paragraph staffPara = body.AppendChild(new Paragraph());
                    Run staffRun = staffPara.AppendChild(new Run());

                    staffRun.AppendChild(new Text("Additional Staff Involved:"));
                    staffRun.AppendChild(new Break());


                    if (report.staffs.ToList().Any())
                    {
                        foreach (var staff in report.staffs)
                        {
                            staffRun.AppendChild(new Text(staff.staffName));
                            staffRun.AppendChild(new Break());
                        }
                    }
                    else
                    {
                        staffRun.AppendChild(new Text("No additional staff identified."));
                        staffRun.AppendChild(new Break());
                    }




                    // ACTIONS TAKEN
                    Paragraph actionsPara = body.AppendChild(new Paragraph());
                    Run actionsRun = actionsPara.AppendChild(new Run(new Text("Actions Taken")));


                    // PATTERNS
                    Paragraph patternsPara = body.AppendChild(new Paragraph());
                    Run patternsRun = patternsPara.AppendChild(new Run(new Text("Pattern Behavior/Recommendation")));



                    // PAGE BREAK
                    body.AppendChild(new Paragraph(
                        new Run(
                            new Break() { Type = BreakValues.Page })));

                }

                //run.AppendChild(new Text("From Date: " + agenda.fromDate.ToShortDateString() + " to "  + agenda.toDate.ToShortDateString()));


               

            }


            SessionController session = new SessionController();

            var user = session.Get();
            session.Dispose();
            
            FileStream documentStream = new FileStream(fileName, FileMode.Open);


            MailMessage msg = new MailMessage();

            msg.To.Add(new MailAddress(user.userEmail));
            //msg.Bcc.Add(new MailAddress("jbowen@cfsbny.org"));
            msg.From = new MailAddress("no-reply@cfsbny.org");
            msg.Subject = "CFS Incident Reports: Agenda Document";
            msg.IsBodyHtml = true;


            msg.Attachments.Add(new System.Net.Mail.Attachment(documentStream, agenda.agendaTitle + ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"));

            StringBuilder messageBody = new StringBuilder();
            messageBody.Append("<h1>Incident Reports Agenda</h1>");
            messageBody.Append("<p>An agenda document has been created and is attached.</p>");
            messageBody.Append("<p>A copy has been saved <a href=\"\\\\844dc2\\Residential Incidents\\\">here</a>.</p>");

            msg.Body = messageBody.ToString();

            SmtpClient smtp = new SmtpClient("cfs-mailserv");
            smtp.Send(msg);


            smtp.Dispose();
            msg.Dispose();

            documentStream.Close();
            documentStream.Dispose();

            return fileName;
        }





        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}