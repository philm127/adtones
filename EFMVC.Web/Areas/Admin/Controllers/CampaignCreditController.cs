using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using EFMVC.Web.Areas.Admin.Models;
using EFMVC.Web.Areas.Admin.ViewModel;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Helpers;
using EFMVC.Web.ViewModels;
using EFMVC.Model.Entities;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("CampaignCredit")]
    public class CampaignCreditController : Controller
    {
        //
        // GET: /Admin/UserCredit/

        // GET: /AdminQuestion/

        /// <summary>
        /// The _client repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        private readonly ICampaignProfileRepository _profileRepository;
        private readonly ICampaignCreditPeriodRepository _campaignCreditPeriodRepository;
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        public CampaignCreditController(ICommandBus commandBus, IUserRepository userRepository, ICampaignProfileRepository profileRepository, ICampaignCreditPeriodRepository campaignCreditPeriodRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _profileRepository = profileRepository;
            _campaignCreditPeriodRepository = campaignCreditPeriodRepository;
        }

        [Route("Index")]
        public ActionResult Index()
        {
            List<CampaignCreditResult> _result = FillCampaignCreditResult();
            SearchClass.CampaignCreditFilter _filterCritearea = new SearchClass.CampaignCreditFilter();

            var advertiserDropdown = (from item in _result
                                      select new
                                      {
                                          Text = item.UserName,
                                          Value = item.UserId
                                      }).Distinct().ToList();
            ViewBag.advertiserDetails = new MultiSelectList(advertiserDropdown, "Value", "Text");

            var campaignDropdown = (from item in _result
                                    select new
                                    {
                                        Text = item.CampaignName,
                                        Value = item.CampaignId
                                    }).Distinct().ToList();
            ViewBag.campaignDetails = new MultiSelectList(campaignDropdown, "Value", "Text");

            return View(Tuple.Create(_result, _filterCritearea));
        }

        public List<CampaignCreditResult> FillCampaignCreditResult()
        {
           
            var result = _campaignCreditPeriodRepository.GetAll().Select(s => new CampaignCreditResult
            {
              CampaignCreditPeriodId = s.CampaignCreditPeriodId, UserId = s.UserId, UserName = s.User.FirstName + " " + s.User.LastName,
              CampaignId = s.CampaignProfileId, CampaignName = s.CampaignProfile.CampaignName, CreditPeriod = s.CreditPeriod,
              CreatedDate = s.CreatedDate
            }).OrderByDescending(top => top.CreatedDate).ToList();

         

            return result;
        }


        [Route("SearchCampaignCredit")]
        public ActionResult SearchCampaignCredit(int[] UserId, int[] CampaignProfileId)
        {
            if (User.Identity.IsAuthenticated)
            {                
                var _result = FillCampaignCreditResult();
                var finalresult = getCampaignCreditResult(_result, UserId, CampaignProfileId);               
                return PartialView("_CampaignCreditDetails", finalresult);
            }
            else
            {
                return PartialView("_CampaignCreditDetails", "notauthorise");
            }
        }

        public List<CampaignCreditResult> getCampaignCreditResult(List<CampaignCreditResult> CampaignCreditResult, int[] UserId, int[] CampaignProfileId)
        {           
            if (UserId != null)
            {
                CampaignCreditResult = CampaignCreditResult.Where(top => UserId.Contains(top.UserId)).ToList();
            }
            if (CampaignProfileId != null)
            {
                CampaignCreditResult = CampaignCreditResult.Where(top => CampaignProfileId.Contains(top.CampaignId)).ToList();
            }
            return CampaignCreditResult;
        }

        [Route("AddCampaignCredit")]
        public ActionResult AddCampaignCredit()
        {
            CampaignCreditFormModel _creditmodel = new CampaignCreditFormModel();          
            FillAdvertiserDropdown();
            return View(_creditmodel);
        }

        [Route("AddCampaignCredit")]
        [HttpPost]
        public ActionResult AddCampaignCredit(CampaignCreditFormModel _creditmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var campaignName = _profileRepository.GetById(_creditmodel.CampaignProfileId).CampaignName;
                    _creditmodel.CreatedDate = DateTime.Now;
                    _creditmodel.UpdatedDate = DateTime.Now;
                    CreateOrUpdateCampaignCreditPeriodCommand command =
                    Mapper.Map<CampaignCreditFormModel, CreateOrUpdateCampaignCreditPeriodCommand>(_creditmodel);
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        //TempData["success"] = "Record added successfully.";
                        TempData["success"] = "Credit Period added successfully for Campaign " + campaignName;
                        return RedirectToAction("Index");
                    }
                }
                return View(_creditmodel);
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.InnerException.ToString();
                return RedirectToAction("Index");
            }
           
        }

        [HttpGet]
        [Route("UpdateCampaignCredit")]
        public ActionResult UpdateCampaignCredit(int id)
        {
            var campaignCreditDetails = _campaignCreditPeriodRepository.GetById(id);
            CampaignCreditFormModel _creditmodel = new CampaignCreditFormModel();
            _creditmodel.CampaignCreditPeriodId = campaignCreditDetails.CampaignCreditPeriodId;
            _creditmodel.UserId = campaignCreditDetails.UserId;
            _creditmodel.CampaignProfileId = campaignCreditDetails.CampaignProfileId;
            _creditmodel.CreditPeriod = campaignCreditDetails.CreditPeriod;
            _creditmodel.CreatedDate = campaignCreditDetails.CreatedDate;
            _creditmodel.UpdatedDate = campaignCreditDetails.UpdatedDate;
            _creditmodel.AdtoneServerCampaignCreditPeriodId = campaignCreditDetails.AdtoneServerCampaignCreditPeriodId;

            var userDetails = _userRepository.GetMany(top => top.RoleId == 3 && top.VerificationStatus == true && top.Activated == 1).Select(top => new SelectListItem
            {
                Text = top.FirstName + " " + top.LastName,
                Value = top.UserId.ToString(),
            }).ToList();
            ViewBag.userDetails = userDetails;

            var campaignDetails = _profileRepository.GetMany(s=>s.CampaignProfileId == _creditmodel.CampaignProfileId).Select(top => new SelectListItem
            {
                Text = top.CampaignName,
                Value = top.CampaignProfileId.ToString(),
            }).ToList();
            ViewBag.campaignDetails = campaignDetails;

            return View(_creditmodel);
        }

        [Route("UpdateCampaignCredit")]
        [HttpPost]
        public ActionResult UpdateCampaignCredit(CampaignCreditFormModel _creditmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var campaignName = _profileRepository.GetById(_creditmodel.CampaignProfileId).CampaignName;
                    _creditmodel.UpdatedDate = DateTime.Now;
                    CreateOrUpdateCampaignCreditPeriodCommand command =
                    Mapper.Map<CampaignCreditFormModel, CreateOrUpdateCampaignCreditPeriodCommand>(_creditmodel);
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        //TempData["success"] = "Record Updated successfully.";
                        TempData["success"] = "Credit Period updated successfully for Campaign " + campaignName;
                        return RedirectToAction("Index");
                    }
                }
                return View(_creditmodel);
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.InnerException.ToString();
                return RedirectToAction("Index");
            }
           
        }

        public void FillAdvertiserDropdown()
        {
            var userDetails = _userRepository.GetMany(top => top.RoleId == 3 && top.VerificationStatus == true && top.Activated == 1).Select(top => new SelectListItem
            {
                Text = top.FirstName + " " + top.LastName,
                Value = top.UserId.ToString(),
            }).ToList();
            ViewBag.userDetails = userDetails;
            List<SelectListItem> campaignDetails = new List<SelectListItem>();
            campaignDetails.Add(new SelectListItem { Text = "--Select Campaign--", Value = "" });
            ViewBag.campaignDetails = campaignDetails;
        }


        [Route("GetCampaignDetails")]
        [HttpPost]
        public ActionResult GetCampaignDetails(int? userId)
        {
            List<SelectListItem> _campaigndetails = new List<SelectListItem>();
            if (userId != null)
            {
                var usedCampaign = _campaignCreditPeriodRepository.GetAll().Select(s => s.CampaignProfileId).ToList();
                _campaigndetails = _profileRepository.GetMany(s => !usedCampaign.Contains(s.CampaignProfileId) && s.UserId == userId).Select(s => new SelectListItem { Text = s.CampaignName, Value = s.CampaignProfileId.ToString() }).ToList();
            }
            return Json(_campaigndetails);
        }

        [Route("GetCampaignDataByUserId")]
        [HttpPost]
        public ActionResult GetCampaignDataByUserId(int[] userId)
        {
            try
            {
                if (userId != null && userId.FirstOrDefault() != 0)
                {                  
                    var campaignDetails = _campaignCreditPeriodRepository.GetMany(s=> userId.Contains(s.UserId)).Select(top => new
                    {
                        Name = top.CampaignProfile.CampaignName,
                        Id = top.CampaignProfile.CampaignProfileId
                    }).ToList();
                    return Json(campaignDetails);                   
                }
                else
                {
                    var campaignDetails = _campaignCreditPeriodRepository.GetAll().Select(top => new
                    {
                        Name = top.CampaignProfile.CampaignName,
                        Id = top.CampaignProfile.CampaignProfileId
                    }).ToList();
                    return Json(campaignDetails);
                }
            }
            catch (Exception)
            {

                return Json("error");
            }
        }
                
    }
}
