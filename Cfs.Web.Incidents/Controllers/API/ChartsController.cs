using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cfs.Web.Incidents.Controllers.API
{
    public class ChartsController : ApiController
    {


        public enum ChartTimeSpan
        {
            Month,
            NinetyDays,
            YTD
        }


        private Models.CfsMasterEntities _db = new Models.CfsMasterEntities();





        [Route("api/charts/incidents")]
        public List<Models.Presentation.HighChartsDataModel> GetIncidentCounts()
        {
            List<Models.Presentation.HighChartsDataModel> chartData = new List<Models.Presentation.HighChartsDataModel>();



            DateTime today = DateTime.Today;
           
            DateTime startDate = new DateTime(today.AddMonths(-11).Year, today.AddMonths(-11).Month, 1);
            DateTime endDate = new DateTime(today.AddMonths(1).Year, today.AddMonths(1).Month, 1).AddDays(-1);

            List<object> categoryDataValues = new List<object>();
            List<object> incidentCountsByMonth = new List<object>();

            var incidents = (from i in this._db.IncidentReports
                             where i.incidentDate >= startDate && i.incidentDate <= endDate
                             select i.incidentDate).ToList();



            for (var month = startDate.Date; month <= endDate; month = month.AddMonths(1))
            {
                categoryDataValues.Add(string.Format("{0} {1}", month.ToString("MMM"), month.Year.ToString()));

                var incidentsInMonth = incidents.Where(i => i.Month == month.Month && i.Year == month.Year).Count();
                incidentCountsByMonth.Add(incidentsInMonth);
            }


            chartData.Add(new Models.Presentation.HighChartsDataModel()
            {
                name = "Month",
                data = categoryDataValues
            });

            chartData.Add(new Models.Presentation.HighChartsDataModel()
            {
                name = "Incidents",
                data = incidentCountsByMonth
            });



            return chartData;
        }


        [Route("api/charts/restraints")]
        public List<Models.Presentation.HighChartsDataModel> GetRestrainsCounts()
        {
            List<Models.Presentation.HighChartsDataModel> chartData = new List<Models.Presentation.HighChartsDataModel>();



            DateTime today = DateTime.Today;

            DateTime startDate = new DateTime(today.AddMonths(-11).Year, today.AddMonths(-11).Month, 1);
            DateTime endDate = new DateTime(today.AddMonths(1).Year, today.AddMonths(1).Month, 1).AddDays(-1);

            List<object> categoryDataValues = new List<object>();
            List<object> incidentCountsByMonth = new List<object>();

            var incidents = (from i in this._db.IncidentReports
                             join r in this._db.Restraints
                                    on i.incidentId equals r.incidentId
                             where i.incidentDate >= startDate && i.incidentDate <= endDate
                             select i.incidentDate).ToList();



            for (var month = startDate.Date; month <= endDate; month = month.AddMonths(1))
            {
                categoryDataValues.Add(string.Format("{0} {1}", month.ToString("MMM"), month.Year.ToString()));

                var incidentsInMonth = incidents.Where(i => i.Month == month.Month && i.Year == month.Year).Count();
                incidentCountsByMonth.Add(incidentsInMonth);
            }


            chartData.Add(new Models.Presentation.HighChartsDataModel()
            {
                name = "Month",
                data = categoryDataValues
            });

            chartData.Add(new Models.Presentation.HighChartsDataModel()
            {
                name = "Restraints",
                data = incidentCountsByMonth
            });



            return chartData;
        }



        [Route("api/charts/justice/{timeSpan}")]
        public List<Models.Presentation.HighChartsDataModel> GetJusticeCenterCalls(ChartTimeSpan timeSpan)
        {
            List<Models.Presentation.HighChartsDataModel> chartData = new List<Models.Presentation.HighChartsDataModel>();

            switch (timeSpan)
            {
                case ChartTimeSpan.Month:


                    DateTime today = DateTime.Today;
                    DateTime monthStart = new DateTime(today.Year, today.Month, 1);
                    DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);


                    List<object> categoryDataValues = new List<object>();
                    List<object> incidentCountsByDay = new List<object>();

                    var calls = (from i in this._db.IncidentReports
                                 join n in this._db.Notifications
                                        on i.incidentId equals n.incidentId
                                 where (i.incidentDate >= monthStart && i.incidentDate <= monthEnd)
                                         && n.notifyPartyId == 8
                                         && i.statusId != 7 // NOT VOIDED
                                 select n.notifyDateTime).ToList();



                    for (var day = monthStart.Date; day <= monthEnd; day = day.AddDays(1))
                    {
                        categoryDataValues.Add(day.Day);

                        var incidentsOnDate = calls.Where(i => i.Date == day.Date).Count();
                        incidentCountsByDay.Add(incidentsOnDate);
                    }


                    chartData.Add(new Models.Presentation.HighChartsDataModel()
                    {
                        name = "Day of Month",
                        data = categoryDataValues
                    });

                    chartData.Add(new Models.Presentation.HighChartsDataModel()
                    {
                        name = "Justice Center Calls",
                        data = incidentCountsByDay
                    });

                    break;
                case ChartTimeSpan.NinetyDays:
                    chartData.Add(new Models.Presentation.HighChartsDataModel()
                    {
                        name = "Last Ninety Days"
                    });
                    break;
                case ChartTimeSpan.YTD:
                    chartData.Add(new Models.Presentation.HighChartsDataModel()
                    {
                        name = "Year-to-Date"
                    });
                    break;
            }

            return chartData;
        }




        [Route("api/charts/programs")]
        public List<Models.Presentation.HighChartsPieModel> GetIncidentPrograms()
        {
            List<Models.Presentation.HighChartsPieModel> chartData = new List<Models.Presentation.HighChartsPieModel>();


            var programs = from i in this._db.IncidentReports
                             join p in this._db.IncidentPrograms
                                    on i.programId equals p.incidentProgramId
                              where i.statusId != 7 // NOT VOIDED
                              group p by new { p.programTitle } into g
                              select new
                              {
                                  programTitle = g.Key.programTitle,
                                  incidentCounts = g.Count()
                              };




            foreach (var program in programs)
            {
                chartData.Add(new Models.Presentation.HighChartsPieModel()
                {
                    dataName = program.programTitle,
                    dataValue = program.incidentCounts
                });
            }

            return chartData.OrderByDescending(d => d.dataValue).ToList();
        }


        [Route("api/charts/incidentTypes")]
        public List<Models.Presentation.HighChartsPieModel> GetIncidentTypes()
        {
            List<Models.Presentation.HighChartsPieModel> chartData = new List<Models.Presentation.HighChartsPieModel>();
            

            var typesCounts = from i in this._db.IncidentReports
                              join r in this._db.ReportIncidents
                                    on i.incidentId equals r.incidentId
                              join t in this._db.IncidentTypes
                                      on r.incidentTypeId equals t.incidentTypeId
                              where i.statusId != 7 // NOT VOIDED
                              group r by new { t.incidentTypeText } into g
                              select new
                              {
                                  incidentType = g.Key.incidentTypeText,
                                  incidentCounts = g.Count()
                              };




            foreach (var incidentType in typesCounts)
            {
                chartData.Add(new Models.Presentation.HighChartsPieModel()
                {
                    dataName = incidentType.incidentType,
                    dataValue = incidentType.incidentCounts
                });
            }

            return chartData.OrderByDescending(d=>d.dataValue).ToList();
        }



        protected override void Dispose(bool disposing)
        {
            this._db.Dispose();
            base.Dispose(disposing);
        }
    }
}