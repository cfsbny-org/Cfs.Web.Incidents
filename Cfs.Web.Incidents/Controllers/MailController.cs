using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Cfs.Web.Incidents.Controllers
{
    public class MailController : Controller
    {



        public void SendMail(List<string> sendTos, string from, string subject, MailPriority priority, StringBuilder messageBody)
        {

            string emailCss = string.Empty;


            XmlDocument doc = new XmlDocument();
            doc.Load(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/style.xml"));

            XmlNode root = doc.DocumentElement;

            XmlNode node = root.SelectSingleNode("style[@name='default']");
            emailCss = node.InnerText;


            MailMessage msg = new MailMessage();

            foreach (string sendTo in sendTos)
            {
                msg.To.Add(new MailAddress(sendTo));
            }

            msg.From = new MailAddress(from);
            msg.Priority = priority;
            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = string.Format("<style type=\'text/css\'>{0}</style>{1}", emailCss, messageBody.ToString());

            SmtpClient smtp = new SmtpClient("cfs-mailserv");
            smtp.Send(msg);
            smtp.Dispose();

            msg.Dispose();
        }




        public void SendMail(List<string> sendTos, string from, string subject, MailPriority priority, string messageBody)
        {

            string emailCss = string.Empty;


            XmlDocument doc = new XmlDocument();
            doc.Load(System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/style.xml"));

            XmlNode root = doc.DocumentElement;

            XmlNode node = root.SelectSingleNode("style[@name='default']");
            emailCss = node.InnerText;


            MailMessage msg = new MailMessage();

            foreach (string sendTo in sendTos)
            {
                msg.To.Add(new MailAddress(sendTo));
            }

            msg.From = new MailAddress(from);
            msg.Priority = priority;
            msg.Subject = subject;
            msg.IsBodyHtml = true;
            msg.Body = string.Format("<style type=\'text/css\'>{0}</style>{1}", emailCss, messageBody.ToString());

            SmtpClient smtp = new SmtpClient("cfs-mailserv");
            smtp.Send(msg);
            smtp.Dispose();

            msg.Dispose();
        }



        private const string singleLineBreak = "<br />";
        private const string doubleLineBreak = "<br /><br />";

        public void SendExceptionDetail(string apiRoute, string errorMessage, string stackTrace, string currentUser, object entityObject)
        {

            PropertyInfo[] objectProperties = entityObject.GetType().GetProperties();

            StringBuilder msgBody = new StringBuilder();


            msgBody.Append(string.Format("Page: {0}; User: {1}", apiRoute, currentUser)).Append(doubleLineBreak);
            msgBody.Append(errorMessage).Append(doubleLineBreak);
            msgBody.Append(stackTrace).Append(doubleLineBreak);

            foreach (PropertyInfo objectProperty in objectProperties)
            {
                string propertyValue = string.Empty;

                if (objectProperty.GetValue(entityObject, null) == null)
                {
                    propertyValue = "<b style=\"color: #ff0000;\">(null)</b>";
                }
                else
                {
                    propertyValue = objectProperty.GetValue(entityObject, null).ToString();
                }

                msgBody.Append(string.Format("{0}: {1}{2}", objectProperty.Name, propertyValue, singleLineBreak));
            }

            //objectProperty.GetValue(entityObject, null).ToString()





            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress("jbowen@cfsbny.org"));
            msg.From = new MailAddress("techservices@cfsbny.org");
            msg.Subject = "Non-Residential Incident Reports Exception";
            msg.IsBodyHtml = true;
            msg.Body = msgBody.ToString();

            SmtpClient smtp = new SmtpClient("cfs-mailserv");
            smtp.Send(msg);

            msg.Dispose();
            smtp.Dispose();


        }

    }
}