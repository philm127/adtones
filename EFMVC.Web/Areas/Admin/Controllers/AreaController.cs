using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Helpers;
using EFMVC.Data.Repositories;
using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFMVC.Web.ViewModels;
using EFMVC.Domain.Commands;
using EFMVC.Model.Entities;
using EFMVC.Web.Areas.Admin.Models;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("Area")]
    public class AreaController : Controller
    {
        private readonly IAreaRepository _areaRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICommandBus _commandBus;

        public AreaController(ICommandBus commandBus, IAreaRepository areaRepository, ICountryRepository countryRepository)
        {
            _commandBus = commandBus;
            _areaRepository = areaRepository;
            _countryRepository = countryRepository;
        }
        
        [Route("Index")]
        public ActionResult Index()
        {
            List<AreaResult> _result = FillAreaResult();
            SearchClass.AreaFilter _filterCritearea = new SearchClass.AreaFilter();
            FillCountry();
            return View(Tuple.Create(_result, _filterCritearea));
            //var GetAll = _areaRepository.GetMany(s=>s.IsActive == true).ToList();
            //return View(GetAll);
        }

        // Listing Area
        public List<AreaResult> FillAreaResult()
        {
            List<AreaResult> _result = new List<AreaResult>();
            var areaResult = _areaRepository.GetMany(top => top.IsActive == true).ToList().OrderByDescending(a => a.AreaId);
            foreach (var item in areaResult)
            {
                _result.Add(new AreaResult { AreaId = item.AreaId, Name = item.AreaName, CountryId = item.CountryId, CountryName = item.Country.Name });
            }
            return _result;
        }

        public void FillCountry()
        {
            var countrydetails = _countryRepository.GetAll().Select(top => new
            {
                Name = top.Name,
                Id = top.Id
            }).ToList();
            ViewBag.countrydetails = new MultiSelectList(countrydetails, "Id", "Name");

        }

        [Route("AddArea")]
        public ActionResult AddArea()
        {
            FillCountry();
            AreaFormModel _areaModel = new AreaFormModel();

            return View(_areaModel);
        }

        [Route("AddArea")]
        [HttpPost]
        public ActionResult AddArea(AreaFormModel areamodel)
        {
            if (ModelState.IsValid)
            {
                //Add 06-02-2019
                var areaExist = _areaRepository.Get(top => top.AreaName.Trim().ToLower().Equals(areamodel.AreaName.Trim().ToLower()) && top.CountryId == areamodel.CountryId);
                if (areaExist != null)
                {
                    FillCountry();
                    TempData["Error"] = areamodel.AreaName + " Record Exists.";
                    return View("AddArea");
                }

                //AreaFormModel areadata = new AreaFormModel();
                //areadata.AreaName = areamodel.AreaName;
                //areadata.CountryId = areamodel.CountryId;
                areamodel.IsActive = true;

                CreateOrUpdateAreaCommand command =
                Mapper.Map<AreaFormModel, CreateOrUpdateAreaCommand>(areamodel);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    //TempData["status"] = "Record added successfully.";
                    TempData["status"] = "Area " + areamodel.AreaName + " added successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(areamodel);
        }


        [Route("UpdateArea")]
        public ActionResult UpdateArea(int id)
        {
            FillCountry();
            var area = _areaRepository.Get(top => top.AreaId == id);
            ViewBag.name = area.AreaName;
            AreaFormModel command =
               Mapper.Map<Area, AreaFormModel>(area);
            return View(command);
        }

        [Route("UpdateArea")]
        [HttpPost]
        public ActionResult UpdateArea(AreaFormModel areamodel)
        {
            if (ModelState.IsValid)
            {
                //Add 06-02-2019
                var areaExist = _areaRepository.Get(top => top.AreaName.Trim().ToLower().Equals(areamodel.AreaName.Trim().ToLower()) && top.AreaId != areamodel.AreaId && top.CountryId == areamodel.CountryId);
                if (areaExist != null)
                {
                    FillCountry();
                    TempData["Error"] = areamodel.AreaName + " Record Exists.";
                    return View("UpdateArea", areamodel);
                }

                //AreaFormModel areas = new AreaFormModel();
                //areas.AreaId = areamodel.AreaId;
                //areas.AreaName = areamodel.AreaName;
                //areas.CountryId = areamodel.CountryId;
                areamodel.IsActive = true;

                CreateOrUpdateAreaCommand command =
              Mapper.Map<AreaFormModel, CreateOrUpdateAreaCommand>(areamodel);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    //TempData["status"] = "Record updated successfully.";
                    TempData["status"] = "Area " + areamodel.AreaName + " updated successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(areamodel);
        }

        [Route("DeleteArea")]
        public JsonResult DeleteArea(int id)
        {
            DeleteAreaCommand command = new DeleteAreaCommand();
            command.AreaId = id;
            ICommandResult result = _commandBus.Submit(command);
            if (result.Success)
            {
                TempData["status"] = "Record deleted successfully.";
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        // Search Area
        [Route("SearchArea")]
        public ActionResult SearchArea([Bind(Prefix = "Item2")]SearchClass.AreaFilter _filterCritearea, int?[] CountryId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<AreaResult> _result = new List<AreaResult>();
                var finalresult = new List<AreaResult>();
                if (_filterCritearea != null)
                {
                    _result = FillAreaResult();
                    finalresult = getAreaResult(_result, _filterCritearea, CountryId);
                }
                else
                {
                    _result = FillAreaResult();
                    finalresult = getAreaResult(_result, _filterCritearea, CountryId);
                }

                return PartialView("_AreaDetails", finalresult);
            }
            else
            {
                return PartialView("_AreaDetails", "notauthorise");
            }
        }

        // Search Area
        public List<AreaResult> getAreaResult(List<AreaResult> arearesult, SearchClass.AreaFilter _filterCritearea, int?[] CountryId)
        {
            if (arearesult != null && _filterCritearea != null)
            {
                if (CountryId != null)
                {
                    arearesult = arearesult.Where(top => CountryId.Contains(top.CountryId)).ToList();
                }
                if (_filterCritearea.Name != null)
                {
                    arearesult = arearesult.Where(top => top.Name.ToLower().Contains(_filterCritearea.Name.ToLower())).ToList();
                }
            }
            else
            {
                if (CountryId != null)
                {
                    arearesult = arearesult.Where(top => CountryId.Contains(top.CountryId)).ToList();
                }
            }
            return arearesult;
        }

    }
}