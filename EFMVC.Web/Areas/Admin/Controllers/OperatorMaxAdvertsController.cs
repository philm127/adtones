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
    [RoutePrefix("OperatorMaxAdverts")]
    public class OperatorMaxAdvertsController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IOperatorRepository _operatorRepository;
        private readonly IOperatorMaxAdvertRepository _operatorMaxAdvertRepository;
        private readonly ICommandBus _commandBus;        
        public OperatorMaxAdvertsController(ICommandBus commandBus, IUserRepository userRepository, IOperatorRepository operatorRepository, IOperatorMaxAdvertRepository operatorMaxAdvertRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _operatorRepository = operatorRepository;
            _operatorMaxAdvertRepository = operatorMaxAdvertRepository;
        }

        // GET: Admin/OperatorMaxAdverts
        [Route("Index")]
        public ActionResult Index()
        {
            List<OperatorMaxAdvertsResult> _result = FillOperatorMaxAdvertResult();
            SearchClass.OperatorMaxAdvertsFilter _filterCritearea = new SearchClass.OperatorMaxAdvertsFilter();
            FillOperator();
            return View(Tuple.Create(_result, _filterCritearea));
        }

        //Listing Operator Max Advert Result
        public List<OperatorMaxAdvertsResult> FillOperatorMaxAdvertResult()
        {
            List<OperatorMaxAdvertsResult> _result = new List<OperatorMaxAdvertsResult>();
            var operatorMaxAdvertsResult = _operatorMaxAdvertRepository.GetAll().ToList().OrderByDescending(a => a.OperatorMaxAdvertId);
            foreach (var item in operatorMaxAdvertsResult)
            {
                _result.Add(new OperatorMaxAdvertsResult { OperatorMaxAdvertId = item.OperatorMaxAdvertId, KeyName = item.KeyName, KeyValue = item.KeyValue, OperatorId = item.OperatorId, OperatorName = item.Operator.OperatorName, Addeddate = item.Addeddate.ToShortDateString() });
            }
            return _result;
        }

        //Add Operator Max Advert
        [Route("AddOperatorMaxAdverts")]
        public ActionResult AddOperatorMaxAdverts()
        {
            FillOperator();
            OperatorMaxAdvertsFormModel _operatorMaxAdvertsModel = new OperatorMaxAdvertsFormModel();  
            return View(_operatorMaxAdvertsModel);
        }

        //Save Operator Max Advert
        [Route("AddOperatorMaxAdverts")]
        [HttpPost]
        public ActionResult AddOperatorMaxAdverts(OperatorMaxAdvertsFormModel operatorMaxAdvertsFormModel)
        {
            if (ModelState.IsValid)
            {
                var operatorMaxAdvertExist = _operatorMaxAdvertRepository.Get(top => top.OperatorId == operatorMaxAdvertsFormModel.OperatorId && top.KeyName.ToLower().Equals(operatorMaxAdvertsFormModel.KeyName.ToLower()));
                if (operatorMaxAdvertExist != null)
                {
                    FillOperator();                    
                    TempData["Error"] = operatorMaxAdvertsFormModel.OperatorName + " Record Exists.";
                    return View("AddOperatorMaxAdverts");
                }
                operatorMaxAdvertsFormModel.Addeddate = DateTime.Now;
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                CreateOrUpdateOperatorMaxAdvertCommand command = Mapper.Map<OperatorMaxAdvertsFormModel, CreateOrUpdateOperatorMaxAdvertCommand>(operatorMaxAdvertsFormModel);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    TempData["status"] = "Operator " + operatorMaxAdvertsFormModel.OperatorName + " max advert is added successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(operatorMaxAdvertsFormModel);
        }

        //Edit Operator Max Advert
        [Route("UpdateOperatorMaxAdverts")]
        public ActionResult UpdateOperatorMaxAdverts(int id)
        {
            FillOperator();
            var operatorMaxAdverts = _operatorMaxAdvertRepository.Get(top => top.OperatorMaxAdvertId == id);
            OperatorMaxAdvertsFormModel command = 
            Mapper.Map<OperatorMaxAdvert, OperatorMaxAdvertsFormModel>(operatorMaxAdverts);
            ViewBag.OperatorName = operatorMaxAdverts.Operator.OperatorName;
            return View(command);
        }

        //Update Operator Max Advert
        [Route("UpdateOperatorMaxAdverts")]
        [HttpPost]
        public ActionResult UpdateOperatorMaxAdverts(OperatorMaxAdvertsFormModel operatorMaxAdvertsFormModel)
        {
            if (ModelState.IsValid)
            {
                var operatorMaxAdvertExist = _operatorMaxAdvertRepository.Get(top => top.OperatorMaxAdvertId != operatorMaxAdvertsFormModel.OperatorMaxAdvertId && top.OperatorId == operatorMaxAdvertsFormModel.OperatorId && top.KeyName.ToLower().Equals(operatorMaxAdvertsFormModel.KeyName.ToLower()));
                if (operatorMaxAdvertExist != null)
                {
                    FillOperator();
                    TempData["Error"] = operatorMaxAdvertsFormModel.OperatorName + " Record Exists.";
                    return View("UpdateOperatorMaxAdverts");
                }
                operatorMaxAdvertsFormModel.Updateddate = DateTime.Now;
                CreateOrUpdateOperatorMaxAdvertCommand command = Mapper.Map<OperatorMaxAdvertsFormModel, CreateOrUpdateOperatorMaxAdvertCommand>(operatorMaxAdvertsFormModel);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    TempData["status"] = "Operator " + operatorMaxAdvertsFormModel.OperatorName + " updated successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(operatorMaxAdvertsFormModel);
        }

        // Search Operator Max Advert
        [Route("SearchOperatorMaxAdvert")]
        public ActionResult SearchOperatorMaxAdvert([Bind(Prefix = "Item2")]SearchClass.OperatorMaxAdvertsFilter _filterCritearea, int?[] OperatorId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<OperatorMaxAdvertsResult> _result = new List<OperatorMaxAdvertsResult>();
                var finalresult = new List<OperatorMaxAdvertsResult>();
                if (_filterCritearea != null)
                {
                    _result = FillOperatorMaxAdvertResult();
                    finalresult = getOperatorMaxAdvertResult(_result, _filterCritearea, OperatorId);
                }
                else
                {
                    _result = FillOperatorMaxAdvertResult();
                    finalresult = getOperatorMaxAdvertResult(_result, _filterCritearea, OperatorId);
                }

                return PartialView("_OperatorMaxAdvertsDetails", finalresult);
            }
            else
            {
                return PartialView("_OperatorMaxAdvertsDetails", "notauthorise");
            }
        }

        // Filter Operator Max Advert
        public List<OperatorMaxAdvertsResult> getOperatorMaxAdvertResult(List<OperatorMaxAdvertsResult> operatormaxadvertresult, SearchClass.OperatorMaxAdvertsFilter _filterCritearea, int?[] OperatorId)
        {
            if (operatormaxadvertresult != null && _filterCritearea != null)
            {
                if (OperatorId != null)
                {
                    operatormaxadvertresult = operatormaxadvertresult.Where(top => OperatorId.Contains(top.OperatorId)).ToList();
                }
                if (_filterCritearea.Key != null)
                {
                    operatormaxadvertresult = operatormaxadvertresult.Where(top => top.KeyName.ToLower().Contains(_filterCritearea.Key.ToLower())).ToList();
                }
                if (_filterCritearea.Value != null)
                {
                    operatormaxadvertresult = operatormaxadvertresult.Where(top => top.KeyValue.ToLower().Contains(_filterCritearea.Value.ToLower())).ToList();
                }
            }
            else
            {
                if (OperatorId != null)
                {
                    operatormaxadvertresult = operatormaxadvertresult.Where(top => OperatorId.Contains(top.OperatorId)).ToList();
                }
            }
            return operatormaxadvertresult;
        }

        //Fill Operator List
        public void FillOperator()
        {
            var operatordetails = _operatorRepository.GetMany(top => top.IsActive == true).Select(top => new
            {
                Name = top.OperatorName,
                Id = top.OperatorId
            }).ToList();
            ViewBag.operatordetails = new MultiSelectList(operatordetails, "Id", "Name");
        }
    }
}