using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cfs.Web.Incidents.Models.Presentation
{
    public class ReportLocksViewModel
    {
        public long reportLockId { get; set; }
        public string staffName { get; set; }
        public string staffEmail { get; set; }
        public DateTime lockStamp { get; set; }
    }

    public class UnlockInfoModel
    {
        public long reportLockId { get; set; }
        public DateTime lockReleased { get; set; }
    }
}