using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using EFMVC.Data.Repositories;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Web.ViewModels;
using EFMVC.Domain.Commands;
using EFMVC.Domain.Commands.ProfileMatchInfo;
using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.Model.Entities;
using EFMVC.Data;
using EFMVC.Web.Areas.Admin.SearchClass.Models;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("ProfileMatchInformation")]
    public class ProfileMatchInformationController : Controller
    {
        // GET: Admin/ProfileMatchInformation
        private readonly ICountryRepository _countryRepository;
        private readonly IProfileMatchInformationRepository _profileMatchInformationRepository;
        private readonly IProfileMatchLabelRepository _profileMatchLabelRepository;
        private readonly ICommandBus _commandBus;

        public ProfileMatchInformationController(ICommandBus commandBus, ICountryRepository countryRepository, IProfileMatchInformationRepository profileMatchInformationRepository, IProfileMatchLabelRepository profileMatchLabelRepository)
        {
            _commandBus = commandBus;
            _countryRepository = countryRepository;
            _profileMatchInformationRepository = profileMatchInformationRepository;
            _profileMatchLabelRepository = profileMatchLabelRepository;
        }

        static List<string> profileLabelList;

        //Old Function
        //[Route("Index")]
        //public ActionResult Index()
        //{
        //    FillCountry();
        //    var GetAll = _profileMatchInformationRepository.GetAll().ToList();
        //    return View(GetAll);
        //}

        //New Function
        [Route("Index")]
        public ActionResult Index()
        {
            FillCountry();
            FillProfileType();
            List<ProfileInformationResult> _result = FillProfileMatchInformationResult();
            SearchClass.ProfileInformationFilter _filterCritearea = new SearchClass.ProfileInformationFilter();
            return View(Tuple.Create(_result, _filterCritearea));

            //FillCountry();
            //var GetAll = _profileMatchInformationRepository.GetAll().ToList();
            //return View(GetAll);
        }

        public void FillCountry()
        {
            var countrydetails = _countryRepository.GetAll().Select(top => new
            {
                Name = top.Name,
                Id = top.Id
            }).ToList();
            ViewBag.country = new MultiSelectList(countrydetails, "Id", "Name");

        }

        public void FillProfileType()
        {
            IEnumerable<Common.ProfileType> profileTypes = Enum.GetValues(typeof(Common.ProfileType))
                                                     .Cast<Common.ProfileType>();
            var profileType = profileTypes.Select(top => new
            {
                Name = top.ToString(),
                Id = top.ToString()
            }).ToList();
            ViewBag.profileType = new MultiSelectList(profileType, "Id", "Name");
        }

        public List<ProfileInformationResult> FillProfileMatchInformationResult()
        {
            List<ProfileInformationResult> _result = new List<ProfileInformationResult>();
            var profileMatchInformationResult = _profileMatchInformationRepository.GetAll().ToList().OrderByDescending(a => a.Id);
            foreach (var item in profileMatchInformationResult)
            {
                _result.Add(new ProfileInformationResult { Id = item.Id, ProfileName = item.ProfileName, ProfileType = item.ProfileType, CountryName = item.Country.Name, CountryId = (int)item.CountryId, Status = item.IsActive });
            }
            return _result;
        }

        [Route("AddProfileInfo")]
        public ActionResult AddProfileInfo()
        {
            FillCountry();
            FillProfileType();
            ProfileMatchInformationFormModel _profileInfoModel = new ProfileMatchInformationFormModel();
            _profileInfoModel.IsActive = true;
            _profileInfoModel.profileMatchLabelFormModels = new List<ProfileMatchLabelFormModel>();
            return View(_profileInfoModel);
        }
        [Route("AddProfileInfo")]
        [HttpPost]
        public ActionResult AddProfileInfo(ProfileMatchInformationFormModel model)
        {
            FillCountry();
            FillProfileType();
            try
            {
                if ((profileLabelList != null || profileLabelList.Count() > 0) && string.IsNullOrEmpty(profileLabelList[0]) != true)
                {
                    if (ModelState.IsValid)
                    {
                        var profileLabelExist = _profileMatchInformationRepository.Get(top => top.ProfileName.Equals(model.ProfileName) && top.CountryId == model.CountryId && top.ProfileType.ToLower().Equals(model.ProfileType));
                        if (profileLabelExist != null)
                        {
                            TempData["Error"] = model.ProfileName + " Record Exist.";
                            FillCountry();
                            FillProfileType();
                            ViewBag.name = model.ProfileName;
                            return View("AddProfileInfo", model);
                        }
                        CreateOrUpdateProfileMatchInfoCommand command =
                   Mapper.Map<ProfileMatchInformationFormModel, CreateOrUpdateProfileMatchInfoCommand>(model);
                        ICommandResult result = _commandBus.Submit(command);
                        if (result.Success)
                        {
                            bool profileLabelInfo = SaveProfileLabelInfo(result.Id, profileLabelList);
                            if (profileLabelInfo == true)
                            {
                                profileLabelList = null;
                                //TempData["status"] = "Record added successfully.";
                                TempData["status"] = "Profile Name " + model.ProfileName + " added successfully.";
                                return RedirectToAction("Index");
                            }
                        }
                        return View(model);
                    }
                }
                else
                {
                    FillCountry();
                    FillProfileType();
                    TempData["ProfileLabelError"] = "Please add atleast one record.";
                    return View("AddProfileInfo", model);
                }
            }
            catch (Exception ex)
            {
                FillCountry();
                FillProfileType();
                TempData["ProfileLabelError"] = "Please add atleast one record!";
                return View("AddProfileInfo", model);
                //TempData["Error"] = ex.Message;
                //return View(model);
            }


            return View(model);
        }

        [Route("UpdateProfileInfo")]
        public ActionResult UpdateProfileInfo(int id)
        {
            try
            {
                var ProfileMatchData = _profileMatchInformationRepository.Get(top => top.Id == id);
                ViewBag.name = ProfileMatchData.ProfileName;
                ProfileMatchInformationFormModel command =
                   Mapper.Map<ProfileMatchInformation, ProfileMatchInformationFormModel>(ProfileMatchData);
                var ProfileMatchLabelData = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId == ProfileMatchData.Id).ToList();
                List<ProfileMatchLabelFormModel> command1 = Mapper.Map<List<ProfileMatchLabel>, List<ProfileMatchLabelFormModel>>(ProfileMatchLabelData);
                command.profileMatchLabelFormModels = command1;
                Session["ProfileId"] = ProfileMatchData.Id;
                FillCountry();
                FillProfileType();
                return View(command);
            }
            catch (Exception ex)
            {
                ProfileMatchInformationFormModel command = new ProfileMatchInformationFormModel();
                TempData["Error"] = ex.Message;
                return View(command);
            }

        }

        [Route("UpdateProfileInfo")]
        [HttpPost]
        public ActionResult UpdateProfileInfo(ProfileMatchInformationFormModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var profileLabelExist = _profileMatchInformationRepository.Get(top => top.ProfileName.Equals(model.ProfileName) && top.Id != model.Id && top.CountryId == model.CountryId && top.ProfileType.ToLower().Equals(model.ProfileType));
                    if (profileLabelExist != null)
                    {
                        TempData["Error"] = model.ProfileName + " Record Exist.";
                        ViewBag.name = model.ProfileName;
                        FillCountry();
                        FillProfileType();
                        return View("UpdateProfileInfo", model);
                    }
                    var profileLabelCount = _profileMatchLabelRepository.GetMany(top => top.ProfileMatchInformationId.Equals(model.Id)).Count();
                    if (profileLabelCount > 0)
                    {
                            CreateOrUpdateProfileMatchInfoCommand command =
                        Mapper.Map<ProfileMatchInformationFormModel, CreateOrUpdateProfileMatchInfoCommand>(model);
                        ICommandResult result = _commandBus.Submit(command);
                        if (result.Success)
                        {
                            //TempData["status"] = "Record updated successfully.";
                            TempData["status"] = "Profile Name " + model.ProfileName + " updated successfully.";
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        FillCountry();
                        FillProfileType();
                        TempData["ProfileLabelError"] = "Please add atleast one record.";
                        return RedirectToAction("UpdateProfileInfo", new { @id = Convert.ToInt32(model.Id) });
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                FillCountry();
                FillProfileType();
                TempData["ProfileLabelError"] = "Please add atleast one record.";
                return RedirectToAction("UpdateProfileInfo", new { @id = Convert.ToInt32(model.Id) });
                //TempData["Error"] = ex.Message;
                //return View(model);
            }

        }

        // Add Profile Label
        [Route("AddProfileLabel")]
        [HttpPost]
        public ActionResult AddProfileLabel(string profileLabel)
        {
            try
            {
                if (profileLabel != "" || profileLabel != null)
                {
                    int profileId = int.Parse(Session["ProfileId"].ToString());

                    EFMVCDataContex db = new EFMVCDataContex();
                    var profileLabelExist = _profileMatchLabelRepository.Get(top => top.ProfileLabel.Equals(profileLabel) && top.ProfileMatchInformationId == profileId);
                    if (profileLabelExist != null)
                    {
                        return Json("Exist");
                    }
                    else
                    {
                        ProfileMatchInformationFormModel model = new ProfileMatchInformationFormModel();

                        ProfileMatchLabel profileMatchLabel = new ProfileMatchLabel();
                        profileMatchLabel.ProfileLabel = profileLabel;
                        profileMatchLabel.ProfileMatchInformationId = profileId;
                        profileMatchLabel.CreatedDate = DateTime.Now;
                        profileMatchLabel.UpdatedDate = DateTime.Now;
                        db.ProfileMatchLabel.Add(profileMatchLabel);
                        db.SaveChanges();

                        var ProfileLabel = _profileMatchLabelRepository.GetAll().ToList();
                        ProfileLabel = ProfileLabel.Where(top => top.ProfileMatchInformationId == profileId).ToList();
                        List<ProfileMatchLabelFormModel> command = Mapper.Map<List<ProfileMatchLabel>, List<ProfileMatchLabelFormModel>>(ProfileLabel);
                        model.profileMatchLabelFormModels = command;
                        return PartialView("_AddProfileMatchLabel", model);
                    }
                }
                return Json("Fail");
            }
            catch (Exception ex)
            {
                return Json("Fail");
            }
        }

        // Update Profile Label
        [Route("UpdateProfileLabel")]
        [HttpPost]
        public ActionResult UpdateProfileLabel(string profileLabelId, string profileLabel)
        {
            try
            {
                if ((profileLabelId != "" || profileLabelId != null) && string.IsNullOrEmpty(profileLabelList[0]) != true)
                {
                    EFMVCDataContex db = new EFMVCDataContex();
                    int Id = int.Parse(profileLabelId);
                    int profileId = int.Parse(Session["ProfileId"].ToString());
                    var profileLabelExist = _profileMatchLabelRepository.Get(top => top.ProfileLabel.Equals(profileLabel) && top.Id != Id && top.ProfileMatchInformationId == profileId);
                    if (profileLabelExist != null)
                    {
                        return Json("Exist");
                    }
                    else
                    {
                        ProfileMatchInformationFormModel model = new ProfileMatchInformationFormModel();
                        var profileLabelData = db.ProfileMatchLabel.Where(top => top.Id == Id).FirstOrDefault();
                        profileLabelData.ProfileLabel = profileLabel;
                        profileLabelData.UpdatedDate = DateTime.Now;
                        db.SaveChanges();
                        var ProfileLabel = _profileMatchLabelRepository.GetAll().ToList();
                        ProfileLabel = ProfileLabel.Where(top => top.ProfileMatchInformationId == profileId).ToList();
                        List<ProfileMatchLabelFormModel> command = Mapper.Map<List<ProfileMatchLabel>, List<ProfileMatchLabelFormModel>>(ProfileLabel);
                        model.profileMatchLabelFormModels = command;
                        return PartialView("_AddProfileMatchLabel", model);
                    }
                }
                return Json("Fail");
            }
            catch (Exception ex)
            {
                return Json("Fail");
            }
        }

        // Delete Profile Label
        [Route("DeleteProfileLabel")]
        [HttpPost]
        public ActionResult DeleteProfileLabel(string profileLabelId)
        {
            try
            {
                if (profileLabelId != "" || profileLabelId != null)
                {
                    int profileId = int.Parse(Session["ProfileId"].ToString());
                    EFMVCDataContex db = new EFMVCDataContex();
                    int Id = int.Parse(profileLabelId);
                    ProfileMatchInformationFormModel model = new ProfileMatchInformationFormModel();
                    var profileLabelData = db.ProfileMatchLabel.Where(top => top.Id == Id).FirstOrDefault();
                    db.ProfileMatchLabel.Remove(profileLabelData);
                    db.SaveChanges();
                    var ProfileLabel = _profileMatchLabelRepository.GetAll().ToList();
                    ProfileLabel = ProfileLabel.Where(top => top.ProfileMatchInformationId == profileId).ToList();
                    List<ProfileMatchLabelFormModel> command = Mapper.Map<List<ProfileMatchLabel>, List<ProfileMatchLabelFormModel>>(ProfileLabel);
                    model.profileMatchLabelFormModels = command;
                    return PartialView("_AddProfileMatchLabel", model);
                }
                return Json("Fail");
            }
            catch (Exception ex)
            {
                return Json("Fail");
            }
        }

        // Delete Profile Label
        [Route("CancelProfileLabel")]
        [HttpPost]
        public ActionResult CancelProfileLabel()
        {
            try
            {
                int profileId = int.Parse(Session["ProfileId"].ToString());
                EFMVCDataContex db = new EFMVCDataContex();
                ProfileMatchInformationFormModel model = new ProfileMatchInformationFormModel();
                var ProfileLabel = _profileMatchLabelRepository.GetAll().ToList();
                ProfileLabel = ProfileLabel.Where(top => top.ProfileMatchInformationId == profileId).ToList();
                List<ProfileMatchLabelFormModel> command = Mapper.Map<List<ProfileMatchLabel>, List<ProfileMatchLabelFormModel>>(ProfileLabel);
                model.profileMatchLabelFormModels = command;
                return PartialView("_AddProfileMatchLabel", model);
            }
            catch (Exception ex)
            {
                return Json("Fail");
            }
        }

        // Save Profile Label
        [Route("SaveProfileLabel")]
        [HttpPost]
        public ActionResult SaveProfileLabel(List<string> allrecord)
        {
            try
            {
                if (allrecord.Count() > 0)
                {
                    profileLabelList = allrecord;

                    return Json("Success");
                }
                profileLabelList = null;
                return Json("Fail");
            }
            catch (Exception ex)
            {
                return Json("Fail");
            }
        }

        public bool SaveProfileLabelInfo(int profileId, List<string> profileLabelList)
        {
            try
            {
                if (profileLabelList != null || profileLabelList.Count() > 0)
                {
                    EFMVCDataContex db = new EFMVCDataContex();
                    var profileLabelExist = _profileMatchLabelRepository.Get(top => profileLabelList.Contains(top.ProfileLabel) && top.ProfileMatchInformationId == profileId);
                    if (profileLabelExist != null)
                    {
                        return false;
                    }
                    else
                    {
                        //ProfileMatchInformationFormModel model = new ProfileMatchInformationFormModel();

                        foreach (var profileLabel in profileLabelList)
                        {
                            ProfileMatchLabel profileMatchLabel = new ProfileMatchLabel();
                            profileMatchLabel.ProfileLabel = profileLabel;
                            profileMatchLabel.ProfileMatchInformationId = profileId;
                            profileMatchLabel.CreatedDate = DateTime.Now;
                            profileMatchLabel.UpdatedDate = DateTime.Now;
                            db.ProfileMatchLabel.Add(profileMatchLabel);
                            db.SaveChanges();
                        }

                        //var ProfileLabel = _profileMatchLabelRepository.GetAll().ToList();
                        //ProfileLabel = ProfileLabel.Where(top => top.ProfileMatchInformationId == profileId).ToList();
                        //List<ProfileMatchLabelFormModel> command = Mapper.Map<List<ProfileMatchLabel>, List<ProfileMatchLabelFormModel>>(ProfileLabel);
                        //model.profileMatchLabelFormModels = command;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [Route("SearchProfileMatchInformation")]
        public ActionResult SearchProfileMatchInformation([Bind(Prefix = "Item2")]SearchClass.ProfileInformationFilter _filterCritearea, string[] ProfileType, int[] CountryId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<ProfileInformationResult> _result = new List<ProfileInformationResult>();
                var finalresult = new List<ProfileInformationResult>();
                if (_filterCritearea != null)
                {
                    _result = FillProfileMatchInformationResult();
                    finalresult = getProfileMatchInformationResult(_result, _filterCritearea, ProfileType, CountryId);
                }
                else
                {
                    _result = FillProfileMatchInformationResult();
                    finalresult = getProfileMatchInformationResult(_result, _filterCritearea, ProfileType, CountryId);
                }

                return PartialView("_ProfileInfoDetails", finalresult);
            }
            else
            {
                return PartialView("_ProfileInfoDetails", "notauthorise");
            }
        }

        public List<ProfileInformationResult> getProfileMatchInformationResult(List<ProfileInformationResult> profileMatchInformationresult, SearchClass.ProfileInformationFilter _filterCritearea, string[] ProfileType, int[] CountryId)
        {
            if (profileMatchInformationresult != null && _filterCritearea != null)
            {
                if (ProfileType != null)
                {
                    profileMatchInformationresult = profileMatchInformationresult.Where(top => ProfileType.Contains(top.ProfileType)).ToList();
                }
                if (CountryId != null)
                {
                    profileMatchInformationresult = profileMatchInformationresult.Where(top => CountryId.Contains(top.CountryId)).ToList();
                }
                if ((_filterCritearea.ProfileName != null))
                {
                    //profileMatchInformationresult = profileMatchInformationresult.Where(top => top.ProfileName == _filterCritearea.ProfileName).ToList();
                    profileMatchInformationresult = profileMatchInformationresult.Where(top => top.ProfileName.ToLower().Contains(_filterCritearea.ProfileName.ToLower())).ToList();
                }
            }
            else
            {
                if (ProfileType != null)
                {
                    profileMatchInformationresult = profileMatchInformationresult.Where(top => ProfileType.Contains(top.ProfileType)).ToList();
                }
                if (CountryId != null)
                {
                    profileMatchInformationresult = profileMatchInformationresult.Where(top => CountryId.Contains(top.CountryId)).ToList();
                }
            }
            return profileMatchInformationresult;
        }
    }
}