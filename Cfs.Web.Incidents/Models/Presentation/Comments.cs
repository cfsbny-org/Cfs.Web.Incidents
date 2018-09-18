using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cfs.Web.Incidents.Models.Presentation
{
    public class AdminCommentsViewModel
    {
        public long adminCommentsId { get; set; }
        public long userId { get; set; }
        public string staffName { get; set; }
        public string comments { get; set; }
        public DateTime commentStamp { get; set; }
    }
}