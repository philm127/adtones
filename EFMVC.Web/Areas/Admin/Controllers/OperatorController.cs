using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using EFMVC.Model.Entities;
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

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("Operator")]
    public class OperatorController : Controller
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

        private readonly IOperatorRepository _operatorRepository;
        private readonly ICurrencyRepository _currencyRepository;
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        
        public OperatorController(ICommandBus commandBus, IUserRepository userRepository, ICountryRepository countryRepository, IOperatorRepository operatorRepository, ICurrencyRepository currencyRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _countryRepository = countryRepository;
            _operatorRepository = operatorRepository;
            _currencyRepository = currencyRepository;
        }
        [Route("Index")]
        public ActionResult Index()
        {
            List<OperatorResult> _result = FillOperatorResult();
            SearchClass.OperatorFilter _filterCritearea = new SearchClass.OperatorFilter();
            FillCountry();
           
            return View(Tuple.Create(_result, _filterCritearea));
            //FillCountry();
            //var GetAll = _operatorRepository.GetAll().ToList();
            //return View(GetAll);
        }

        // Listing Operator
        public List<OperatorResult> FillOperatorResult()
        {
            List<OperatorResult> _result = new List<OperatorResult>();
            var operatorResult = _operatorRepository.GetAll().ToList().OrderByDescending(a => a.OperatorId);
            foreach (var item in operatorResult)
            {
                _result.Add(new OperatorResult { OperatorId = item.OperatorId, Name = item.OperatorName, CountryId = item.CountryId, CountryName = item.CountryId == null ? "-" : item.Country.Name, IsActive = item.IsActive == true ? "Yes" : "No", EmailCost = item.EmailCost, SmsCost = item.SmsCost, Currency = item.CurrencyId == null ? "-" : item.Currency.CurrencyCode });
            }
            return _result;
        }

        public void FillCountry()
        {
            var clientdetails = _countryRepository.GetAll().Select(top => new
            {
                Name = top.Name,
                Id = top.Id
            }).ToList();
            ViewBag.countrydetails = new MultiSelectList(clientdetails, "Id", "Name");

        }

        public void FillCurrency()
        {
            var currencyDetails = _currencyRepository.GetAll().Select(top => new
            {
                Name = top.CurrencyCode,
                Id = top.CurrencyId
            }).ToList();
            ViewBag.CurrencyDetails = new MultiSelectList(currencyDetails, "Id", "Name");

        }

        [Route("AddOperator")]
        public ActionResult AddOperator()
        {
            FillCountry();
            FillCurrency();
            OperatorFormModel _operatorModel = new OperatorFormModel();
            _operatorModel.IsActive = true; 
            return View(_operatorModel);
        }
        [Route("AddOperator")]
        [HttpPost]
        public ActionResult AddOperator(OperatorFormModel operatormodel)
        {
            if (ModelState.IsValid)
            {
                //Add 06-02-2019
                var operatorExist = _operatorRepository.Get(top => top.OperatorName.Trim().ToLower().Equals(operatormodel.OperatorName.Trim().ToLower()) && top.CountryId == operatormodel.CountryId);
                if (operatorExist != null)
                {
                    FillCountry();
                    FillCurrency();
                    TempData["Error"] = operatormodel.OperatorName + " Record Exists.";
                    return View("AddOperator");
                }

                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                OperatorFormModel operatordata = new OperatorFormModel();
                operatordata.OperatorName = operatormodel.OperatorName;
                operatordata.CountryId = operatormodel.CountryId;
                operatordata.IsActive = operatormodel.IsActive;
                operatordata.EmailCost = operatormodel.EmailCost;
                operatordata.SmsCost = operatormodel.SmsCost;
                operatordata.CurrencyId = operatormodel.CurrencyId;

                CreateOrUpdateOperatorCommand command =
                Mapper.Map<OperatorFormModel, CreateOrUpdateOperatorCommand>(operatordata);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    //TempData["status"] = "Record added successfully.";
                    TempData["status"] = "Operator " + operatormodel.OperatorName + " added successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(operatormodel);
        }
       

        [Route("UpdateOperator")]
        public ActionResult UpdateOperator(int id)
        {
            FillCountry();
            FillCurrency();
            var Operator = _operatorRepository.Get(top => top.OperatorId == id);
            ViewBag.name = Operator.OperatorName;
            OperatorFormModel command =
               Mapper.Map<Operator, OperatorFormModel>(Operator);
            return View(command);
        }

        [Route("UpdateOperator")]
        [HttpPost]
        public ActionResult UpdateOperator(OperatorFormModel operatormodel)
        {
            if (ModelState.IsValid)
            {
                //Add 06-02-2019
                var operatorExist = _operatorRepository.Get(top => top.OperatorName.Trim().ToLower().Equals(operatormodel.OperatorName.Trim().ToLower()) && top.OperatorId != operatormodel.OperatorId && top.CountryId == operatormodel.CountryId);
                if (operatorExist != null)
                {
                    FillCountry();
                    FillCurrency();
                    TempData["Error"] = operatormodel.OperatorName + " Record Exists.";
                    return View("UpdateOperator", operatormodel);
                }

                OperatorFormModel operators = new OperatorFormModel();
                operators.OperatorId = operatormodel.OperatorId;
                operators.OperatorName = operatormodel.OperatorName;
                operators.CountryId = operatormodel.CountryId;
                operators.IsActive = operatormodel.IsActive;
                operators.EmailCost = operatormodel.EmailCost;
                operators.SmsCost = operatormodel.SmsCost;
                operators.CurrencyId = operatormodel.CurrencyId;
                CreateOrUpdateOperatorCommand command =
                Mapper.Map<OperatorFormModel, CreateOrUpdateOperatorCommand>(operators);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    //TempData["status"] = "Record updated successfully.";
                    TempData["status"] = "Operator " + operatormodel.OperatorName + " updated successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(operatormodel);
        }

       

        [Route("DeleteOperator")]
        public JsonResult DeleteOperator(int id)
        {
            DeleteOperatorCommand command = new DeleteOperatorCommand();
            command.OperatorId = id;
            ICommandResult result = _commandBus.Submit(command);
            if (result.Success)
            {
                TempData["status"] = "Record deleted successfully.";
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false,JsonRequestBehavior.AllowGet);
        }

        // Search Operator
        [Route("SearchOperator")]
        public ActionResult SearchOperator([Bind(Prefix = "Item2")]SearchClass.OperatorFilter _filterCritearea, int?[] CountryId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<OperatorResult> _result = new List<OperatorResult>();
                var finalresult = new List<OperatorResult>();
                if (_filterCritearea != null)
                {
                    _result = FillOperatorResult();
                    finalresult = getOperatorResult(_result, _filterCritearea, CountryId);
                }
                else
                {
                    _result = FillOperatorResult();
                    finalresult = getOperatorResult(_result, _filterCritearea, CountryId);
                }

                return PartialView("_OperatorDetails", finalresult);
            }
            else
            {
                return PartialView("_OperatorDetails", "notauthorise");
            }
        }

        // Search Operator
        public List<OperatorResult> getOperatorResult(List<OperatorResult> operatorresult, SearchClass.OperatorFilter _filterCritearea, int?[] CountryId)
        {
            if (operatorresult != null && _filterCritearea != null)
            {
                if (CountryId != null)
                {
                    operatorresult = operatorresult.Where(top => CountryId.Contains(top.CountryId)).ToList();
                }
                if (_filterCritearea.Name != null)
                {
                    operatorresult = operatorresult.Where(top => top.Name.ToLower().Contains(_filterCritearea.Name.ToLower())).ToList();
                }
            }
            else
            {
                if (CountryId != null)
                {
                    operatorresult = operatorresult.Where(top => CountryId.Contains(top.CountryId)).ToList();
                }
            }
            return operatorresult;
        }

    }
}