using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cfs.Web.Incidents.Models.Presentation
{
    public class ReportSignsViewModel
    {
        public long reportSigId { get; set; }
        public long incidentId { get; set; }
        public string reportSigType { get; set; }
        public long incidentMedicalId { get; set; }
        public long reportSigUserId { get; set; }
        public string staffName { get; set; }
        public string staffTitle { get; set; }
        public int approvalStatusId { get; set; }
        public string approvalComments { get; set; }
        public DateTime reportSigStamp { get; set; }
        public string reportSigStation { get; set; }
    }
}