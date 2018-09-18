using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cfs.Web.Incidents.Models.Presentation
{
    public class ProgramNotifiersModel
    {
        public long programStaffId { get; set; }
        public int programId { get; set; }
        public string programTitle { get; set; }
        public int notifyPartyId { get; set; }
        public string notifyPartyText { get; set; }
        public long? userId { get; set; }
        public string staffName { get; set; }
        public string emailAddress { get; set; }
        public string smsNumber { get; set; }
        public long incidentId { get; set; }
        public bool isRemovable { get; set; }
    }
}