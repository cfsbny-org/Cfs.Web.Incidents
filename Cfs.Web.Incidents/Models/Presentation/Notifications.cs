using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cfs.Web.Incidents.Models.Presentation
{
    public class NotificationsViewModel
    {
        public long notificationId { get; set; }
        public long incidentId { get; set; }
        public int notifyPartyId { get; set; }
        public string notifyPartyText { get; set; }
        public DateTime notifyDateTime { get; set; }
        public string notifyContact { get; set; }
        public string notifyMethod { get; set; }
        public string notifyComments { get; set; }
        public long notifyStaffId { get; set; }
        public string notifyStaffName { get; set; }
        public int isAcknowledged { get; set; }
        public long acknowledgeUserId { get; set; }
        public DateTime? acknowledgedStamp { get; set; }
        public string acknowledgedStation { get; set; }
        public string notifyInfo { get; set; }
    }
}