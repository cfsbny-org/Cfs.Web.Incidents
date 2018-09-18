using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cfs.Web.Incidents.Models.Presentation
{
    public class HighChartsDataModel
    {
        public string name { get; set; }
        public List<object> data { get; set; }
    }

    public class HighChartsPieModel
    {
        public string dataName { get; set; }
        public object dataValue { get; set; }
    }
}