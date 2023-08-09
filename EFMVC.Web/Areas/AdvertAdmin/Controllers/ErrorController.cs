using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.AdvertAdmin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "AdvertAdmin")]
    [RouteArea("AdvertAdmin")]
    [RoutePrefix("Error")]
    public class ErrorController : Controller
    {
        // GET: AdvertAdmin/Error

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