using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class StaffController : ApiController
    {


        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();




        public IQueryable<Models.IncidentStaff> Get(long id)
        {
            return this._db.IncidentStaffs.Where(s => s.incidentId == id);
        }



        [Route("api/staff/supervisor/{id}")]
        public Models.User GetStaffSupervisor(long id)
        {
            var supervisor = (from u in this._db.Users
                              join s in this._db.Users
                                   on u.supervisorId equals s.userId
                              where u.userId == id
                              select s).SingleOrDefault();

            return supervisor;
        }





        [HttpGet, Route("api/staff/search/{query}")]
        public IQueryable<Models.Presentation.UserSearchModel> Search(string query)
        {
            var users = from u in this._db.Users
                        where u.lastName.Contains(query) || u.firstName.Contains(query)
                        orderby u.lastName, u.firstName
                        select new Models.Presentation.UserSearchModel
                        {
                            userId = u.userId,
                            staffName = u.firstName + " " + u.lastName,
                            jobTitle = u.jobTitle
                        };


            return users;
        }





        public void Post([FromBody]Models.IncidentStaff staff)
        {
            this._db.IncidentStaffs.Add(staff);
            this._db.SaveChanges();
        }



        public void Delete([FromBody]Models.IncidentStaff staff)
        {
            this._db.IncidentStaffs.Attach(staff);
            this._db.Entry(staff).State = System.Data.Entity.EntityState.Deleted;
            this._db.SaveChanges();
        }

        


        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}