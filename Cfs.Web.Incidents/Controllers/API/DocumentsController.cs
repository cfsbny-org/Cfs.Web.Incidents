using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class DocumentsController : ApiController
    {


        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();



        public IQueryable<Models.Presentation.AttachedDocumentsModel> Get(long id)
        {
            var documents = from a in this._db.IncidentAttachments
                         join u in this._db.Users
                             on a.uploadedBy equals u.userId
                         where a.incidentId == id && a.attachmentTypeId == 2 && a.isVoided == false
                         select new Models.Presentation.AttachedDocumentsModel
                         {
                             incidentAttachmentId = a.incidentAttachmentId,
                             incidentId = a.incidentId,
                             attachmentUrl = a.attachmentUrl,
                             attachmentTitle = a.attachmentTitle,
                             attachmentComments = a.attachmentComments,
                             staffName = u.lastName + " " + u.firstName,
                             uploadedBy = a.uploadedBy,
                             uploadedStamp = a.uploadedStamp
                         };


            return documents;
        }




        public void Post([FromBody]Models.Presentation.AttachmentUploadModel documentModel)
        {

            string virtualPath = "/content/attachments/" + documentModel.incidentId.ToString() + "/";
            string filePath = System.Web.HttpContext.Current.Server.MapPath(virtualPath);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string mimeType = documentModel.attachFileType.Split(';')[0].Substring(5);
            string fileExt = string.Empty;
            switch (mimeType)
            {
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    fileExt = ".docx";
                    break;
                case "application/msword":
                    fileExt = ".doc";
                    break;
                case "application/pdf":
                    fileExt = ".pdf";
                    break;
                default:
                    fileExt = ".txt";
                    break;
            }

            string docFileName = Guid.NewGuid().ToString() + fileExt;
            string docFileNamePath = filePath + "/" + docFileName;


            System.IO.File.WriteAllBytes(docFileNamePath, Convert.FromBase64String(documentModel.attachFileBase64));


            Models.IncidentAttachment attachment = new Models.IncidentAttachment();

            attachment.incidentId =documentModel.incidentId;
            attachment.attachmentTypeId = 2;
            attachment.attachmentUrl = virtualPath + docFileName;
            attachment.attachmentTitle =documentModel.attachmentTitle;
            attachment.attachmentComments =documentModel.attachmentComments;
            attachment.uploadedBy =documentModel.userId;
            attachment.uploadedStamp = DateTime.Now;
            attachment.isVoided = false;

            this._db.IncidentAttachments.Add(attachment);
            this._db.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}