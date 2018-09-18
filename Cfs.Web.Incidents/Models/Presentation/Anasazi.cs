using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cfs.Web.Incidents.Models.Presentation
{
    public class AnasaziClientModel
    {
        public decimal caseNumber { get; set; }
        public string clientName { get; set; }
        public DateTime birthDate { get; set; }
        public string clientGender { get; set; }
    }
}