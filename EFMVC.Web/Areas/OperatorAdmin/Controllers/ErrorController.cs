using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.OperatorAdmin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "OperatorAdmin")]
    [RouteArea("OperatorAdmin")]
    [RoutePrefix("Error")]
    public class ErrorController : Controller
    {
        // GET: OperatorAdmin/Error

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