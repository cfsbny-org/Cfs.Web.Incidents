using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class UsersController : ApiController
    {


        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();



        public IQueryable<Models.User> Get()
        {
            return this._db.Users;
        }


        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}