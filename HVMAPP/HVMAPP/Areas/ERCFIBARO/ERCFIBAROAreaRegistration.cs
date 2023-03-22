using System.Web.Mvc;

namespace HVMAPP.Areas.ERCFIBARO
{
    public class ERCFIBAROAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ERCFIBARO";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ERCFIBARO_default",
                "ERCFIBARO/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}