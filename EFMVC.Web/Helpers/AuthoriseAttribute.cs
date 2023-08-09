using System.Web;
using System.Web.Mvc;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;

namespace EFMVC.Web.Helpers
{
    public class AdminRequiredAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);

            if (!isAuthorized)
                return false;

            EFMVCUser user = HttpContext.Current.User.GetEFMVCUser();
            
            return user.RoleName.ToLower() == "admin";
        }
        //protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        //{
        //    filterContext.Result = new RedirectResult("~/Home/UnAuthorized");
        //}
    }
}