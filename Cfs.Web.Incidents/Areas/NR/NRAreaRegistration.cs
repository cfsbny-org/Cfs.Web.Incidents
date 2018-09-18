using System.Web.Mvc;

namespace Cfs.Web.Incidents.Areas.NR
{
    public class NRAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "NR";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "NR_default",
                "NR/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}