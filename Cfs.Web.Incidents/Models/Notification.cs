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
    
    public partial class Notification
    {
        public long notificationId { get; set; }
        public long incidentId { get; set; }
        public int notifyPartyId { get; set; }
        public System.DateTime notifyDateTime { get; set; }
        public string notifyContact { get; set; }
        public string notifyMethod { get; set; }
        public string notifyComments { get; set; }
        public long notifyStaffId { get; set; }
        public string notifyInfo { get; set; }
        public int isAcknowledged { get; set; }
        public long acknowledgeUserId { get; set; }
        public Nullable<System.DateTime> acknowledgedStamp { get; set; }
        public string acknowledgedStation { get; set; }
    }
}
