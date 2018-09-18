using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cfs.Web.Incidents.Models.Presentation
{
    public class AgendaPostModel
    {
        public string agendaTitle { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
    }
}