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
    
    public partial class IncidentStaff
    {
        public long incidentStaffId { get; set; }
        public long incidentId { get; set; }
        public long userId { get; set; }
        public string staffTitle { get; set; }
        public string staffName { get; set; }
    }
}