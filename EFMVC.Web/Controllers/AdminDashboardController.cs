using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;

namespace EFMVC.Web.Controllers
{
    [CompressResponse]
    [Authorize(Roles ="Admin")]
    [AdminRequired]
    public class AdminDashboardController : Controller
    {
        //
        // GET: /AdminDashbaord/

        public ActionResult Index()
        {
            return View();
        }
      
        
    }
}
