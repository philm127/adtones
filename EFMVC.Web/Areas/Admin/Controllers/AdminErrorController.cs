using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("AdminError")]
    public class AdminErrorController : Controller
    {
        // GET: Admin/AdminError
        
        [Route("Http404")]
        public ActionResult Http404()
        {
            return View();
        }
        [Route("Error")]
        public ActionResult Error()
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