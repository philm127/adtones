using System;
using System.Web;
using System.Web.Mvc;

namespace Aussie.ActionFilter
{
    [AttributeUsage(AttributeTargets.Class |
      AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class AuthorizeFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            if (session != null)
            {
                var activeSession = session["UserId"];
                if (activeSession == null)
                {
                    //Redirect
                    HttpContext context = HttpContext.Current;
                    string OriginalUrl = context.Request.RawUrl;
                    var url = new UrlHelper(filterContext.RequestContext);
                    var loginUrl = url.Content("~/Landing/Index");
                    filterContext.HttpContext.Response.Redirect(String.Format("{0}?ReturnUrl={1}", loginUrl, context.Server.UrlEncode(OriginalUrl)), true);
                }
            }
        }
    }
}