using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using EFMVC.ProvisioningModel;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.Users.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "User")]
    [RouteArea("Users")]
    [RoutePrefix("TimeSettings")]
    public class TimeSettingsController : Controller
    {
        // GET: Users/TimeSettings

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly IProfileRepository _profileRepository;

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;
        public TimeSettingsController(ICommandBus commandBus, IProfileRepository profileRepository,
                               IUserRepository userRepository)
        {
            _commandBus = commandBus;
            _profileRepository = profileRepository;
            _userRepository = userRepository;
        }
        [Route("Index")]
        public ActionResult Index()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

            User user = _userRepository.GetById(efmvcUser.UserId);

            IEnumerable<UserProfile> userProfiles = _profileRepository.GetMany(x => x.UserId == efmvcUser.UserId);
            UserFormModel userFormModel = Mapper.Map<User, UserFormModel>(user);
            UserProfileTimeSettingFormModel userProfileTimeSetting = new ViewModels.UserProfileTimeSettingFormModel();
            if (userFormModel.UserProfile != null)
            {
                ViewBag.UserProfileId = userFormModel.UserProfile.UserProfileId;
                userProfileTimeSetting = UserProfileTimeSettingFormModel(userFormModel.UserProfile.UserProfileId);
            }
            else
            {
                ViewBag.UserProfileId = 0;
            }

            return View(userProfileTimeSetting);
        }
        [Route("SaveTimeSettings")]
        [HttpPost]
        public ActionResult SaveTimeSettings(UserProfileTimeSettingFormModel model)
        {
            if (ModelState.IsValid)
            {
                CreateOrUpdateUserProfileTimeSettingCommand command =
                    Mapper.Map<UserProfileTimeSettingFormModel, CreateOrUpdateUserProfileTimeSettingCommand>(model);

              

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    //using (var mySQLEntities = new arthar_addcache_provisioningEntities4())
                    //{

                    //    var GetUserMatchProfileID = mySQLEntities.usermatches.Where(s => s.MSUserProfileId == model.UserProfileId).FirstOrDefault().UserProfileId;
                    //    var GetUserProfileTimeByID = mySQLEntities.userprofiletimesettings.Where(s => s.UserProfileId == model.UserProfileId).FirstOrDefault();
                    //    if (GetUserProfileTimeByID != null)
                    //    {
                    //        var GetUserProfileForTimeSettings = mySQLEntities.userprofiletimesettings.Where(s => s.UserProfileTimeSettingsId == GetUserProfileTimeByID.UserProfileTimeSettingsId).FirstOrDefault();
                    //        GetUserProfileForTimeSettings.Monday = model.MondayPostedTimes.DayIds != null ? string.Join(",", model.MondayPostedTimes.DayIds) : null;
                    //        GetUserProfileForTimeSettings.Tuesday = model.TuesdayPostedTimes.DayIds != null ? string.Join(",", model.TuesdayPostedTimes.DayIds) : null;
                    //        GetUserProfileForTimeSettings.Wednesday = model.WednesdayPostedTimes.DayIds != null ? string.Join(",", model.WednesdayPostedTimes.DayIds) : null;
                    //        GetUserProfileForTimeSettings.Thursday = model.ThursdayPostedTimes.DayIds != null ? string.Join(",", model.ThursdayPostedTimes.DayIds) : null;
                    //        GetUserProfileForTimeSettings.Friday = model.FridayPostedTimes.DayIds != null ? string.Join(",", model.FridayPostedTimes.DayIds) : null;
                    //        GetUserProfileForTimeSettings.Saturday = model.SaturdayPostedTimes.DayIds != null ? string.Join(",", model.SaturdayPostedTimes.DayIds) : null;
                    //        GetUserProfileForTimeSettings.Sunday = model.SundayPostedTimes.DayIds != null ? string.Join(",", model.SundayPostedTimes.DayIds) : null;
                    //        GetUserProfileForTimeSettings.UserProfileId = GetUserMatchProfileID;
                    //        mySQLEntities.SaveChanges();
                    //    }
                    //    else
                    //    {
                    //        var userprofiletimesetting = new userprofiletimesetting();

                    //        userprofiletimesetting.Monday = model.MondayPostedTimes.DayIds != null ? string.Join(",", model.MondayPostedTimes.DayIds) : null;
                    //        userprofiletimesetting.Tuesday = model.TuesdayPostedTimes.DayIds != null ? string.Join(",", model.TuesdayPostedTimes.DayIds) : null;
                    //        userprofiletimesetting.Wednesday = model.WednesdayPostedTimes.DayIds != null ? string.Join(",", model.WednesdayPostedTimes.DayIds) : null;
                    //        userprofiletimesetting.Thursday = model.ThursdayPostedTimes.DayIds != null ? string.Join(",", model.ThursdayPostedTimes.DayIds) : null;
                    //        userprofiletimesetting.Friday = model.FridayPostedTimes.DayIds != null ? string.Join(",", model.FridayPostedTimes.DayIds) : null;
                    //        userprofiletimesetting.Saturday = model.SaturdayPostedTimes.DayIds != null ? string.Join(",", model.SaturdayPostedTimes.DayIds) : null;
                    //        userprofiletimesetting.Sunday = model.SundayPostedTimes.DayIds != null ? string.Join(",", model.SundayPostedTimes.DayIds) : null;
                    //        userprofiletimesetting.UserProfileId = GetUserMatchProfileID;
                    //        mySQLEntities.userprofiletimesettings.Add(userprofiletimesetting);
                    //        mySQLEntities.SaveChanges();
                    //    }

                    //}


                    using (var SQLServerEntities = new EFMVCDataContex())
                    {

                        var GetUserProfileForTimeSettings = SQLServerEntities.UserProfileTimeSetting.Where(s => s.UserProfileId == model.UserProfileId).FirstOrDefault();
                        if (GetUserProfileForTimeSettings != null)
                        {
                            //var GetUserProfileForTimeSettings = mySQLEntities.userprofiletimesettings.Where(s => s.UserProfileTimeSettingsId == GetUserProfileTimeByID.UserProfileTimeSettingsId).FirstOrDefault();
                            GetUserProfileForTimeSettings.Monday = model.MondayPostedTimes.DayIds != null ? string.Join(",", model.MondayPostedTimes.DayIds) : null;
                            GetUserProfileForTimeSettings.Tuesday = model.TuesdayPostedTimes.DayIds != null ? string.Join(",", model.TuesdayPostedTimes.DayIds) : null;
                            GetUserProfileForTimeSettings.Wednesday = model.WednesdayPostedTimes.DayIds != null ? string.Join(",", model.WednesdayPostedTimes.DayIds) : null;
                            GetUserProfileForTimeSettings.Thursday = model.ThursdayPostedTimes.DayIds != null ? string.Join(",", model.ThursdayPostedTimes.DayIds) : null;
                            GetUserProfileForTimeSettings.Friday = model.FridayPostedTimes.DayIds != null ? string.Join(",", model.FridayPostedTimes.DayIds) : null;
                            GetUserProfileForTimeSettings.Saturday = model.SaturdayPostedTimes.DayIds != null ? string.Join(",", model.SaturdayPostedTimes.DayIds) : null;
                            GetUserProfileForTimeSettings.Sunday = model.SundayPostedTimes.DayIds != null ? string.Join(",", model.SundayPostedTimes.DayIds) : null;
                            SQLServerEntities.SaveChanges();
                        }
                        else
                        {
                            var userprofiletimesetting = new UserProfileTimeSetting();

                            userprofiletimesetting.Monday = model.MondayPostedTimes.DayIds != null ? string.Join(",", model.MondayPostedTimes.DayIds) : null;
                            userprofiletimesetting.Tuesday = model.TuesdayPostedTimes.DayIds != null ? string.Join(",", model.TuesdayPostedTimes.DayIds) : null;
                            userprofiletimesetting.Wednesday = model.WednesdayPostedTimes.DayIds != null ? string.Join(",", model.WednesdayPostedTimes.DayIds) : null;
                            userprofiletimesetting.Thursday = model.ThursdayPostedTimes.DayIds != null ? string.Join(",", model.ThursdayPostedTimes.DayIds) : null;
                            userprofiletimesetting.Friday = model.FridayPostedTimes.DayIds != null ? string.Join(",", model.FridayPostedTimes.DayIds) : null;
                            userprofiletimesetting.Saturday = model.SaturdayPostedTimes.DayIds != null ? string.Join(",", model.SaturdayPostedTimes.DayIds) : null;
                            userprofiletimesetting.Sunday = model.SundayPostedTimes.DayIds != null ? string.Join(",", model.SundayPostedTimes.DayIds) : null;
                            userprofiletimesetting.UserProfileId = model.UserProfileId;
                            SQLServerEntities.UserProfileTimeSetting.Add(userprofiletimesetting);
                            SQLServerEntities.SaveChanges();
                        }

                    }
                    if (result.Success)
                    {
                        TempData["sucess"] = "Record updated successfully.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["error"] = "Internal server error. Please try again";
                        return RedirectToAction("Index");
                    }
                }
            }

            TempData["error"] = "Internal server error. Please try again";
            return RedirectToAction("Index");
        }
        public UserProfileTimeSettingFormModel UserProfileTimeSettingFormModel(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            UserProfile userProfile = _profileRepository.GetById(id);

            if (userProfile != null)
            {
                if (userProfile.UserProfileTimeSettings != null && userProfile.UserProfileTimeSettings.Count != 0)
                {
                    UserProfileTimeSetting userProfileTimeSettings =
                        userProfile.UserProfileTimeSettings.FirstOrDefault();

                    var model = new UserProfileTimeSettingFormModel
                    {
                        UserProfileId =
                                            userProfileTimeSettings.UserProfileId,
                        UserProfileTimeSettingsId =
                                            userProfileTimeSettings.
                                            UserProfileTimeSettingsId
                    };

                    if (userProfileTimeSettings.Monday != null)
                        model.MondaySelectedTimes =
                            ConvertTimesArrayToList(userProfileTimeSettings.Monday.Split(",".ToCharArray()));

                    if (userProfileTimeSettings.Tuesday != null)
                        model.TuesdaySelectedTimes =
                            ConvertTimesArrayToList(userProfileTimeSettings.Tuesday.Split(",".ToCharArray()));

                    if (userProfileTimeSettings.Wednesday != null)
                        model.WednesdaySelectedTimes =
                            ConvertTimesArrayToList(userProfileTimeSettings.Wednesday.Split(",".ToCharArray()));

                    if (userProfileTimeSettings.Thursday != null)
                        model.ThursdaySelectedTimes =
                            ConvertTimesArrayToList(userProfileTimeSettings.Thursday.Split(",".ToCharArray()));

                    if (userProfileTimeSettings.Friday != null)
                        model.FridaySelectedTimes =
                            ConvertTimesArrayToList(userProfileTimeSettings.Friday.Split(",".ToCharArray()));

                    if (userProfileTimeSettings.Saturday != null)
                        model.SaturdaySelectedTimes =
                            ConvertTimesArrayToList(userProfileTimeSettings.Saturday.Split(",".ToCharArray()));

                    if (userProfileTimeSettings.Sunday != null)
                        model.SundaySelectedTimes =
                            ConvertTimesArrayToList(userProfileTimeSettings.Sunday.Split(",".ToCharArray()));

                    model.AvailableTimes = GetTimes();

                    return model;
                }
            }
            if (id == 0)
            {
                return new UserProfileTimeSettingFormModel
                { UserProfileId = id, AvailableTimes = GetTimes() };
            }
            return new UserProfileTimeSettingFormModel
            { UserProfileId = userProfile.UserProfileId, AvailableTimes = GetTimes() };
        }
        public IList<TimeOfDay> ConvertTimesArrayToList(string[] selectedTimes)
        {
            return selectedTimes.Select(time => new TimeOfDay { Id = time, Name = time, IsSelected = true }).ToList();
        }
        public IList<TimeOfDay> GetTimes()
        {
            IList<TimeOfDay> times = new List<TimeOfDay>();
            times.Add(new TimeOfDay { Id = "01:00", Name = "01:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "02:00", Name = "02:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "03:00", Name = "03:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "04:00", Name = "04:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "05:00", Name = "05:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "06:00", Name = "06:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "07:00", Name = "07:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "08:00", Name = "08:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "09:00", Name = "09:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "10:00", Name = "10:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "11:00", Name = "11:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "12:00", Name = "12:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "13:00", Name = "13:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "14:00", Name = "14:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "15:00", Name = "15:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "16:00", Name = "16:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "17:00", Name = "17:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "18:00", Name = "18:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "19:00", Name = "19:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "20:00", Name = "20:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "21:00", Name = "21:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "22:00", Name = "22:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "23:00", Name = "23:00", IsSelected = false });
            times.Add(new TimeOfDay { Id = "24:00", Name = "24:00", IsSelected = false });

            return times;
        }
    }
}