using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFMVC.Web.Core.ActionFilters;

namespace EFMVC.Web.Controllers
{
    [CompressResponse]
    [Authorize]
    public class BasicInfoController : Controller
    {
        //
        // GET: /BasicInfo/

        public ActionResult Index()
        {
            return View();
        }

    }
}
