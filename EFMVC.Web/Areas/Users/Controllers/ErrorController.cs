using EFMVC.Web.Core.ActionFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.Users.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "User")]
    [RouteArea("Users")]
    [RoutePrefix("Error")]
    public class ErrorController : Controller
    {
        // GET: Users/Error

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

        [Route("Http404")]
        public ActionResult Http404()
        {
            return View();
        }
        [Route("InternalServerError")]
        public ActionResult InternalServerError()
        {
            return View();
        }
    }
}