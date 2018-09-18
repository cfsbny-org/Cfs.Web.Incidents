using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class CommentsController : ApiController
    {


        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();



        
        public IQueryable<Models.Presentation.AdminCommentsViewModel> Get(long id)
        {
            var comments = from c in this._db.AdminComments
                           join u in this._db.Users
                                on c.adminUserId equals u.userId
                           where c.incidentId == id
                           select new Models.Presentation.AdminCommentsViewModel
                           {
                               adminCommentsId = c.adminCommentId,
                               userId = c.adminUserId,
                               staffName = u.firstName + " " + u.lastName,
                               comments = c.adminCommentText,
                               commentStamp = c.adminCommentStamp
                           };

            return comments;
        }


        public IQueryable<Models.AdminComment> Print(long id)
        {
            return this._db.AdminComments.Where(c => c.incidentId == id);
        }

        

        public void Post([FromBody]Models.AdminComment comment)
        {
            if (comment.adminCommentId == 0)
            {
                this._db.AdminComments.Add(comment);
            }
            else
            {
                this._db.AdminComments.Attach(comment);
                this._db.Entry(comment).State = System.Data.Entity.EntityState.Modified;
            }
            this._db.SaveChanges();
        }

        

        
        public void Delete(long id)
        {
            Models.AdminComment comment = this._db.AdminComments.Where(c => c.adminCommentId == id).SingleOrDefault();

            this._db.AdminComments.Attach(comment);
            this._db.Entry(comment).State = System.Data.Entity.EntityState.Deleted;

            this._db.SaveChanges();
        }



        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}