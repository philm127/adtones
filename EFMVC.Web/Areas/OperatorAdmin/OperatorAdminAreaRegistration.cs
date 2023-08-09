using System.Web.Mvc;

namespace EFMVC.Web.Areas.OperatorAdmin
{
    public class OperatorAdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "OperatorAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "OperatorAdmin_default",
                "OperatorAdmin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}