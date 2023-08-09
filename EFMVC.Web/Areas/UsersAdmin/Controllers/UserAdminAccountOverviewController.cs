using AutoMapper;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Model;
using EFMVC.Web.Areas.UsersAdmin.SearchClass;
using EFMVC.Web.Common;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.UsersAdmin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "UserAdmin")]
    [RouteArea("UsersAdmin")]
    [RoutePrefix("UserAdminAccountOverview")]
    public class UserAdminAccountOverviewController : Controller
    {
        // GET: Users/AccountOverview

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly IProfileRepository _profileRepository;

        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly ICampaignAuditRepository _campaignAuditRepository;

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        public UserAdminAccountOverviewController(ICommandBus commandBus, IProfileRepository profileRepository,
                               IUserRepository userRepository, ICampaignAuditRepository campaignAuditRepository)
        {
            _commandBus = commandBus;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _campaignAuditRepository = campaignAuditRepository;
        }
        [Route("Index")]
        public ActionResult Index()
        {
            if (Session["userId"] != null)
            {
                IEnumerable<UserProfileAdvertsReceivedFromModel> models = null;
                AccountOverviewFilter _filterCritearea = new AccountOverviewFilter();
                models = FillAccountOverview(models);
                return View(Tuple.Create(models, _filterCritearea));
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "UserAdmin" });
            }
        }
        private IEnumerable<UserProfileAdvertsReceivedFromModel> FillAccountOverview(IEnumerable<UserProfileAdvertsReceivedFromModel> models)
        {
            var userId = Convert.ToInt32(Session["userId"]);
            User user = _userRepository.GetById(userId);
            if (user.UserProfiles != null)
            {
                UserProfile userProfile = user.UserProfiles.FirstOrDefault();
                if (userProfile != null)
                {
                    IOrderedEnumerable<UserProfileAdvertsReceived> userProfileAdvertsReceiveds =
                        userProfile.UserProfileAdvertsReceived.OrderByDescending(x => x.DateTimePlayed);

                    models =
                       Mapper.Map<IEnumerable<UserProfileAdvertsReceived>, IEnumerable<UserProfileAdvertsReceivedFromModel>>(
                           userProfileAdvertsReceiveds);
                    CaculateMatric(models, userProfile.UserProfileId);
                }

            }

            return models;
        }
        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
        private void CaculateMatric(IEnumerable<UserProfileAdvertsReceivedFromModel> userprofileModel, int userProfileId)
        {
            userprofileModel = userprofileModel.Where(top => top.PlayLengthTicks > 6000);
            //  userprofileModel = userprofileModel.Where(top => top.Status.ToLower() == "played" && top.PlayLengthTicks > 6000); // Second
            // userprofileModel = userprofileModel.Where(top => top.CampaignAudit.Status.ToLower() == "played" && top.CampaignAudit.PlayLengthTicks > 6000); //First
            if (userprofileModel.Count() > 0)
            {
                //get total credit
                var totalitem = userprofileModel.Sum(top => Convert.ToDouble(top.CreditsReceived));

                ViewBag.TotalCredit = RoundUp(totalitem, 0);
                //get monthly credit
                var monthlyitem = userprofileModel.Where(top => top.DateTimePlayed.Year == DateTime.Now.Year && top.DateTimePlayed.Month == DateTime.Now.Month).Sum(top => Convert.ToDouble(top.CreditsReceived));

                ViewBag.MonthlyCredit = RoundUp(monthlyitem, 0);

                //get weekly credit
                DateTime beginningOfWeek = DateTime.Now.BeginningOfWeek();
                DateTime endOfLastWeek = beginningOfWeek.AddDays(6);
                var weeklyitem = userprofileModel.Where(top => top.DateTimePlayed.Date >= beginningOfWeek.Date && top.DateTimePlayed.Date <= endOfLastWeek.Date).Sum(top => Convert.ToDouble(top.CreditsReceived));

                ViewBag.WeeklyCredit = RoundUp(weeklyitem, 0);

                //get today credit
                var totdayitem = userprofileModel.Where(top => top.DateTimePlayed.Date == DateTime.Now.Date).Sum(top => Convert.ToDouble(top.CreditsReceived));

                ViewBag.TodayCredit = RoundUp(totdayitem, 0);

            }
            else
            {
                ViewBag.TotalCredit = 0;
                ViewBag.MonthlyCredit = 0;
                ViewBag.WeeklyCredit = 0;
                ViewBag.TodayCredit = 0;
            }
        }

        [Route("SearchAccountOverview")]
        public ActionResult SearchAccountOverview([Bind(Prefix = "Item2")]SearchClass.AccountOverviewFilter _filterCritearea)
        {
            if (User.Identity.IsAuthenticated)
            {
                IEnumerable<UserProfileAdvertsReceivedFromModel> models = null;


                if (_filterCritearea != null)
                {
                    models = FillAccountOverview(models);
                    IEnumerable<UserProfileAdvertsReceivedFromModel> accountOverview = getaccountOverviewResult(models, _filterCritearea);
                    return PartialView("_AccountOverviewDetails", accountOverview);
                }
                else
                {
                    models = FillAccountOverview(models);
                    IEnumerable<UserProfileAdvertsReceivedFromModel> accountOverview = getaccountOverviewResult(models, _filterCritearea);
                    return PartialView("_AccountOverviewDetails", accountOverview);
                }


            }
            else
            {
                return PartialView("_AccountOverviewDetails", "notauthorise");
            }
        }
        public IEnumerable<UserProfileAdvertsReceivedFromModel> getaccountOverviewResult(IEnumerable<UserProfileAdvertsReceivedFromModel> accountOverview, SearchClass.AccountOverviewFilter _filterCritearea)
        {
            if (accountOverview != null && _filterCritearea != null)
            {
                if (!String.IsNullOrEmpty(_filterCritearea.AdvertRef))
                {
                    accountOverview = accountOverview.Where(top => top.AdvertRef.Contains(_filterCritearea.AdvertRef)).ToList();

                }
                if (!String.IsNullOrEmpty(_filterCritearea.AdvertName))
                {
                    accountOverview = accountOverview.Where(top => top.AdvertName.ToLower().Contains(_filterCritearea.AdvertName.ToLower())).ToList();

                }
                if (!String.IsNullOrEmpty(_filterCritearea.Brand))
                {
                    accountOverview = accountOverview.Where(top => top.Brand.ToLower().Contains(_filterCritearea.Brand.ToLower())).ToList();

                }
                if ((_filterCritearea.FromDateTimePlayed != null && _filterCritearea.ToDateTimePlayed != null))
                {
                    //Comment 04-06-2019
                    //accountOverview = accountOverview.Where(top => top.DateTimePlayed.Date >= _filterCritearea.FromDateTimePlayed.Value.Date && top.DateTimePlayed.Date <= _filterCritearea.FromDateTimePlayed.Value.Date).ToList();

                    //Add 04-06-2019
                    string strTodate = _filterCritearea.ToDateTimePlayed;
                    DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strFromdate = _filterCritearea.FromDateTimePlayed;
                    DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    accountOverview = accountOverview.Where(top => top.DateTimePlayed.Date >= Fromdate && top.DateTimePlayed.Date <= Todate).ToList();
                }
                if ((!String.IsNullOrEmpty(_filterCritearea.FromCreditReceived) && !String.IsNullOrEmpty(_filterCritearea.ToCreditReceived)))
                {
                    accountOverview = accountOverview.Where(top => Convert.ToDecimal(top.CreditsReceived) >= Convert.ToDecimal(_filterCritearea.FromCreditReceived) && Convert.ToDecimal(top.CreditsReceived) <= Convert.ToDecimal(_filterCritearea.ToCreditReceived)).ToList();
                }
            }

            return accountOverview;
        }
    }
}