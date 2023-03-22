using System.Web.Mvc;

namespace HVMAPP.Areas.HVM
{
    public class HVMAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "HVM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "HVM_default",
                "HVM/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}