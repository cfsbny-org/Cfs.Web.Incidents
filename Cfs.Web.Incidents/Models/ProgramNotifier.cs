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
    
    public partial class ProgramNotifier
    {
        public long programStaffId { get; set; }
        public int programId { get; set; }
        public int notifyPartyId { get; set; }
        public Nullable<long> userId { get; set; }
        public string distEmail { get; set; }
        public string smsNumber { get; set; }
        public bool isRemovable { get; set; }
    }
}
