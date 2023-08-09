using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model.Entities;
using EFMVC.Web.Areas.Admin.Models;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Helpers;
using EFMVC.Web.Models;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("AdvertCategory")]
    public class AdvertCategoryController : Controller
    {
        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The _advertCategory Repository
        /// </summary>
        private readonly IAdvertCategoryRepository _advertCategoryRepository;

        /// <summary>
        /// The _country Repository
        /// </summary>
        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// The _advert Repository
        /// </summary>
        private readonly IAdvertRepository _advertRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        public AdvertCategoryController(ICommandBus commandBus, IUserRepository userRepository, IAdvertCategoryRepository advertCategoryRepository, ICountryRepository countryRepository, IAdvertRepository advertRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _advertCategoryRepository = advertCategoryRepository;
            _countryRepository = countryRepository;
            _advertRepository = advertRepository;
        }

        // GET: Admin/AdvertCategory
        [Route("Index")]
        public ActionResult Index()
        {
            List<AdvertCategoryResult> _result = FillAdvertCategoryResult();
            SearchClass.AdvertCategoryFilter _filterCritearea = new SearchClass.AdvertCategoryFilter();
            FillCountry();
            return View(Tuple.Create(_result, _filterCritearea));
        }

        //Add 01-07-2019
        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                List<AdvertCategoryResult> _result = new List<AdvertCategoryResult>();
                IEnumerable<AdvertCategory> advertCategory = null;
                string status = string.Empty;
                ViewBag.SearchResult = false;
                var cnt = 10;
                int userId = 0;

                bool searchValue = false;
                List<String> columnSearch = new List<string>();

                if (param.Columns != null)
                {
                    foreach (var col in param.Columns)
                    {
                        columnSearch.Add(col.Search.Value);
                        if (!string.IsNullOrEmpty(col.Search.Value) && col.Search.Value != "null")
                            searchValue = true;
                    }
                }

                if (searchValue == true)
                {
                    #region Search Functionality
                    string AdvertCategoryName = "";
                    int[] CountryId = new int[cnt];

                    if (!String.IsNullOrEmpty(columnSearch[0]))
                    {
                        if (columnSearch[0] != "null")
                        {
                            AdvertCategoryName = columnSearch[0].ToString();
                        }
                        else
                        {
                            columnSearch[0] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[1]))
                    {
                        if (columnSearch[1] != "null")
                        {
                            CountryId = columnSearch[1].Split(',').Select(a => (int)Convert.ToInt32(a)).ToArray();
                        }
                        else
                        {
                            columnSearch[1] = null;
                        }
                    }

                    advertCategory =  _advertCategoryRepository.GetAll().OrderByDescending(top => top.CreatedDate);
                    foreach (var item in advertCategory)
                    {
                        _result.Add(new AdvertCategoryResult { AdvertCategoryId = item.AdvertCategoryId, Name = item.Name, CountryId = item.CountryId, CountryName = item.CountryId == null ? "-" : item.Country.Name, CreatedDate = item.CreatedDate == null ? null : item.CreatedDate.Value.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate });
                    }

                    if (columnSearch[0] != null)
                    {
                        _result = _result.Where(top => top.Name == AdvertCategoryName).ToList();
                    }
                    if (columnSearch[1] != null)
                    {
                        _result = _result.Where(top => (CountryId.Contains(Convert.ToInt32(top.CountryId)))).ToList();
                    }

                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();

                    #endregion
                }
                else
                {
                    advertCategory = _advertCategoryRepository.GetAll().OrderByDescending(top => top.CreatedDate);
                    foreach (var item in advertCategory)
                    {
                        _result.Add(new AdvertCategoryResult { AdvertCategoryId = item.AdvertCategoryId, Name = item.Name, CountryId = item.CountryId, CountryName = item.CountryId == null ? "-" : item.Country.Name, CreatedDate = item.CreatedDate == null ? null : item.CreatedDate.Value.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate });
                    }

                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();
                }

                _result = ApplySorting(param, _result);
                //_result = _result.Skip(param.Start).Take(param.Length).ToList();

                DTResult<AdvertCategoryResult> result = new DTResult<AdvertCategoryResult>
                {
                    draw = param.Draw,
                    data = _result,
                    recordsFiltered = cnt,
                    recordsTotal = cnt
                };

                return Json(result);

            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        //Add 01-07-2019
        // Function For Filter/Sorting AdvertCategory Data
        private static List<AdvertCategoryResult> ApplySorting(DTParameters param, List<AdvertCategoryResult> result)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Name).ToList();
                    else
                        result = result.OrderByDescending(top => top.Name).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CountryName).ToList();
                    else
                        result = result.OrderByDescending(top => top.CountryName).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CreatedDateSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.CreatedDateSort).ToList();
                }
            }
            return result;
        }

        // Listing AdvertCategory
        public List<AdvertCategoryResult> FillAdvertCategoryResult()
        {
            List<AdvertCategoryResult> _result = new List<AdvertCategoryResult>();
            var advertCategoryResult = _advertCategoryRepository.GetAll().OrderByDescending(top => top.CreatedDate);
            foreach (var item in advertCategoryResult)
            {
                _result.Add(new AdvertCategoryResult { AdvertCategoryId = item.AdvertCategoryId, Name = item.Name, CountryId = item.CountryId, CountryName = item.CountryId == null ? "-" : item.Country.Name, CreatedDate = item.CreatedDate == null ? null : item.CreatedDate.Value.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate });
            }
            return _result;
        }

        //Add 02-04-2019
        [Route("FillUserDropdownAJAX")]
        [HttpPost]
        public ActionResult FillUserDropdownAJAX(string UserName)
        {
            try
            {
                if (!string.IsNullOrEmpty(UserName))
                {
                    var userdetails = _userRepository.GetMany(top => (top.FirstName + " " + top.LastName).Contains(UserName)).Select(top => new
                    // var userdetails = _userRepository.GetMany(top => top.FirstName.Contains(UserName) || top.LastName.Contains(UserName)).Select(top => new
                    {
                        Name = top.FirstName + " " + top.LastName,
                        UserId = top.UserId,
                    }).ToList();
                    ViewBag.userdetails = new MultiSelectList(userdetails, "UserId", "Name");
                    var data = userdetails.Select(x => new { id = x.UserId, name = x.Name }).ToArray();
                    return Json(data);
                }
                else
                {
                    return Json("");
                }
            }
            catch (Exception ex)
            {
                return Json("error");
            }
        }

        //Fill Counrty
        public void FillCountry()
        {
            var countrydetails = _countryRepository.GetAll().Select(top => new
            {
                Name = top.Name,
                Id = top.Id
            }).ToList();
            ViewBag.countrydetails = new MultiSelectList(countrydetails, "Id", "Name");
        }

        // Add AdvertCategory
        [Route("AddAdvertCategory")]
        public ActionResult AddAdvertCategory()
        {
            AdvertCategoryFormModel _rewardModel = new AdvertCategoryFormModel();
            FillCountry();

            return View(_rewardModel);
        }

        // Save AdvertCategory
        [Route("AddAdvertCategory")]
        [HttpPost]
        public ActionResult AddAdvertCategory(AdvertCategoryFormModel advertCategoryFormModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var rewardExist = _advertCategoryRepository.Get(top => top.Name.Equals(advertCategoryFormModel.Name) && top.CountryId == advertCategoryFormModel.CountryId);
                    if (rewardExist != null)
                    {
                        TempData["Error"] = advertCategoryFormModel.Name + " Record Exist.";
                        FillCountry();
                        ViewBag.name = advertCategoryFormModel.Name;
                        return View("AddAdvertCategory", advertCategoryFormModel);
                    }

                    advertCategoryFormModel.CreatedDate = DateTime.Now;

                    CreateOrUpdateAdvertCategoryCommand command =
                    Mapper.Map<AdvertCategoryFormModel, CreateOrUpdateAdvertCategoryCommand>(advertCategoryFormModel);
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        //TempData["status"] = "Record added successfully.";
                        TempData["status"] = "Advert Category " + advertCategoryFormModel.Name + " added successfully.";
                        return RedirectToAction("Index");
                    }
                }
                return View(advertCategoryFormModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "fail";
                return View(advertCategoryFormModel);
            }
        }

        // Edit AdvertCategory
        [Route("UpdateAdvertCategory")]
        public ActionResult UpdateAdvertCategory(int id)
        {
            var advertCategory = _advertCategoryRepository.Get(top => top.AdvertCategoryId == id);
            ViewBag.name = advertCategory.Name;
            AdvertCategoryFormModel command =
                Mapper.Map<AdvertCategory, AdvertCategoryFormModel>(advertCategory);
            FillCountry();
            return View(command);
        }

        // Update AdvertCategory
        [Route("UpdateAdvertCategory")]
        [HttpPost]
        public ActionResult UpdateAdvertCategory(AdvertCategoryFormModel advertCategoryFormModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var rewardExist = _advertCategoryRepository.Get(top => top.Name.Equals(advertCategoryFormModel.Name) && top.CountryId == advertCategoryFormModel.CountryId && top.AdvertCategoryId != advertCategoryFormModel.AdvertCategoryId);
                    if (rewardExist != null)
                    {
                        TempData["Error"] = advertCategoryFormModel.Name + " Record Exist.";
                        FillCountry();
                        ViewBag.name = advertCategoryFormModel.Name;
                        return View("UpdateAdvertCategory", advertCategoryFormModel);
                    }

                    advertCategoryFormModel.UpdatedDate = DateTime.Now;

                    CreateOrUpdateAdvertCategoryCommand command =
                        Mapper.Map<AdvertCategoryFormModel, CreateOrUpdateAdvertCategoryCommand>(advertCategoryFormModel);

                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        //TempData["status"] = "Record updated successfully.";
                        TempData["status"] = "Advert Category " + advertCategoryFormModel.Name + " updated successfully.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Error"] = "Something went wrong.";
                    return View(advertCategoryFormModel);
                }
                return View(advertCategoryFormModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "fail";
                return View(advertCategoryFormModel);
            }
        }

        // Delete AdvertCategory
        [Route("DeleteAdvertCategory")]
        [HttpPost]
        public ActionResult DeleteAdvertCategory(string id)
        {
            try
            {
                int advertCategoryId = int.Parse(id);
                AdvertCategoryFormModel model = new AdvertCategoryFormModel();
                model.AdvertCategoryId = advertCategoryId;

                var advertCategory = _advertCategoryRepository.GetById(advertCategoryId);

                var advertExists = _advertRepository.GetMany(top => top.AdvertCategoryId == advertCategoryId).ToList();
                if (advertExists != null && advertExists.Count() > 0)
                {
                    return Json("Exists");
                    //return Json(new { success = "successbudget", value = advertCategory.Name + "is already used in Advert" }, JsonRequestBehavior.AllowGet);
                }

                DeleteAdvertCategoryCommand command =
                    Mapper.Map<AdvertCategoryFormModel, DeleteAdvertCategoryCommand>(model);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    var finalresult = new List<AdvertCategoryResult>();
                    finalresult = FillAdvertCategoryResult();

                    return PartialView("_AdvertCategoryDetails", finalresult);
                }
                return Json("Fail");
            }
            catch (Exception ex)
            {
                return Json("Fail");
            }
        }

        // Search AdvertCategory
        [Route("SearchAdvertCategory")]
        public ActionResult SearchAdvertCategory([Bind(Prefix = "Item2")]SearchClass.AdvertCategoryFilter _filterCritearea, int?[] CountryId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<AdvertCategoryResult> _result = new List<AdvertCategoryResult>();
                var finalresult = new List<AdvertCategoryResult>();
                if (_filterCritearea != null)
                {
                    _result = FillAdvertCategoryResult();
                    finalresult = getAdvertCategoryResult(_result, _filterCritearea, CountryId);
                }
                else
                {
                    _result = FillAdvertCategoryResult();
                    finalresult = getAdvertCategoryResult(_result, _filterCritearea, CountryId);
                }

                return PartialView("_AdvertCategoryDetails", finalresult);
            }
            else
            {
                return PartialView("_AdvertCategoryDetails", "notauthorise");
            }
        }

        // Search AdvertCategory
        public List<AdvertCategoryResult> getAdvertCategoryResult(List<AdvertCategoryResult> advertCategoryresult, SearchClass.AdvertCategoryFilter _filterCritearea, int?[] CountryId)
        {
            if (advertCategoryresult != null && _filterCritearea != null)
            {

                if (CountryId != null)
                {
                    advertCategoryresult = advertCategoryresult.Where(top => CountryId.Contains(top.CountryId)).ToList();
                }
                if (_filterCritearea.Name != null)
                {
                    //advertCategoryresult = advertCategoryresult.Where(top => top.Name.ToLower() == _filterCritearea.Name.ToLower()).ToList();
                    advertCategoryresult = advertCategoryresult.Where(top => top.Name.ToLower().Contains(_filterCritearea.Name.ToLower())).ToList();
                }

            }
            else
            {
                if (CountryId != null)
                {
                    advertCategoryresult = advertCategoryresult.Where(top => CountryId.Contains(top.CountryId)).ToList();
                }
            }
            return advertCategoryresult;
        }
    }
}