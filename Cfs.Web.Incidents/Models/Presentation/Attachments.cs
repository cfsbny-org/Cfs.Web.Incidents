using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cfs.Web.Incidents.Models.Presentation
{
    public class AttachmentUploadModel
    {
        public long incidentId { get; set; }
        public string attachFileName { get; set; }
        public string attachFileType { get; set; }
        public string attachFileBase64 { get; set; }
        public string attachmentTitle { get; set; }
        public string attachmentComments { get; set; }
        public long userId { get; set; }
    }



    public class AttachedPhotosModel
    {
        public long incidentAttachmentId { get; set; }
        public long incidentId { get; set; }
        public string attachmentUrl { get; set; }
        public string attachmentTitle { get; set; }
        public string attachmentComments { get; set; }
        public string staffName { get; set; }
        public long uploadedBy { get; set; }
        public DateTime uploadedStamp { get; set; }
    }


    public class AttachedDocumentsModel
    {
        public long incidentAttachmentId { get; set; }
        public long incidentId { get; set; }
        public string attachmentUrl { get; set; }
        public string attachmentTitle { get; set; }
        public string attachmentComments { get; set; }
        public string staffName { get; set; }
        public long uploadedBy { get; set; }
        public DateTime uploadedStamp { get; set; }
    }

}