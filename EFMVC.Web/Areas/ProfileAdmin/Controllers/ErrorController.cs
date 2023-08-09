using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.ProfileAdmin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "ProfileAdmin")]
    [RouteArea("ProfileAdmin")]
    [RoutePrefix("Error")]
    public class ErrorController : Controller
    {
        // GET: ProfileAdmin/Error

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