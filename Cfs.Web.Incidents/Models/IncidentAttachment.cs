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
    
    public partial class IncidentAttachment
    {
        public long incidentAttachmentId { get; set; }
        public long incidentId { get; set; }
        public int attachmentTypeId { get; set; }
        public string attachmentUrl { get; set; }
        public string attachmentTitle { get; set; }
        public string attachmentComments { get; set; }
        public long uploadedBy { get; set; }
        public System.DateTime uploadedStamp { get; set; }
        public bool isVoided { get; set; }
    }
}
