using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cfs.Web.Incidents.Models.Presentation
{
    public class LogsViewModel
    {
        public long reportLogId { get; set; }
        public long incidentId { get; set; }
        public long userId { get; set; }
        public string staffName { get; set; }
        public DateTime logDateTime { get; set; }
        public string userStation { get; set; }
        public string logDetails { get; set; }
    }
}