using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("Error")]
    public class ErrorController : Controller
    {
        // GET: Admin/Error

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