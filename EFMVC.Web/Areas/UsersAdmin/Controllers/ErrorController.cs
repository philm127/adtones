using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.UsersAdmin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "UserAdmin")]
    [RouteArea("UsersAdmin")]
    [RoutePrefix("Error")]
    public class ErrorController : Controller
    {
        // GET: UsersAdmin/Error

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