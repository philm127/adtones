using AutoMapper;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Model;
using EFMVC.Web.Areas.Users.Models;
using EFMVC.Web.Areas.Users.SearchClass;
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

namespace EFMVC.Web.Areas.Users.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "User")]
    [RouteArea("Users")]
    [RoutePrefix("AccountOverview")]
    public class AccountOverviewController : Controller
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

        /// <summary>
        /// The _reward repository
        /// </summary>
        private readonly IRewardRepository _rewardRepository;

        /// <summary>
        /// The _userReward repository
        /// </summary>
        private readonly IUserRewardRepository _userRewardRepository;

        /// <summary>
        /// The _currency repository
        /// </summary>
        private readonly ICurrencyRepository _currencyRepository;

        public AccountOverviewController(ICommandBus commandBus, IProfileRepository profileRepository,
                               IUserRepository userRepository, ICampaignAuditRepository campaignAuditRepository, IRewardRepository rewardRepository,
                               IUserRewardRepository userRewardRepository, ICurrencyRepository currencyRepository)
        {
            _commandBus = commandBus;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
            _campaignAuditRepository = campaignAuditRepository;
            _rewardRepository = rewardRepository;
            _userRewardRepository = userRewardRepository;
            _currencyRepository = currencyRepository;
        }

        EFMVCDataContex db = new EFMVCDataContex();
        [Route("Index")]
        public ActionResult Index()
        {
            var url = Request.UrlReferrer;
            if (url != null)
            {
                var urlValue = url.ToString();
                if (urlValue.Contains("/Users/Login/Register") || urlValue.Contains("/Users/Login/Index"))
                {
                    ViewBag.FromLogin = "True";
                }
                else
                {
                    ViewBag.FromLogin = "False";
                }
            }
            IEnumerable<UserProfileAdvertsReceivedFromModel> models = null;
            AccountOverviewFilter _filterCritearea = new AccountOverviewFilter();
            models = FillAccountOverview(models);
            return View(Tuple.Create(models, _filterCritearea));

        }

        //Comment 07-03-2019
        //private IEnumerable<UserProfileAdvertsReceivedFromModel> FillAccountOverview(IEnumerable<UserProfileAdvertsReceivedFromModel> models)
        //{
        //    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
        //    User user = _userRepository.GetById(efmvcUser.UserId);
        //    if (user.UserProfiles != null)
        //    {
        //        UserProfile userProfile = user.UserProfiles.FirstOrDefault();
        //        if (userProfile != null)
        //        {
        //            double pot = 0.00;
        //            //IOrderedEnumerable<UserProfileAdvertsReceived> userProfileAdvertsReceiveds =
        //            //    userProfile.UserProfileAdvertsReceived.OrderByDescending(x => x.DateTimePlayed);

        //            //models =
        //            //   Mapper.Map<IEnumerable<UserProfileAdvertsReceived>, IEnumerable<UserProfileAdvertsReceivedFromModel>>(
        //            //       userProfileAdvertsReceiveds);
        //            //CaculateMatric(models, userProfile.UserProfileId);

        //            var OperatorID = user.OperatorId;

        //            if (OperatorID == 1)
        //            {
        //                //Add 19-02-2019
        //                //var userRewardInfo = _userRewardRepository.GetMany(top => top.UserId == user.UserId);
        //                //var rewardValue = "";
        //                //if (user.RewardId != null)
        //                //{
        //                //    rewardValue = _rewardRepository.GetById(user.RewardId.Value).RewardValue;
        //                //}

        //                IOrderedEnumerable<UserProfileAdvertsReceived> userProfileAdvertsReceiveds =
        //                    userProfile.UserProfileAdvertsReceived.OrderByDescending(x => x.DateTimePlayed);

        //                models =
        //                   Mapper.Map<IEnumerable<UserProfileAdvertsReceived>, IEnumerable<UserProfileAdvertsReceivedFromModel>>(
        //                       userProfileAdvertsReceiveds);

        //                //Add 20-02-2019

        //                var userRewardInfo = _userRewardRepository.GetMany(top => top.UserId == user.UserId).ToList();
        //                var rewardValue = "";
        //                var CurrencyCode = "";
        //                //double pot = 0.00;

        //                //Add 22-02-2019
        //                var currencyId = _currencyRepository.Get(top => top.CountryId == user.Operator.CountryId);
        //                if (currencyId != null)
        //                {
        //                    if (currencyId.CurrencyId == 13)
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR")).CurrencyCode;
        //                    }
        //                    else
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyId == currencyId.CurrencyId).CurrencyCode;
        //                    }
        //                }
        //                else
        //                {
        //                    CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD")).CurrencyCode;
        //                }

        //                ViewBag.currencyCode = CurrencyCode;

        //                if (userRewardInfo != null)
        //                {
        //                    if (userRewardInfo.Count() == 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            //Un-Commented 21-02-2019
        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);

        //                            foreach (var item in models)
        //                            {
        //                                //Commentde 21-02-2019
        //                                //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                                foreach (var data in userRewardInfo)
        //                                {
        //                                    if (pot > double.Parse(item.CreditsReceived.ToString()))
        //                                    {
        //                                        var rewardInfo = _rewardRepository.GetById(data.RewardId).RewardValue;
        //                                        item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) / double.Parse(rewardInfo));
        //                                        pot = Convert.ToDouble(item.Rewards);
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                    else if (userRewardInfo.Count() > 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);
        //                            //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                            int userRewardInfoCount = 0;
        //                            foreach (var item in models)
        //                            {
        //                                for (int i = 0; i < userRewardInfo.Count(); i++)
        //                                {
        //                                    if (userRewardInfoCount == userRewardInfo.Count())
        //                                    {
        //                                        userRewardInfoCount = 0;
        //                                    }
        //                                    var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                    if (pot > Convert.ToDouble(rewardInfo))
        //                                    {
        //                                        //var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                        //item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) - double.Parse(rewardInfo));
        //                                        item.Rewards = string.Format("{0:#,###0.00}", rewardInfo);
        //                                        pot = double.Parse(pot.ToString()) - double.Parse(rewardInfo);
        //                                        if (userRewardInfo.Count() == i)
        //                                        {
        //                                            userRewardInfoCount = 0;
        //                                        }
        //                                        else
        //                                        {
        //                                            userRewardInfoCount++;
        //                                            break;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                }

        //                //Add 21-02-2019
        //                models = models.OrderByDescending(x => x.CreditsReceived);

        //                //Add 18-02-2019
        //                //foreach (var item in models)
        //                //{
        //                //    if (rewardValue != "")
        //                //    {
        //                //        if (double.Parse(item.CreditsReceived) > double.Parse(rewardValue))
        //                //        {
        //                //            item.Rewards = double.Parse(item.CreditsReceived) - double.Parse(rewardValue);
        //                //        }
        //                //        else
        //                //        {
        //                //            item.Rewards = 0.00;
        //                //        }
        //                //    }
        //                //}
        //            }
        //            else if (OperatorID == 2)
        //            {
        //                //Add 18-02-2019
        //                //var rewardValue = "";
        //                //if (user.RewardId != null)
        //                //{
        //                //    rewardValue = _rewardRepository.GetById(user.RewardId.Value).RewardValue;
        //                //}

        //                // var userProfileAdvertsReceiveds = db.UserProfileAdvertsReceived2.Where(s=> s.UserProfileId == userProfile.UserProfileId).OrderByDescending(x => x.DateTimePlayed);
        //                IOrderedEnumerable<UserProfileAdvertsReceived2> userProfileAdvertsReceiveds =
        //                    userProfile.UserProfileAdvertsReceived2.OrderByDescending(x => x.DateTimePlayed);
        //                models =
        //                   Mapper.Map<IEnumerable<UserProfileAdvertsReceived2>, IEnumerable<UserProfileAdvertsReceivedFromModel>>(
        //                       userProfileAdvertsReceiveds);

        //                //Add 20-02-2019

        //                var userRewardInfo = _userRewardRepository.GetMany(top => top.UserId == user.UserId).ToList();
        //                var rewardValue = "";
        //                var CurrencyCode = "";
        //                //double pot = 0.00;

        //                //Add 22-02-2019
        //                var currencyId = _currencyRepository.Get(top => top.CountryId == user.Operator.CountryId);
        //                if (currencyId != null)
        //                {
        //                    if (currencyId.CurrencyId == 13)
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR")).CurrencyCode;
        //                    }
        //                    else
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyId == currencyId.CurrencyId).CurrencyCode;
        //                    }
        //                }
        //                else
        //                {
        //                    CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD")).CurrencyCode;
        //                }
        //                ViewBag.currencyCode = CurrencyCode;

        //                if (userRewardInfo != null)
        //                {
        //                    if (userRewardInfo.Count() == 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            //Un-Commented 21-02-2019
        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);

        //                            foreach (var item in models)
        //                            {
        //                                //Commentde 21-02-2019
        //                                //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                                foreach (var data in userRewardInfo)
        //                                {
        //                                    if (pot > double.Parse(item.CreditsReceived.ToString()))
        //                                    {
        //                                        var rewardInfo = _rewardRepository.GetById(data.RewardId).RewardValue;
        //                                        item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) / double.Parse(rewardInfo));
        //                                        pot = Convert.ToDouble(item.Rewards);
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                    else if (userRewardInfo.Count() > 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);
        //                            //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                            int userRewardInfoCount = 0;
        //                            foreach (var item in models)
        //                            {
        //                                for (int i = 0; i < userRewardInfo.Count(); i++)
        //                                {
        //                                    if (userRewardInfoCount == userRewardInfo.Count())
        //                                    {
        //                                        userRewardInfoCount = 0;
        //                                    }
        //                                    var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                    if (pot > Convert.ToDouble(rewardInfo))
        //                                    {
        //                                        //var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                        //item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) - double.Parse(rewardInfo));
        //                                        item.Rewards = string.Format("{0:#,###0.00}", rewardInfo);
        //                                        pot = double.Parse(pot.ToString()) - double.Parse(rewardInfo);
        //                                        if (userRewardInfo.Count() == i)
        //                                        {
        //                                            userRewardInfoCount = 0;
        //                                        }
        //                                        else
        //                                        {
        //                                            userRewardInfoCount++;
        //                                            break;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                }

        //                //Add 21-02-2019
        //                models = models.OrderByDescending(x => x.CreditsReceived);
        //            }
        //            else if (OperatorID == 3)
        //            {
        //                //Add 18-02-2019
        //                //var rewardValue = "";
        //                //if (user.RewardId != null)
        //                //{
        //                //    rewardValue = _rewardRepository.GetById(user.RewardId.Value).RewardValue;
        //                //}

        //                IOrderedEnumerable<UserProfileAdvertsReceived3> userProfileAdvertsReceiveds =
        //                     userProfile.UserProfileAdvertsReceived3.OrderByDescending(x => x.DateTimePlayed);
        //                models =
        //                   Mapper.Map<IEnumerable<UserProfileAdvertsReceived3>, IEnumerable<UserProfileAdvertsReceivedFromModel>>(
        //                       userProfileAdvertsReceiveds);

        //                //Add 20-02-2019

        //                var userRewardInfo = _userRewardRepository.GetMany(top => top.UserId == user.UserId).ToList();
        //                var rewardValue = "";
        //                var CurrencyCode = "";
        //                //double pot = 0.00;

        //                //Add 22-02-2019
        //                var currencyId = _currencyRepository.Get(top => top.CountryId == user.Operator.CountryId);
        //                if (currencyId != null)
        //                {
        //                    if (currencyId.CurrencyId == 13)
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR")).CurrencyCode;
        //                    }
        //                    else
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyId == currencyId.CurrencyId).CurrencyCode;
        //                    }
        //                }
        //                else
        //                {
        //                    CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD")).CurrencyCode;
        //                }
        //                ViewBag.currencyCode = CurrencyCode;

        //                if (userRewardInfo != null)
        //                {
        //                    if (userRewardInfo.Count() == 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            //Un-Commented 21-02-2019
        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);

        //                            foreach (var item in models)
        //                            {
        //                                //Commentde 21-02-2019
        //                                //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                                foreach (var data in userRewardInfo)
        //                                {
        //                                    if (pot > double.Parse(item.CreditsReceived.ToString()))
        //                                    {
        //                                        var rewardInfo = _rewardRepository.GetById(data.RewardId).RewardValue;
        //                                        item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) / double.Parse(rewardInfo));
        //                                        pot = Convert.ToDouble(item.Rewards);
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                    else if (userRewardInfo.Count() > 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);
        //                            //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                            int userRewardInfoCount = 0;
        //                            foreach (var item in models)
        //                            {
        //                                for (int i = 0; i < userRewardInfo.Count(); i++)
        //                                {
        //                                    if (userRewardInfoCount == userRewardInfo.Count())
        //                                    {
        //                                        userRewardInfoCount = 0;
        //                                    }
        //                                    var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                    if (pot > Convert.ToDouble(rewardInfo))
        //                                    {
        //                                        //var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                        //item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) - double.Parse(rewardInfo));
        //                                        item.Rewards = string.Format("{0:#,###0.00}", rewardInfo);
        //                                        pot = double.Parse(pot.ToString()) - double.Parse(rewardInfo);
        //                                        if (userRewardInfo.Count() == i)
        //                                        {
        //                                            userRewardInfoCount = 0;
        //                                        }
        //                                        else
        //                                        {
        //                                            userRewardInfoCount++;
        //                                            break;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                }

        //                //Add 21-02-2019
        //                models = models.OrderByDescending(x => x.CreditsReceived);
        //            }
        //            else if (OperatorID == 4)
        //            {
        //                //Add 18-02-2019
        //                //var rewardValue = "";
        //                //if (user.RewardId != null)
        //                //{
        //                //    rewardValue = _rewardRepository.GetById(user.RewardId.Value).RewardValue;
        //                //}

        //                IOrderedEnumerable<UserProfileAdvertsReceived4> userProfileAdvertsReceiveds =
        //                    userProfile.UserProfileAdvertsReceived4.OrderByDescending(x => x.DateTimePlayed);

        //                models =
        //                   Mapper.Map<IEnumerable<UserProfileAdvertsReceived4>, IEnumerable<UserProfileAdvertsReceivedFromModel>>(
        //                       userProfileAdvertsReceiveds);

        //                //Add 20-02-2019

        //                var userRewardInfo = _userRewardRepository.GetMany(top => top.UserId == user.UserId).ToList();
        //                var rewardValue = "";
        //                var CurrencyCode = "";
        //                //double pot = 0.00;

        //                //Add 22-02-2019
        //                var currencyId = _currencyRepository.Get(top => top.CountryId == user.Operator.CountryId);
        //                if (currencyId != null)
        //                {
        //                    if (currencyId.CurrencyId == 13)
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR")).CurrencyCode;
        //                    }
        //                    else
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyId == currencyId.CurrencyId).CurrencyCode;
        //                    }
        //                }
        //                else
        //                {
        //                    CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD")).CurrencyCode;
        //                }
        //                ViewBag.currencyCode = CurrencyCode;

        //                if (userRewardInfo != null)
        //                {
        //                    if (userRewardInfo.Count() == 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            //Un-Commented 21-02-2019
        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);

        //                            foreach (var item in models)
        //                            {
        //                                //Commentde 21-02-2019
        //                                //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                                foreach (var data in userRewardInfo)
        //                                {
        //                                    if (pot > double.Parse(item.CreditsReceived.ToString()))
        //                                    {
        //                                        var rewardInfo = _rewardRepository.GetById(data.RewardId).RewardValue;
        //                                        item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) / double.Parse(rewardInfo));
        //                                        pot = Convert.ToDouble(item.Rewards);
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                    else if (userRewardInfo.Count() > 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);
        //                            //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                            int userRewardInfoCount = 0;
        //                            foreach (var item in models)
        //                            {
        //                                for (int i = 0; i < userRewardInfo.Count(); i++)
        //                                {
        //                                    if (userRewardInfoCount == userRewardInfo.Count())
        //                                    {
        //                                        userRewardInfoCount = 0;
        //                                    }
        //                                    var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                    if (pot > Convert.ToDouble(rewardInfo))
        //                                    {
        //                                        //var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                        //item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) - double.Parse(rewardInfo));
        //                                        item.Rewards = string.Format("{0:#,###0.00}", rewardInfo);
        //                                        pot = double.Parse(pot.ToString()) - double.Parse(rewardInfo);
        //                                        if (userRewardInfo.Count() == i)
        //                                        {
        //                                            userRewardInfoCount = 0;
        //                                        }
        //                                        else
        //                                        {
        //                                            userRewardInfoCount++;
        //                                            break;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                }

        //                //Add 21-02-2019
        //                models = models.OrderByDescending(x => x.CreditsReceived);
        //            }
        //            else if (OperatorID == 5)
        //            {
        //                //Add 18-02-2019
        //                //var rewardValue = "";
        //                //if (user.RewardId != null)
        //                //{
        //                //    rewardValue = _rewardRepository.GetById(user.RewardId.Value).RewardValue;
        //                //}

        //                IOrderedEnumerable<UserProfileAdvertsReceived5> userProfileAdvertsReceiveds =
        //                     userProfile.UserProfileAdvertsReceived5.OrderByDescending(x => x.DateTimePlayed);
        //                models =
        //                   Mapper.Map<IEnumerable<UserProfileAdvertsReceived5>, IEnumerable<UserProfileAdvertsReceivedFromModel>>(
        //                       userProfileAdvertsReceiveds);

        //                //Add 20-02-2019

        //                var userRewardInfo = _userRewardRepository.GetMany(top => top.UserId == user.UserId).ToList();
        //                var rewardValue = "";
        //                var CurrencyCode = "";
        //                //double pot = 0.00;

        //                //Add 22-02-2019
        //                var currencyId = _currencyRepository.Get(top => top.CountryId == user.Operator.CountryId);
        //                if (currencyId != null)
        //                {
        //                    if (currencyId.CurrencyId == 13)
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR")).CurrencyCode;
        //                    }
        //                    else
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyId == currencyId.CurrencyId).CurrencyCode;
        //                    }
        //                }
        //                else
        //                {
        //                    CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD")).CurrencyCode;
        //                }
        //                ViewBag.currencyCode = CurrencyCode;

        //                if (userRewardInfo != null)
        //                {
        //                    if (userRewardInfo.Count() == 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            //Un-Commented 21-02-2019
        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);

        //                            foreach (var item in models)
        //                            {
        //                                //Commentde 21-02-2019
        //                                //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                                foreach (var data in userRewardInfo)
        //                                {
        //                                    if (pot > double.Parse(item.CreditsReceived.ToString()))
        //                                    {
        //                                        var rewardInfo = _rewardRepository.GetById(data.RewardId).RewardValue;
        //                                        item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) / double.Parse(rewardInfo));
        //                                        pot = Convert.ToDouble(item.Rewards);
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                    else if (userRewardInfo.Count() > 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);
        //                            //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                            int userRewardInfoCount = 0;
        //                            foreach (var item in models)
        //                            {
        //                                for (int i = 0; i < userRewardInfo.Count(); i++)
        //                                {
        //                                    if (userRewardInfoCount == userRewardInfo.Count())
        //                                    {
        //                                        userRewardInfoCount = 0;
        //                                    }
        //                                    var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                    if (pot > Convert.ToDouble(rewardInfo))
        //                                    {
        //                                        //var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                        //item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) - double.Parse(rewardInfo));
        //                                        item.Rewards = string.Format("{0:#,###0.00}", rewardInfo);
        //                                        pot = double.Parse(pot.ToString()) - double.Parse(rewardInfo);
        //                                        if (userRewardInfo.Count() == i)
        //                                        {
        //                                            userRewardInfoCount = 0;
        //                                        }
        //                                        else
        //                                        {
        //                                            userRewardInfoCount++;
        //                                            break;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                }

        //                //Add 21-02-2019
        //                models = models.OrderByDescending(x => x.CreditsReceived);
        //            }
        //            else if (OperatorID == 6)
        //            {
        //                //Add 18-02-2019
        //                //var rewardValue = "";
        //                //if (user.RewardId != null)
        //                //{
        //                //    rewardValue = _rewardRepository.GetById(user.RewardId.Value).RewardValue;
        //                //}

        //                IOrderedEnumerable<UserProfileAdvertsReceived6> userProfileAdvertsReceiveds =
        //                    userProfile.UserProfileAdvertsReceived6.OrderByDescending(x => x.DateTimePlayed);
        //                models =
        //                   Mapper.Map<IEnumerable<UserProfileAdvertsReceived6>, IEnumerable<UserProfileAdvertsReceivedFromModel>>(
        //                       userProfileAdvertsReceiveds);

        //                //Add 20-02-2019

        //                var userRewardInfo = _userRewardRepository.GetMany(top => top.UserId == user.UserId).ToList();
        //                var rewardValue = "";
        //                var CurrencyCode = "";
        //                //double pot = 0.00;

        //                //Add 22-02-2019
        //                var currencyId = _currencyRepository.Get(top => top.CountryId == user.Operator.CountryId);
        //                if (currencyId != null)
        //                {
        //                    if (currencyId.CurrencyId == 13)
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR")).CurrencyCode;
        //                    }
        //                    else
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyId == currencyId.CurrencyId).CurrencyCode;
        //                    }
        //                }
        //                else
        //                {
        //                    CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD")).CurrencyCode;
        //                }
        //                ViewBag.currencyCode = CurrencyCode;

        //                if (userRewardInfo != null)
        //                {
        //                    if (userRewardInfo.Count() == 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            //Un-Commented 21-02-2019
        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);

        //                            foreach (var item in models)
        //                            {
        //                                //Commentde 21-02-2019
        //                                //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                                foreach (var data in userRewardInfo)
        //                                {
        //                                    if (pot > double.Parse(item.CreditsReceived.ToString()))
        //                                    {
        //                                        var rewardInfo = _rewardRepository.GetById(data.RewardId).RewardValue;
        //                                        item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) / double.Parse(rewardInfo));
        //                                        pot = Convert.ToDouble(item.Rewards);
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                    else if (userRewardInfo.Count() > 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);
        //                            //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                            int userRewardInfoCount = 0;
        //                            foreach (var item in models)
        //                            {
        //                                for (int i = 0; i < userRewardInfo.Count(); i++)
        //                                {
        //                                    if (userRewardInfoCount == userRewardInfo.Count())
        //                                    {
        //                                        userRewardInfoCount = 0;
        //                                    }
        //                                    var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                    if (pot > Convert.ToDouble(rewardInfo))
        //                                    {
        //                                        //var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                        //item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) - double.Parse(rewardInfo));
        //                                        item.Rewards = string.Format("{0:#,###0.00}", rewardInfo);
        //                                        pot = double.Parse(pot.ToString()) - double.Parse(rewardInfo);
        //                                        if (userRewardInfo.Count() == i)
        //                                        {
        //                                            userRewardInfoCount = 0;
        //                                        }
        //                                        else
        //                                        {
        //                                            userRewardInfoCount++;
        //                                            break;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                }

        //                //Add 21-02-2019
        //                models = models.OrderByDescending(x => x.CreditsReceived);
        //            }
        //            else if (OperatorID == 7)
        //            {
        //                //Add 18-02-2019
        //                //var rewardValue = "";
        //                //if (user.RewardId != null)
        //                //{
        //                //    rewardValue = _rewardRepository.GetById(user.RewardId.Value).RewardValue;
        //                //}

        //                IOrderedEnumerable<UserProfileAdvertsReceived7> userProfileAdvertsReceiveds =
        //                    userProfile.UserProfileAdvertsReceived7.OrderByDescending(x => x.DateTimePlayed);
        //                models =
        //                   Mapper.Map<IEnumerable<UserProfileAdvertsReceived7>, IEnumerable<UserProfileAdvertsReceivedFromModel>>(
        //                       userProfileAdvertsReceiveds);

        //                //Add 20-02-2019

        //                var userRewardInfo = _userRewardRepository.GetMany(top => top.UserId == user.UserId).ToList();
        //                var rewardValue = "";
        //                var CurrencyCode = "";
        //                //double pot = 0.00;

        //                //Add 22-02-2019
        //                var currencyId = _currencyRepository.Get(top => top.CountryId == user.Operator.CountryId);
        //                if (currencyId != null)
        //                {
        //                    if (currencyId.CurrencyId == 13)
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR")).CurrencyCode;
        //                    }
        //                    else
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyId == currencyId.CurrencyId).CurrencyCode;
        //                    }
        //                }
        //                else
        //                {
        //                    CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD")).CurrencyCode;
        //                }
        //                ViewBag.currencyCode = CurrencyCode;

        //                if (userRewardInfo != null)
        //                {
        //                    if (userRewardInfo.Count() == 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            //Un-Commented 21-02-2019
        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);

        //                            foreach (var item in models)
        //                            {
        //                                //Commentde 21-02-2019
        //                                //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                                foreach (var data in userRewardInfo)
        //                                {
        //                                    if (pot > double.Parse(item.CreditsReceived.ToString()))
        //                                    {
        //                                        var rewardInfo = _rewardRepository.GetById(data.RewardId).RewardValue;
        //                                        item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) / double.Parse(rewardInfo));
        //                                        pot = Convert.ToDouble(item.Rewards);
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                    else if (userRewardInfo.Count() > 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);
        //                            //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                            int userRewardInfoCount = 0;
        //                            foreach (var item in models)
        //                            {
        //                                for (int i = 0; i < userRewardInfo.Count(); i++)
        //                                {
        //                                    if (userRewardInfoCount == userRewardInfo.Count())
        //                                    {
        //                                        userRewardInfoCount = 0;
        //                                    }
        //                                    var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                    if (pot > Convert.ToDouble(rewardInfo))
        //                                    {
        //                                        //var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                        //item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) - double.Parse(rewardInfo));
        //                                        item.Rewards = string.Format("{0:#,###0.00}", rewardInfo);
        //                                        pot = double.Parse(pot.ToString()) - double.Parse(rewardInfo);
        //                                        if (userRewardInfo.Count() == i)
        //                                        {
        //                                            userRewardInfoCount = 0;
        //                                        }
        //                                        else
        //                                        {
        //                                            userRewardInfoCount++;
        //                                            break;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                }

        //                //Add 21-02-2019
        //                models = models.OrderByDescending(x => x.CreditsReceived);
        //            }
        //            else if (OperatorID == 8)
        //            {
        //                //Add 18-02-2019
        //                //var rewardValue = "";
        //                //if (user.RewardId != null)
        //                //{
        //                //    rewardValue = _rewardRepository.GetById(user.RewardId.Value).RewardValue;
        //                //}

        //                IOrderedEnumerable<UserProfileAdvertsReceived8> userProfileAdvertsReceiveds =
        //                    userProfile.UserProfileAdvertsReceived8.OrderByDescending(x => x.DateTimePlayed);
        //                models =
        //                   Mapper.Map<IEnumerable<UserProfileAdvertsReceived8>, IEnumerable<UserProfileAdvertsReceivedFromModel>>(
        //                       userProfileAdvertsReceiveds);

        //                //Add 20-02-2019

        //                var userRewardInfo = _userRewardRepository.GetMany(top => top.UserId == user.UserId).ToList();
        //                var rewardValue = "";
        //                var CurrencyCode = "";
        //                //double pot = 0.00;

        //                //Add 22-02-2019
        //                var currencyId = _currencyRepository.Get(top => top.CountryId == user.Operator.CountryId);
        //                if (currencyId != null)
        //                {
        //                    if (currencyId.CurrencyId == 13)
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR")).CurrencyCode;
        //                    }
        //                    else
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyId == currencyId.CurrencyId).CurrencyCode;
        //                    }
        //                }
        //                else
        //                {
        //                    CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD")).CurrencyCode;
        //                }
        //                ViewBag.currencyCode = CurrencyCode;

        //                if (userRewardInfo != null)
        //                {
        //                    if (userRewardInfo.Count() == 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            //Un-Commented 21-02-2019
        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);

        //                            foreach (var item in models)
        //                            {
        //                                //Commentde 21-02-2019
        //                                //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                                foreach (var data in userRewardInfo)
        //                                {
        //                                    if (pot > double.Parse(item.CreditsReceived.ToString()))
        //                                    {
        //                                        var rewardInfo = _rewardRepository.GetById(data.RewardId).RewardValue;
        //                                        item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) / double.Parse(rewardInfo));
        //                                        pot = Convert.ToDouble(item.Rewards);
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                    else if (userRewardInfo.Count() > 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);
        //                            //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                            int userRewardInfoCount = 0;
        //                            foreach (var item in models)
        //                            {
        //                                for (int i = 0; i < userRewardInfo.Count(); i++)
        //                                {
        //                                    if (userRewardInfoCount == userRewardInfo.Count())
        //                                    {
        //                                        userRewardInfoCount = 0;
        //                                    }
        //                                    var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                    if (pot > Convert.ToDouble(rewardInfo))
        //                                    {
        //                                        //var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                        //item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) - double.Parse(rewardInfo));
        //                                        item.Rewards = string.Format("{0:#,###0.00}", rewardInfo);
        //                                        pot = double.Parse(pot.ToString()) - double.Parse(rewardInfo);
        //                                        if (userRewardInfo.Count() == i)
        //                                        {
        //                                            userRewardInfoCount = 0;
        //                                        }
        //                                        else
        //                                        {
        //                                            userRewardInfoCount++;
        //                                            break;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                }

        //                //Add 21-02-2019
        //                models = models.OrderByDescending(x => x.CreditsReceived);
        //            }
        //            else if (OperatorID == 9)
        //            {
        //                //Add 18-02-2019
        //                //var rewardValue = "";
        //                //if (user.RewardId != null)
        //                //{
        //                //    rewardValue = _rewardRepository.GetById(user.RewardId.Value).RewardValue;
        //                //}

        //                IOrderedEnumerable<UserProfileAdvertsReceived9> userProfileAdvertsReceiveds =
        //                    userProfile.UserProfileAdvertsReceived9.OrderByDescending(x => x.DateTimePlayed);
        //                models =
        //                   Mapper.Map<IEnumerable<UserProfileAdvertsReceived9>, IEnumerable<UserProfileAdvertsReceivedFromModel>>(
        //                       userProfileAdvertsReceiveds);

        //                //Add 20-02-2019

        //                var userRewardInfo = _userRewardRepository.GetMany(top => top.UserId == user.UserId).ToList();
        //                var rewardValue = "";
        //                var CurrencyCode = "";
        //                //double pot = 0.00;

        //                //Add 22-02-2019
        //                var currencyId = _currencyRepository.Get(top => top.CountryId == user.Operator.CountryId);
        //                if (currencyId != null)
        //                {
        //                    if (currencyId.CurrencyId == 13)
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR")).CurrencyCode;
        //                    }
        //                    else
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyId == currencyId.CurrencyId).CurrencyCode;
        //                    }
        //                }
        //                else
        //                {
        //                    CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD")).CurrencyCode;
        //                }
        //                ViewBag.currencyCode = CurrencyCode;

        //                if (userRewardInfo != null)
        //                {
        //                    if (userRewardInfo.Count() == 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            //Un-Commented 21-02-2019
        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);

        //                            foreach (var item in models)
        //                            {
        //                                //Commentde 21-02-2019
        //                                //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                                foreach (var data in userRewardInfo)
        //                                {
        //                                    if (pot > double.Parse(item.CreditsReceived.ToString()))
        //                                    {
        //                                        var rewardInfo = _rewardRepository.GetById(data.RewardId).RewardValue;
        //                                        item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) / double.Parse(rewardInfo));
        //                                        pot = Convert.ToDouble(item.Rewards);
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                    else if (userRewardInfo.Count() > 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);
        //                            //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                            int userRewardInfoCount = 0;
        //                            foreach (var item in models)
        //                            {
        //                                for (int i = 0; i < userRewardInfo.Count(); i++)
        //                                {
        //                                    if (userRewardInfoCount == userRewardInfo.Count())
        //                                    {
        //                                        userRewardInfoCount = 0;
        //                                    }
        //                                    var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                    if (pot > Convert.ToDouble(rewardInfo))
        //                                    {
        //                                        //var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                        //item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) - double.Parse(rewardInfo));
        //                                        item.Rewards = string.Format("{0:#,###0.00}", rewardInfo);
        //                                        pot = double.Parse(pot.ToString()) - double.Parse(rewardInfo);
        //                                        if (userRewardInfo.Count() == i)
        //                                        {
        //                                            userRewardInfoCount = 0;
        //                                        }
        //                                        else
        //                                        {
        //                                            userRewardInfoCount++;
        //                                            break;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                }

        //                //Add 21-02-2019
        //                models = models.OrderByDescending(x => x.CreditsReceived);
        //            }
        //            else if (OperatorID == 10)
        //            {
        //                //Add 18-02-2019
        //                //var rewardValue = "";
        //                //if (user.RewardId != null)
        //                //{
        //                //    rewardValue = _rewardRepository.GetById(user.RewardId.Value).RewardValue;
        //                //}

        //                IOrderedEnumerable<UserProfileAdvertsReceived10> userProfileAdvertsReceiveds =
        //                    userProfile.UserProfileAdvertsReceived10.OrderByDescending(x => x.DateTimePlayed);
        //                models =
        //                   Mapper.Map<IEnumerable<UserProfileAdvertsReceived10>, IEnumerable<UserProfileAdvertsReceivedFromModel>>(
        //                       userProfileAdvertsReceiveds);

        //                //Add 20-02-2019

        //                var userRewardInfo = _userRewardRepository.GetMany(top => top.UserId == user.UserId).ToList();
        //                var rewardValue = "";
        //                var CurrencyCode = "";
        //                //double pot = 0.00;

        //                //Add 22-02-2019
        //                var currencyId = _currencyRepository.Get(top => top.CountryId == user.Operator.CountryId);
        //                if (currencyId != null)
        //                {
        //                    if (currencyId.CurrencyId == 13)
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR")).CurrencyCode;
        //                    }
        //                    else
        //                    {
        //                        CurrencyCode = _currencyRepository.Get(top => top.CurrencyId == currencyId.CurrencyId).CurrencyCode;
        //                    }
        //                }
        //                else
        //                {
        //                    CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD")).CurrencyCode;
        //                }
        //                ViewBag.currencyCode = CurrencyCode;

        //                if (userRewardInfo != null)
        //                {
        //                    if (userRewardInfo.Count() == 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            //Un-Commented 21-02-2019
        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);

        //                            foreach (var item in models)
        //                            {
        //                                //Commentde 21-02-2019
        //                                //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                                foreach (var data in userRewardInfo)
        //                                {
        //                                    if (pot > double.Parse(item.CreditsReceived.ToString()))
        //                                    {
        //                                        var rewardInfo = _rewardRepository.GetById(data.RewardId).RewardValue;
        //                                        item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) / double.Parse(rewardInfo));
        //                                        pot = Convert.ToDouble(item.Rewards);
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                    else if (userRewardInfo.Count() > 1)
        //                    {
        //                        if (models.Count() > 0)
        //                        {
        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(top => top.CreditsReceived);

        //                            pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

        //                            //Add 21-02-2019
        //                            models = models.OrderByDescending(x => x.DateTimePlayed);
        //                            //pot = models.Count() + int.Parse(item.CreditsReceived);
        //                            int userRewardInfoCount = 0;
        //                            foreach (var item in models)
        //                            {
        //                                for (int i = 0; i < userRewardInfo.Count(); i++)
        //                                {
        //                                    if (userRewardInfoCount == userRewardInfo.Count())
        //                                    {
        //                                        userRewardInfoCount = 0;
        //                                    }
        //                                    var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                    if (pot > Convert.ToDouble(rewardInfo))
        //                                    {
        //                                        //var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
        //                                        //item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) - double.Parse(rewardInfo));
        //                                        item.Rewards = string.Format("{0:#,###0.00}", rewardInfo);
        //                                        pot = double.Parse(pot.ToString()) - double.Parse(rewardInfo);
        //                                        if (userRewardInfo.Count() == i)
        //                                        {
        //                                            userRewardInfoCount = 0;
        //                                        }
        //                                        else
        //                                        {
        //                                            userRewardInfoCount++;
        //                                            break;
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        item.Rewards = string.Format("{0:#,###0.00}", 0.00);
        //                                    }
        //                                }

        //                            }
        //                        }
        //                    }
        //                }

        //                //Add 21-02-2019
        //                models = models.OrderByDescending(x => x.CreditsReceived);
        //            }

        //            CaculateMatric(models, userProfile.UserProfileId, pot);

        //        }

        //    }

        //    return models;
        //}


        //Add 07-03-2019
        private IEnumerable<UserProfileAdvertsReceivedFromModel> FillAccountOverview(IEnumerable<UserProfileAdvertsReceivedFromModel> models)
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            User user = _userRepository.GetById(efmvcUser.UserId);
            if (user.UserProfiles != null)
            {
                UserProfile userProfile = user.UserProfiles.FirstOrDefault();
                if (userProfile != null)
                {
                    decimal pot = 0.00M;
                    string rewardsRedeemed = "0";

                    var OperatorID = user.OperatorId;

                    IOrderedEnumerable<UserProfileAdvertsReceived> userProfileAdvertsReceiveds =
                        userProfile.UserProfileAdvertsReceived.OrderByDescending(x => x.Id);

                    models =
                       Mapper.Map<IEnumerable<UserProfileAdvertsReceived>, IEnumerable<UserProfileAdvertsReceivedFromModel>>(
                           userProfileAdvertsReceiveds);

                    var userRewardInfo = _userRewardRepository.GetMany(top => top.UserId == user.UserId).ToList();
                    var rewardValue = "";
                    var CurrencyCode = "";

                    //Comment 28-05-2019
                    //var currencyId = _currencyRepository.Get(top => top.CountryId == user.Operator.CountryId);
                    //if (currencyId != null)
                    //{
                    //    if (currencyId.CurrencyId == 13)
                    //    {
                    //        CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("EUR")).CurrencyCode;
                    //    }
                    //    else
                    //    {
                    //        CurrencyCode = _currencyRepository.Get(top => top.CurrencyId == currencyId.CurrencyId).CurrencyCode;
                    //    }
                    //}
                    //else
                    //{
                    //    CurrencyCode = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD")).CurrencyCode;
                    //}

                    //ViewBag.currencyCode = CurrencyCode;

                    if (userRewardInfo.Count() != 0)
                    {
                        //Add 25-06-2019 Change as per calculation given by Evans
                        if (userRewardInfo.Count() == 1)
                        {
                            if (models.Count() > 0)
                            {
                                models = models.OrderByDescending(top => top.DateTimePlayed);

                                pot = decimal.Round(models.Sum(top => Convert.ToDecimal(top.CreditsReceived)), 2); //Ex. pot = 15.30

                                var rewardInfo = _rewardRepository.GetById(userRewardInfo.FirstOrDefault().RewardId.Value).RewardValue; //Ex. rewardInfo = 3.83

                                if (pot > Convert.ToDecimal(rewardInfo.ToString()))
                                {
                                    var rewardsRedeemedCalculate = decimal.Parse(pot.ToString()) / decimal.Parse(rewardInfo);
                                    rewardsRedeemed = Convert.ToString(Math.Floor(rewardsRedeemedCalculate));
                                    //string.Format("{0:#,#}", decimal.Parse(pot.ToString()) / decimal.Parse(rewardInfo)); //Ex. rewardsRedeemed = 3
                                    pot = decimal.Round((decimal.Parse(pot.ToString()) % decimal.Parse(rewardInfo)), 2); //Ex. pot = 3.81
                                }

                                foreach (var item in models)
                                {
                                    pot = pot + Convert.ToDecimal(item.CreditsReceived.ToString()); //Ex. pot = 15.3 + 0.85 = 16.15

                                    if (pot > Convert.ToDecimal(rewardInfo.ToString())) //Ex.16.15 > 3.83 If "Yes"
                                    {
                                        //Comment 07-08-2019
                                        //item.Rewards = string.Format("{0:#,#}", decimal.Parse(pot.ToString()) / decimal.Parse(rewardInfo)); //Ex. item.Rewards = 16.15 / 3.83 = 4.21 = 4

                                        //Add 07-08-2019
                                        item.Rewards = string.Format("{0:#,#}", 1); //Ex. item.Rewards = 1

                                        pot = decimal.Round((decimal.Parse(pot.ToString()) % decimal.Parse(rewardInfo)), 2); //Ex. pot = 0.83
                                    }
                                    else //Ex. If "No"
                                    {
                                        item.Rewards = "0"; //Ex. item.Rewards = 0
                                        pot = pot; //Ex. pot = pot
                                    }
                                }
                            }
                        }
                        //Add 10-07-2019 Change as per calculation given by Evans
                        else if (userRewardInfo.Count() > 1)
                        {
                            if (models.Count() > 0)
                            {
                                models = models.OrderByDescending(top => top.DateTimePlayed);

                                pot = decimal.Round(models.Sum(top => Convert.ToDecimal(top.CreditsReceived)), 2); //Ex. pot = 11.05
                                int userRewardInfoCount = 0;

                                foreach (var item in models)
                                {
                                    for (int i = 0; i < userRewardInfo.Count(); i++)
                                    {
                                        if (userRewardInfoCount == userRewardInfo.Count()) //Ex. 0 == 0 If "Yes"
                                        {
                                            userRewardInfoCount = 0; //Ex.userRewardInfoCount = 0
                                        }

                                        var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId.Value).RewardValue; //Ex. rewardInfo = 3.83, 4.50, etc....
                                        //pot = pot + item.CreditsReceived; //Ex. pot = 11.05 + 0.85 = 11.90

                                        #region Add by me for check "CreditsReceived" is "0" or Not
                                        if (item.CreditsReceived == 0.00M) //Ex. 0 == 0 If "Yes"
                                        {
                                            item.Rewards = "0"; //Ex. item.Rewards = 0
                                            pot = pot; //Ex. pot = pot
                                        }
                                        #endregion
                                        else
                                        {
                                            if (pot > Convert.ToDecimal(rewardInfo)) //Ex. 11.90 > 3.83 If "Yes"
                                            {
                                                item.Rewards = string.Format("{0:#,#}", 1); //Ex. item.Rewards = 1
                                                pot = decimal.Parse(pot.ToString()) - decimal.Parse(rewardInfo); //Ex. pot = 11.90 - 3.83 = 8.07
                                                if (userRewardInfo.Count() == i) //Ex. 0 == 0 If "Yes"
                                                {
                                                    userRewardInfoCount = 0; //Ex.userRewardInfoCount = 0
                                                }
                                                else
                                                {
                                                    userRewardInfoCount++; //Ex. userRewardInfoCount = userRewardInfoCount + 1
                                                    break;
                                                }
                                            }
                                            else //Ex. If "No"
                                            {
                                                item.Rewards = "0"; //Ex. item.Rewards = 0
                                                pot = pot; //Ex. pot = pot
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //Comment 25-06-2019
                        //if (userRewardInfo.Count() == 1)
                        //{
                        //    if (models.Count() > 0)
                        //    {
                        //        models = models.OrderByDescending(top => top.CreditsReceived);

                        //        pot = models.Count() + double.Parse(models.FirstOrDefault().CreditsReceived.ToString());

                        //        models = models.OrderByDescending(x => x.DateTimePlayed);

                        //        foreach (var item in models)
                        //        {
                        //            foreach (var data in userRewardInfo)
                        //            {
                        //                if (pot > double.Parse(item.CreditsReceived.ToString()))
                        //                {
                        //                    var rewardInfo = _rewardRepository.GetById(data.RewardId).RewardValue;
                        //                    item.Rewards = string.Format("{0:#,###0.00}", double.Parse(pot.ToString()) / double.Parse(rewardInfo));
                        //                    pot = Convert.ToDouble(item.Rewards);
                        //                }
                        //                else
                        //                {
                        //                    item.Rewards = string.Format("{0:#,###0.00}", 0.00);
                        //                }
                        //            }

                        //        }
                        //    }
                        //}
                        //else if (userRewardInfo.Count() > 1)
                        //{
                        //    if (models.Count() > 0)
                        //    {
                        //        models = models.OrderByDescending(top => top.CreditsReceived);

                        //        pot = models.Count() + int.Parse(models.FirstOrDefault().CreditsReceived.ToString());

                        //        models = models.OrderByDescending(x => x.DateTimePlayed);
                        //        int userRewardInfoCount = 0;
                        //        foreach (var item in models)
                        //        {
                        //            for (int i = 0; i < userRewardInfo.Count(); i++)
                        //            {
                        //                if (userRewardInfoCount == userRewardInfo.Count())
                        //                {
                        //                    userRewardInfoCount = 0;
                        //                }
                        //                var rewardInfo = _rewardRepository.GetById(userRewardInfo[userRewardInfoCount].RewardId).RewardValue;
                        //                if (pot > Convert.ToDouble(rewardInfo))
                        //                {
                        //                    item.Rewards = string.Format("{0:#,###0.00}", rewardInfo);
                        //                    pot = double.Parse(pot.ToString()) - double.Parse(rewardInfo);
                        //                    if (userRewardInfo.Count() == i)
                        //                    {
                        //                        userRewardInfoCount = 0;
                        //                    }
                        //                    else
                        //                    {
                        //                        userRewardInfoCount++;
                        //                        break;
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    item.Rewards = string.Format("{0:#,###0.00}", 0.00);
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                    }
                    else
                    {
                        pot = decimal.Round(models.Sum(top => Convert.ToDecimal(top.CreditsReceived)), 2); //Ex. pot = 11.05
                    }

                    models = models.OrderByDescending(x => x.CreditsReceived);

                    CaculateMatric(models, userProfile.UserProfileId, rewardsRedeemed, pot);
                }
            }
            return models;
        }

        public static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
        private void CaculateMatric(IEnumerable<UserProfileAdvertsReceivedFromModel> userprofileModel, int userProfileId, string rewardsRedeemed, decimal pot)
        {
            //Add 22-02-2019
            if (userprofileModel.Count() > 0)
            {
                var totalRewards = Convert.ToDouble(rewardsRedeemed) + userprofileModel.Sum(top => Convert.ToDouble(top.Rewards));
                //var totalRewards = userprofileModel.Sum(top => Convert.ToDouble(top.Rewards));
                //ViewBag.TotalRewards = RoundUp(totalRewards, 0);
                ViewBag.TotalRewards = totalRewards;

                //ViewBag.UnusedCredit = RoundUp(pot, 0);
                ViewBag.UnusedCredit = pot;
            }
            else
            {
                ViewBag.TotalRewards = 0;
                ViewBag.UnusedCredit = 0;
            }

            //Commented 01-03-2019
            //  userprofileModel = userprofileModel.Where(top => !string.IsNullOrEmpty(top.Status) && top.Status.ToLower() == "played"  && top.PlayLengthTicks > 6000); //Code commented on 05-06-2019 for testing demo

            //Add 01-03-2019
            //userprofileModel = userprofileModel.ToList();
            if (userprofileModel.Count() > 0)
            {
                //get total credit
                //Comment 11-07-2019
                //var totalitem = userprofileModel.Sum(top => Convert.ToDouble(top.CreditsReceived));
                //ViewBag.TotalCredit = RoundUp(totalitem, 0);

                //Add 11-07-2019
                var totalitem = userprofileModel.Sum(top => Convert.ToDecimal(top.CreditsReceived));
                ViewBag.TotalCredit = decimal.Round(totalitem, 2);

                //get monthly credit
                var monthlyitem = userprofileModel.Where(top => top.DateTimePlayed.Year == DateTime.Now.Year && top.DateTimePlayed.Month == DateTime.Now.Month).Sum(top => Convert.ToDouble(top.CreditsReceived));

                //ViewBag.MonthlyCredit = RoundUp(monthlyitem, 0);
                ViewBag.MonthlyCredit = decimal.Round(Convert.ToDecimal(monthlyitem), 2);

                //get weekly credit
                DateTime beginningOfWeek = DateTime.Now.BeginningOfWeek();
                DateTime endOfLastWeek = beginningOfWeek.AddDays(6);
                var weeklyitem = userprofileModel.Where(top => top.DateTimePlayed.Date >= beginningOfWeek.Date && top.DateTimePlayed.Date <= endOfLastWeek.Date).Sum(top => Convert.ToDouble(top.CreditsReceived));

                //ViewBag.WeeklyCredit = RoundUp(weeklyitem, 0);
                ViewBag.WeeklyCredit = weeklyitem;

                //get today credit
                var totdayitem = userprofileModel.Where(top => top.DateTimePlayed.Date == DateTime.Now.Date).Sum(top => Convert.ToDouble(top.CreditsReceived));

                //ViewBag.TodayCredit = RoundUp(totdayitem, 0);
                ViewBag.TodayCredit = totdayitem;

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
                    accountOverview = accountOverview.Where(top => top.AdvertName.Contains(_filterCritearea.AdvertName)).ToList();

                }
                if (!String.IsNullOrEmpty(_filterCritearea.Brand))
                {
                    accountOverview = accountOverview.Where(top => top.Brand.Contains(_filterCritearea.Brand)).ToList();

                }
                if ((_filterCritearea.FromDateTimePlayed != null && _filterCritearea.FromDateTimePlayed != null))
                {
                    //accountOverview = accountOverview.Where(top => top.DateTimePlayed.Date >= _filterCritearea.FromDateTimePlayed.Value.Date && top.DateTimePlayed.Date <= _filterCritearea.FromDateTimePlayed.Value.Date).ToList();
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