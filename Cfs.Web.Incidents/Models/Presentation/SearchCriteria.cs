using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cfs.Web.Incidents.Models.Presentation
{
    public class SearchCriteria
    {
        public string clientName { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}