//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cfs.Web.Incidents.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class IncidentReport
    {
        public long incidentId { get; set; }
        public long userId { get; set; }
        public string staffName { get; set; }
        public string staffTitle { get; set; }
        public string clientName { get; set; }
        public string clientId { get; set; }
        public System.DateTime clientDob { get; set; }
        public System.DateTime incidentDate { get; set; }
        public int programId { get; set; }
        public string reportingAgency { get; set; }
        public System.DateTime createdStamp { get; set; }
        public string createdStation { get; set; }
        public int statusId { get; set; }
        public long currentUser { get; set; }
        public System.DateTime lastModified { get; set; }
        public long lastModifiedBy { get; set; }
        public string clientGender { get; set; }
        public int incidentReportTypeId { get; set; }
        public string incidentLocation { get; set; }
        public bool isApproximate { get; set; }
    }
}