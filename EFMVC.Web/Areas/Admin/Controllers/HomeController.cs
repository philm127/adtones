using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Authentication;
using EFMVC.Web.Helpers;
using EFMVC.Web.Common;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Core.Extensions;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("Home")]
    public class HomeController : Controller
    { 
        /// <summary>
      /// The _command bus
      /// </summary>
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The form authentication
        /// </summary>
        private readonly IFormsAuthentication formAuthentication;
        public HomeController(ICommandBus commandBus, IUserRepository userRepository, IFormsAuthentication formAuthentication)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            this.formAuthentication = formAuthentication;
        }
        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Route("LogOff")]
        public ActionResult LogOff()
        {
            formAuthentication.Signout();
            return RedirectToAction("Index", "Login", new {area="Admin" });
        }

       

        [Route("Keepalive")]
        public ActionResult Keepalive()
        {
            UpdateSessionTimeOut updateSessionTimeOut = new UpdateSessionTimeOut(formAuthentication, _userRepository);
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            updateSessionTimeOut.SessionTimeout(efmvcUser);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}
