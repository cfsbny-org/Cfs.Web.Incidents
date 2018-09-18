using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class SessionController : ApiController
    {


        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();



        

        public Models.Presentation.UserSessionModel Get()
        {
            string username = RequestContext.Principal.Identity.Name.Substring(5);

            string stationInfo = System.Web.HttpContext.Current.Request.UserHostAddress;
            
            try
            {
                stationInfo += ";" + System.Net.Dns.GetHostEntry(stationInfo).HostName;
            }
            catch
            {
                stationInfo = "Unknown";
            }



            var user = (from u in this._db.Users
                        join s in this._db.Users
                            on u.supervisorId equals s.userId
                        where u.userName == username
                        select new Models.Presentation.UserSessionModel
                        {
                            userId = u.userId,
                            userName = u.userName,
                            firstName = u.firstName,
                            lastName = u.lastName,
                            fullName = u.firstName + " " + u.lastName,
                            jobTitle = u.jobTitle,
                            userEmail = u.eMail,
                            supervisorId = u.supervisorId,
                            supervisorName = s.firstName + " " + s.lastName,
                            supervisorEmail = s.eMail
                        }).SingleOrDefault();

            user.stationInfo = stationInfo;

            return user;
        }




        public bool VerifyPassword(string userName, string password)
        {
            DirectoryEntry domain = new DirectoryEntry("LDAP://main.cfsbny.org", @"MAIN\" + userName, password);

            
            using (domain)
            {
                try
                {
                    object nativeObject = domain.NativeObject;
                    DirectorySearcher searcher = new DirectorySearcher(domain);

                    searcher.Filter = "(SAMAccountName=" + userName + ")";
                    //searcher.PropertiesToLoad.Add("cn");
                    SearchResult result = searcher.FindOne();

                    if (result == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message.Trim();
                    if (ex.InnerException != null)
                    {
                        errorMessage += Environment.NewLine + "Inner Exception: " + ex.InnerException.Message;
                    }
                    throw new Exception(errorMessage);
                    
                }
            }

        }


        [HttpGet, Route("api/report/user/validate")]
        public bool ValidateReportUser(long r, long u)
        {
            long reportId = r;
            long userId = u;
            //if (HttpContext.Current.User.IsInRole(@"MAIN\IS"))
            //{
            //    return true;
            //}

            if (RequestContext.Principal.IsInRole(@"MAIN\IncidentAdmins") || RequestContext.Principal.IsInRole(@"MAIN\Nurse"))
            {
                return true;
            }
            else
            {

                Models.IncidentReport report = this._db.IncidentReports.Where(i => i.incidentId == reportId).SingleOrDefault();

                if (report != null)
                {
                    if (report.userId == userId)
                    {
                        return true;
                    }
                    else
                    {
                        var notifications = this._db.Notifications.Where(n => n.incidentId == reportId && n.notifyStaffId == userId);

                        if (notifications.Any())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }


                    }
                }
                else
                {
                    return false;
                }

            }
        }



        [HttpPost, Route("api/user/validate")]
        public HttpResponseMessage ValidateUser([FromBody]string password)
        {

            HttpResponseMessage response = new HttpResponseMessage();

            string userName = HttpContext.Current.User.Identity.Name.Substring(5).ToLower();
            

            DirectoryEntry domain = new DirectoryEntry("LDAP://main.cfsbny.org", @"MAIN\" + userName, password);

            using (domain)
            {
                try
                {
                object nativeObject = domain.NativeObject;
                DirectorySearcher searcher = new DirectorySearcher(domain);

                searcher.Filter = "(SAMAccountName=" + userName + ")";
                //searcher.PropertiesToLoad.Add("cn");
                SearchResult result = searcher.FindOne();

                if (result == null)
                {
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Content = new StringContent("Unable to validate user.  User name and password do not match.");

                    throw new HttpResponseException(response);
                }


                }
                catch (Exception ex)
                {
                    string errorMessage = ex.Message.Trim();
                    if (ex.InnerException != null)
                    {
                        errorMessage += Environment.NewLine + "Inner Exception: " + ex.InnerException.Message;
                    }

                    //errorMessage += Environment.NewLine + "User Name: " + userName;
                    //errorMessage += Environment.NewLine + "Password: " + password;

                    //List<string> sendTo = new List<string>();
                    //sendTo.Add("jbowen@cfsbny.org");

                    //Cfs.Intranet.Utilities.SendNotificationEmail(sendTo, "jbowen@cfsbny.org", "ESS: Signature Validation Failure", errorMessage);

                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Content = new StringContent(errorMessage);

                    throw new HttpResponseException(response);

                }

                response.StatusCode = HttpStatusCode.OK;
                response.Content = new StringContent("true");
                return response;
            }
        }



        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}