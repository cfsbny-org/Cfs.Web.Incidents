using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cfs.Web.Incidents.Models.Presentation
{
    public class UserSessionModel
    {
        public long userId { get; set; }
        public string userName { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string fullName { get; set; }
        public string jobTitle { get; set; }
        public string userEmail { get; set; }
        public long supervisorId { get; set; }
        public string supervisorName { get; set; }
        public string supervisorEmail { get; set; }
        public string stationInfo { get; set; }
    }


    public class UserSearchModel
    {
        public long userId { get; set; }
        public string staffName { get; set; }
        public string jobTitle { get; set; }
    }
}