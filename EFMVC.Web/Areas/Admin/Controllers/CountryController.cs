using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using EFMVC.Web.Areas.Admin.Models;
using EFMVC.Web.Areas.Admin.SearchClass;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Helpers;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Globalization;
using EFMVC.Web.Models;
using EFMVC.Web.Common;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("Country")]
    public class CountryController : Controller
    {
        // GET: Admin/Country


        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The _country repository
        /// </summary>
        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        public CountryController(ICommandBus commandBus, IUserRepository userRepository, ICountryRepository countryRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _countryRepository = countryRepository;
        }
        [Route("Index")]
        public ActionResult Index()
        {
            FillCountry();
            //List<CountryResult> _result = FillCountryResult();
            List<CountryResult> _result = new List<CountryResult>();
            SearchClass.CountryFilter _filterCritearea = new SearchClass.CountryFilter();
            return View(Tuple.Create(_result, _filterCritearea));
        }
        public void FillCountry()
        {
            var clientdetails = _countryRepository.GetAll().Select(top => new
            {
                Name = top.Name,
                Id = top.Id
            }).ToList();
            ViewBag.country = new MultiSelectList(clientdetails, "Id", "Name");

        }

        //Add 28-06-2019
        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                List<CountryResult> _result = new List<CountryResult>();
                IEnumerable<Country> country = null;
                DateTimeFormat dateTimeConvert = new DateTimeFormat();
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
                    int?[] CountryId = new int?[cnt];
                    DateTime CreatedDatefromdate = new DateTime();
                    DateTime CreatedDatetodate = new DateTime();

                    if (!String.IsNullOrEmpty(columnSearch[0]))
                    {
                        if (columnSearch[0] != "null")
                        {
                            CountryId = columnSearch[0].Split(',').Select(a => (int?)Convert.ToInt32(a)).ToArray();
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
                            var data = columnSearch[1].Split(',').ToArray();
                            CreatedDatefromdate = Convert.ToDateTime(data[0]);
                            CreatedDatetodate = Convert.ToDateTime(data[1]);
                        }
                        else
                        {
                            columnSearch[1] = null;
                        }
                    }

                    country = _countryRepository.GetAll().OrderByDescending(top => top.Id).ToList();
                    if (columnSearch[0] != null)
                    {
                        country = country.Where(top => (CountryId.Contains(Convert.ToInt32(top.Id)))).ToList();
                    }
                    if (columnSearch[1] != null)
                    {
                        country = country.Where(top => (top.CreatedDate >= CreatedDatefromdate && top.CreatedDate <= CreatedDatetodate)).ToList();
                    }

                    cnt = country.Count();
                    country = country.Skip(param.Start).Take(param.Length);

                    #endregion
                }
                else
                {
                    country = _countryRepository.GetAll().OrderByDescending(top => top.Id).ToList();
                    cnt = country.Count();
                    country = country.Skip(param.Start).Take(param.Length);
                }

                foreach (var item in country)
                {
                    _result.Add(new CountryResult { Id = item.Id, CreatedDate = item.CreatedDate.Value.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate, Name = item.Name, ShortName = item.ShortName, CountryCode = item.CountryCode, Status = item.Status });
                }

                _result = ApplySorting(param, _result);
                //_result = _result.Skip(param.Start).Take(param.Length).ToList();

                DTResult<CountryResult> result = new DTResult<CountryResult>
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

        //Add 28-06-2019
        // Function For Filter/Sorting Country Data
        private static List<CountryResult> ApplySorting(DTParameters param, List<CountryResult> result)
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
                        result = result.OrderBy(top => top.ShortName).ToList();
                    else
                        result = result.OrderByDescending(top => top.ShortName).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CountryCode).ToList();
                    else
                        result = result.OrderByDescending(top => top.CountryCode).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CreatedDateSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.CreatedDateSort).ToList();
                }
            }
            return result;
        }

        public List<CountryResult> FillCountryResult()
        {
            List<CountryResult> _result = new List<CountryResult>();
            var countryResult = _countryRepository.GetAll().OrderByDescending(top => top.Id).ToList();
            foreach (var item in countryResult)
            {
                _result.Add(new CountryResult { Id = item.Id, CreatedDate = item.CreatedDate.Value.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate, Name = item.Name, ShortName = item.ShortName, CountryCode = item.CountryCode, Status = item.Status });
            }
            return _result;
        }
        [Route("AddCountry")]
        public ActionResult AddCountry()
        {
            CountryFormModel _countryModel = new CountryFormModel();

            return View(_countryModel);
        }
        [Route("AddCountry")]
        [HttpPost]
        public ActionResult AddCountry(CountryFormModel countrymodel, HttpPostedFileBase TermAndConditionFileName)
        {
            if (ModelState.IsValid)
            {
                CountryFormModel country = new CountryFormModel();

                //Add 21-02-2019
                var countryExist = _countryRepository.Get(top => top.Name.Equals(countrymodel.Name));
                if (countryExist != null)
                {
                    TempData["Error"] = countrymodel.Name + " Record Exist.";
                    ViewBag.name = countrymodel.Name;
                    return View("AddCountry", countrymodel);
                }

                if (TermAndConditionFileName != null && TermAndConditionFileName.ContentLength > 0)
                {
                    var extension = Path.GetExtension(TermAndConditionFileName.FileName);
                    if (extension != ".pdf")
                    {
                        TempData["Error"] = "Please upload pdf file only.";
                        return View(countrymodel);
                    }
                    string directoryName = Server.MapPath("~/TermAndCondition/");

                    var fileName = DateTime.Now.Ticks.ToString() + extension;
                    string path = Path.Combine(directoryName, fileName);
                    TermAndConditionFileName.SaveAs(path);

                    country.TermAndConditionFileName = fileName;
                }
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                country.UserId = efmvcUser.UserId;
                country.Name = countrymodel.Name;
                country.ShortName = countrymodel.ShortName;
                country.CountryCode = countrymodel.CountryCode;
                country.CreatedDate = DateTime.Now;

                CreateOrUpdateCountryCommand command =
                Mapper.Map<CountryFormModel, CreateOrUpdateCountryCommand>(country);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    //TempData["status"] = "Record added successfully.";
                    TempData["status"] = "Country " + country.Name + " added successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(countrymodel);
        }

        [Route("UpdateCountry")]
        public ActionResult UpdateCountry(int id)
        {
            var country = _countryRepository.Get(top => top.Id == id);
            ViewBag.name = country.Name;
            ViewBag.TermAndCondition = country.TermAndConditionFileName;
            CountryFormModel command =
                Mapper.Map<Country, CountryFormModel>(country);
            return View(command);
        }

        [Route("UpdateCountry")]
        [HttpPost]
        public ActionResult UpdateCountry(CountryFormModel countrymodel, HttpPostedFileBase TermAndConditionFileName)
        {
            if (ModelState.IsValid)
            {
                CountryFormModel country = new CountryFormModel();

                //Add 21-02-2019
                var countryExist = _countryRepository.Get(top => top.Name.Equals(countrymodel.Name) && top.Id != countrymodel.Id);
                if (countryExist != null)
                {
                    TempData["Error"] = countrymodel.Name + " Record Exist.";
                    ViewBag.name = countrymodel.Name;
                    return View("UpdateCountry", countrymodel);
                }

                if (TermAndConditionFileName != null && TermAndConditionFileName.ContentLength > 0)
                {
                    var extension = Path.GetExtension(TermAndConditionFileName.FileName);
                    if (extension != ".pdf")
                    {
                        TempData["Error"] = "Please upload pdf file only.";
                        return View(countrymodel);
                    }
                    string directoryName = Server.MapPath("~/TermAndCondition/");

                    if (!string.IsNullOrEmpty(countrymodel.TermAndConditionFileName))
                    {
                        string deletedpath = Path.Combine(directoryName, countrymodel.TermAndConditionFileName);
                        System.IO.File.Delete(deletedpath);
                    }

                    var fileName = DateTime.Now.Ticks.ToString() + extension;
                    string path = Path.Combine(directoryName, fileName);
                    TermAndConditionFileName.SaveAs(path);

                    country.TermAndConditionFileName = fileName;
                }
                else
                {
                    country.TermAndConditionFileName = countrymodel.TermAndConditionFileName;
                }
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                country.Id = countrymodel.Id;
                country.UserId = efmvcUser.UserId;
                country.Name = countrymodel.Name;
                country.ShortName = countrymodel.ShortName;
                country.CountryCode = countrymodel.CountryCode;
                country.CreatedDate = countrymodel.CreatedDate;


                CreateOrUpdateCountryCommand command =
                    Mapper.Map<CountryFormModel, CreateOrUpdateCountryCommand>(country);

                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    //TempData["status"] = "Record updated successfully.";
                    TempData["status"] = "Country " + country.Name + " updated successfully.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Error"] = "Something went wrong.";
                return View(countrymodel);
            }
            return View(countrymodel);
        }


        [Route("SearchCountry")]
        public ActionResult SearchCountry([Bind(Prefix = "Item2")]SearchClass.CountryFilter _filterCritearea, int[] CountryId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<CountryResult> _result = new List<CountryResult>();
                var finalresult = new List<CountryResult>();
                if (_filterCritearea != null)
                {
                    _result = FillCountryResult();
                    finalresult = getCountryResult(_result, _filterCritearea, CountryId);
                }
                else
                {
                    _result = FillCountryResult();
                    finalresult = getCountryResult(_result, _filterCritearea, CountryId);
                }

                return PartialView("_CountryDetails", finalresult);
            }
            else
            {
                return PartialView("_CountryDetails", "notauthorise");
            }
        }
        public List<CountryResult> getCountryResult(List<CountryResult> countryresult, SearchClass.CountryFilter _filterCritearea, int[] CountryId)
        {
            if (countryresult != null && _filterCritearea != null)
            {

                if (CountryId != null)
                {
                    countryresult = countryresult.Where(top => CountryId.Contains(top.Id)).ToList();
                }
                if ((_filterCritearea.Fromdate != null && _filterCritearea.Todate != null))
                {
                    // countryresult = countryresult.Where(top => top.CreatedDate.Value.Date >= _filterCritearea.Fromdate.Value.Date && top.CreatedDate.Value.Date <= _filterCritearea.Todate.Value.Date).ToList();

                    string strTodate = _filterCritearea.Todate;
                    DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strFromdate = _filterCritearea.Fromdate;
                    DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    countryresult = countryresult.Where(top => top.CreatedDateSort.Value.Date >= Fromdate && top.CreatedDateSort.Value.Date <= Todate).ToList();
                }

            }
            else
            {
                if (CountryId != null)
                {
                    countryresult = countryresult.Where(top => CountryId.Contains(top.Id)).ToList();
                }
            }
            return countryresult;
        }
    }
}