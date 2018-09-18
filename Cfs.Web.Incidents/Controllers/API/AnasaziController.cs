using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class AnasaziController : ApiController
    {

        private Models.AnaliveEntities _db = new Models.AnaliveEntities();


        
        public Models.Presentation.AnasaziClientModel Get(int id)
        {

            using (StreamReader stream = new StreamReader(@"\\cfs-anasazi02\Anasazi\AZGlobal\AZGlobal.ini"))
            {
                string line = string.Empty;
                while ((line = stream.ReadLine()) != null)
                {
                    if (line.Split('=')[0] == "AZLIVE")
                    {
                        if (line.Split('=')[1].Trim() != "ON")
                        {
                            throw new Exception("Anasazi is not available for search at the moment.");
                        }
                        else
                        {
                            break;
                        }

                    }  // if (line.Split('=')[0] == "AZLIVE")

                }  // while ((line = stream.ReadLine()) != null)

            }  // using (StreamReader stream...

            var client = (from c in this._db.CDCLIENTs
                          where c.CASE_NUM == id
                          select new Models.Presentation.AnasaziClientModel
                          {
                              caseNumber = c.CASE_NUM,
                              clientName = c.LAST_NAME + ", " + c.FIRST_NAME,
                              birthDate = c.DOB,
                              clientGender = c.SEX
                          }).SingleOrDefault();


            return client;
        }


        [HttpGet, Route("api/anasazi/search")]
        public IQueryable<Models.Presentation.AnasaziClientModel> Search(string query, string subUnit)
        {
            using (StreamReader stream = new StreamReader(@"\\cfs-anasazi02\Anasazi\AZGlobal\AZGlobal.ini"))
            {
                string line = string.Empty;
                while ((line = stream.ReadLine()) != null)
                {
                    if (line.Split('=')[0] == "AZLIVE")
                    {
                        if (line.Split('=')[1].Trim() != "ON")
                        {
                            throw new Exception("Anasazi is not available for search at the moment.");
                        }
                        else
                        {
                            break;
                        }

                    }  // if (line.Split('=')[0] == "AZLIVE")

                }  // while ((line = stream.ReadLine()) != null)

            }  // using (StreamReader stream...

            query = query.ToUpper();

            DateTime nullAzDate = new DateTime(1,1,1);

            var clients = from c in this._db.CDCLIENTs
                          where c.SORT_NAME.Contains(query) &&
                             (from a in this._db.CDASSIGNs
                              where a.SUB_UNIT_ID == subUnit
                                     && a.DATE_CLOSED == nullAzDate
                              select a.CLIENT_ID).Contains(c.ID)
                          select new Models.Presentation.AnasaziClientModel
                          {
                              caseNumber = c.CASE_NUM,
                              clientName = c.LAST_NAME + ", " + c.FIRST_NAME,
                              birthDate = c.DOB,
                              clientGender = c.SEX
                          };


            return clients;
        }




        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}