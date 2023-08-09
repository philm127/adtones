using EFMVC.Web.Core.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EFMVC.Web.Controllers
{
    //[AllowAnonymous]
    [CompressResponse]
    [Authorize(Roles = "Advertiser")]
    [RoutePrefix("Error")]
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        protected override void HandleUnknownAction(string actionName)
        {
            if (this.GetType() != typeof(ErrorController))
            {
                
                var errorRoute = new RouteData();
                errorRoute.Values.Add("controller", "Error");
                errorRoute.Values.Add("action", "Http404");
                errorRoute.Values.Add("url", HttpContext.Request.Url.OriginalString);
                if (User.IsInRole("Admin"))
                {
                    RedirectToAction("Http404", "AdminError", new { area = "Admin" }).ExecuteResult(this.ControllerContext);
                   
                }
                else if (User.IsInRole("User"))
                {
                    RedirectToAction("Http404", "Error", new { area = "Users" }).ExecuteResult(this.ControllerContext);
                }
                else
                {
                    View("Http404").ExecuteResult(this.ControllerContext);
                }
                    
            }
            else
            {
                var errorRoute = new RouteData();
                errorRoute.Values.Add("controller", "Error");
                errorRoute.Values.Add("action", "InternalServerError");
                errorRoute.Values.Add("url", HttpContext.Request.Url.OriginalString);
                if (User.IsInRole("Admin"))
                {
                    RedirectToAction("InternalServerError", "AdminError", new { area = "Admin" }).ExecuteResult(this.ControllerContext);
                }
                else if(User.IsInRole("User"))
                {
                    RedirectToAction("InternalServerError", "Error", new { area = "Users" }).ExecuteResult(this.ControllerContext);
                }
                else
                {
                    View("InternalServerError").ExecuteResult(this.ControllerContext);
                }
            }
        }

        public ActionResult Http404()
        {
            if(User.IsInRole("Admin"))
            {
                RedirectToAction("Http404", "AdminError", new { area = "Admin" }).ExecuteResult(this.ControllerContext);
            }
            else if(User.IsInRole("Advertiser"))
            {
                RedirectToAction("Http404Advertiser", "Error").ExecuteResult(this.ControllerContext);
            }
            return View();
        }
        public ActionResult Http404Advertiser()
        {
            return View();
        }
        
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult InternalServerError()
        {
            if (User.IsInRole("Admin"))
            {
                RedirectToAction("InternalServerError", "AdminError", new { area = "Admin" }).ExecuteResult(this.ControllerContext);
            }
            return View();
        }

        [Route("NotFound")]
        public ActionResult NotFound()
        {
            return View();
        }

        [Route("ServerError")]
        public ActionResult ServerError()
        {
            return View();
        }
    }
}
