using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cfs.Web.Incidents.Models.Presentation
{
    public class MedAssmntViewModel
    {
        public long incidentMedicalId { get; set; }
        public long incidentId { get; set; }
        public string clientName { get; set; }
        public long medStaffId { get; set; }
        public string staffName { get; set; }
        public DateTime medAssmntDate { get; set; }
        public string medAssmnt { get; set; }
        public DateTime createdStamp { get; set; }
        public string createdStation { get; set; }
        public int statusId { get; set; }
        public string currentStatus { get; set; }
        public bool isOpen { get; set; }
        public DateTime? completedStamp { get; set; }
    }



    
}