// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="HomeController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Model;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;

/// <summary>
/// The Controllers namespace.
/// </summary>

namespace EFMVC.Web.Controllers
{
    /// <summary>
    /// Class HomeController.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// The _advert repository
        /// </summary>
        private readonly IAdvertRepository _advertRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly ICampaignProfileRepository _profileRepository;

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="profileRepository">The profile repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="advertRepository">The advert repository.</param>
        public HomeController(ICommandBus commandBus, ICampaignProfileRepository profileRepository,
                              IUserRepository userRepository, IAdvertRepository advertRepository)
        {
            _commandBus = commandBus;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _advertRepository = advertRepository;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            if (efmvcUser != null)
            {
                User user = _userRepository.Get(x => x.UserId == efmvcUser.UserId);

                if (user != null)
                {
                    IList<CampaignProfile> activeProfiles = new List<CampaignProfile>();

                    if (user.RoleId == 3)
                    {
                        ICollection<CampaignProfile> profiles = user.CampaignProfiles;

                        foreach (CampaignProfile profile in profiles.Where(profile => profile.Active))
                            activeProfiles.Add(profile);

                        ViewData.Add("ActiveCampaignProfiles", activeProfiles);
                    }
                }
            }

            ViewBag.Message = "";
            return View();
        }

        /// <summary>
        /// Abouts this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult About()
        {
            return View();
        }
    }
}