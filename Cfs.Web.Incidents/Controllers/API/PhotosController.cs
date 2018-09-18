using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class PhotosController : ApiController
    {


        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();




        public IQueryable<Models.Presentation.AttachedPhotosModel> Get(long id)
        {
            var photos = from a in this._db.IncidentAttachments
                         join u in this._db.Users
                             on a.uploadedBy equals u.userId
                         where a.incidentId == id && a.attachmentTypeId == 1 && a.isVoided == false
                         select new Models.Presentation.AttachedPhotosModel
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


            return photos;
        }

        
        
        public string Post([FromBody]Models.Presentation.AttachmentUploadModel photoModel)
        {

            const int maxWidth = 800;
            const int maxHeight = 800;

            string virtualPath = "/content/attachments/" + photoModel.incidentId.ToString() + "/";
            string filePath = System.Web.HttpContext.Current.Server.MapPath(virtualPath);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            string photoFileName = Guid.NewGuid().ToString() + ".png";
            string photoFileNamePath = filePath + "/" + photoFileName;

            byte[] imageBytes = Convert.FromBase64String(photoModel.attachFileBase64);

            MemoryStream stream = new MemoryStream(imageBytes, 0, imageBytes.Length);
            stream.Write(imageBytes, 0, imageBytes.Length);


            Image currentImage = Image.FromStream(stream, true);

            

            int scaledWidth = 0;
            int scaledHeight = 0;
            


            scaledHeight = currentImage.Height;
            scaledWidth = currentImage.Width;

            if (scaledWidth > maxWidth || scaledHeight > maxHeight)
            {
                int deltaWidth = scaledWidth - maxWidth;
                int deltaHeight = scaledHeight - maxHeight;

                double scaleFactor = 1;

                if (deltaHeight > deltaWidth)
                {
                    scaleFactor = ((double)maxHeight) / ((double)scaledHeight);
                }
                else
                {
                    scaleFactor = ((double)maxWidth) / ((double)scaledWidth);
                }

                scaledHeight = Convert.ToInt16(scaleFactor * scaledHeight);
                scaledWidth = Convert.ToInt16(scaleFactor * scaledWidth);
                System.Drawing.Size scaledSize = new System.Drawing.Size(scaledWidth, scaledHeight);

                System.Drawing.Bitmap scaledPhoto = new System.Drawing.Bitmap(currentImage, scaledSize);
                //scaledPhoto.Save(filePath + photoModel.photoFileName, codecInfo, encoderParameters);
                scaledPhoto.Save(photoFileNamePath, System.Drawing.Imaging.ImageFormat.Png);
                scaledPhoto.Dispose();
            }
            else
            {
                //currentImage.Save(filePath + photoModel.photoFileName, codecInfo, encoderParameters);
                currentImage.Save(photoFileNamePath, System.Drawing.Imaging.ImageFormat.Png);
            }


            

            currentImage.Dispose();

            stream.Close();
            stream.Dispose();



            Models.IncidentAttachment attachment = new Models.IncidentAttachment();

            attachment.incidentId = photoModel.incidentId;
            attachment.attachmentTypeId = 1;
            attachment.attachmentUrl = virtualPath + photoFileName;
            attachment.attachmentTitle = photoModel.attachmentTitle;
            attachment.attachmentComments = photoModel.attachmentComments;
            attachment.uploadedBy = photoModel.userId;
            attachment.uploadedStamp = DateTime.Now;
            attachment.isVoided = false;

            this._db.IncidentAttachments.Add(attachment);
            this._db.SaveChanges();


            return photoFileNamePath;
        }




        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}