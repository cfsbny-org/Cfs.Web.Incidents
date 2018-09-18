using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cfs.Web.Incidents.Models.Presentation
{
    public class IncidentTypesRportModel
    {
        public long reportIncidentTypeId { get; set; }
        public long incidentId { get; set; }
        public int incidentTypeId { get; set; }
        public string incidentTypeText { get; set; }
        public string incidentInfo { get; set; }
        public bool needsInfo { get; set; }
        public string needsInfoLabel { get; set; }
    }
}