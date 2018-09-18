using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cfs.Web.Incidents.Models.Presentation
{
    public class ReportsViewModel
    {
        public long incidentId { get; set; }

        public long userId { get; set; }
        public int incidentReportTypeId { get; set; }
        public string clientName { get; set; }
        public string clientId { get; set; }
        public DateTime clientDob { get; set; }
        public string clientGender { get; set; }
        public string staffName { get; set; }
        public string staffTitle { get; set; }
        public string programTitle { get; set; }
        public string reportingAgency { get; set; }
        public string incidentLocation { get; set; }
        public DateTime incidentDate { get; set; }
        public bool isApproximate { get; set; }
        public string createdByName { get; set; }
        public DateTime createdStamp { get; set; }
        public DateTime lastModified { get; set; }
        public string lastModifiedByName { get; set; }
        public int statusId { get; set; }
        public string currentStatus { get; set; }
        public bool medicalCompleted { get; set; }
        public IQueryable<Models.Presentation.MedAssmntViewModel> medicals { get; set; }
    }



    public class ReportStatusChangeModel
    {
        public long incidentId { get; set; }
        public int statusId { get; set; }
        public long currentUser { get; set; }
        public DateTime lastModified { get; set; }
        public long lastModifiedBy { get; set; }
        public string signature { get; set; }
    }



    public class ReportSignatureModel
    {
        public long incidentId { get; set; }
        public long incidentMedicalId { get; set; }
        public int statusId { get; set; }
        public long currentUser { get; set; }
        public string userName { get; set; }
        public string signature { get; set; }
        public string staffName { get; set; }
        public string staffTitle { get; set; }
        public string staffEmail { get; set; }
        public string stationName { get; set; }
        public int approvalStatusId { get; set; }
        public string approvalComments { get; set; }
        public string signatureType { get; set; }
        public string signStatusText { get; set; }
        public DateTime? reportSigStamp { get; set; }
        public string ebookFolder { get; set; }
    }


    

    
}