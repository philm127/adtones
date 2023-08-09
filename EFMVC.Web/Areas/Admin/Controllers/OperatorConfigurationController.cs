using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model.Entities;
using EFMVC.Web.Areas.Admin.Models;
using EFMVC.Web.Core.ActionFilters;
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
    [RoutePrefix("OperatorConfiguration")]
    public class OperatorConfigurationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IOperatorRepository _operatorRepository;
        private readonly IOperatorConfigurationRepository _operatorConfigurationRepository;
        private readonly ICommandBus _commandBus;
        public OperatorConfigurationController(ICommandBus commandBus, IUserRepository userRepository, IOperatorRepository operatorRepository, IOperatorConfigurationRepository operatorConfigurationRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _operatorRepository = operatorRepository;
            _operatorConfigurationRepository = operatorConfigurationRepository;
        }

        // GET: Admin/OperatorConfiguration
        [Route("Index")]
        public ActionResult Index()
        {
            List<OperatorConfigurationResult> _result = FillOperatorMaxAdvertResult();
            FillOperator();
            return View(_result);
        }

        //Listing Operator Configuration Result
        public List<OperatorConfigurationResult> FillOperatorMaxAdvertResult()
        {
            List<OperatorConfigurationResult> _result = new List<OperatorConfigurationResult>();
            var operatorMaxAdvertsResult = _operatorConfigurationRepository.GetAll().ToList().OrderByDescending(a => a.OperatorConfigurationId);
            foreach (var item in operatorMaxAdvertsResult)
            {
                _result.Add(new OperatorConfigurationResult { OperatorConfigurationId = item.OperatorConfigurationId, Days = item.Days, IsActive = item.IsActive, OperatorId = item.OperatorId, OperatorName = item.Operator.OperatorName, Addeddate = item.AddedDate.ToShortDateString() });
            }
            return _result;
        }

        //Add Operator Configuration
        [Route("AddOperatorConfiguration")]
        public ActionResult AddOperatorConfiguration()
        {
            OperatorConfigurationFormModel operatorConfigurationFormModel = new OperatorConfigurationFormModel();
            FillOperator();
            operatorConfigurationFormModel.IsActive = true;
            return View(operatorConfigurationFormModel);
        }

        //Save Operator Configuration
        [Route("AddOperatorConfiguration")]
        [HttpPost]
        public ActionResult AddOperatorConfiguration(OperatorConfigurationFormModel operatorConfigurationFormModel)
        {
            if (ModelState.IsValid)
            {
                var operatorConfigurationExist = _operatorConfigurationRepository.Get(top => top.OperatorId == operatorConfigurationFormModel.OperatorId);
                if (operatorConfigurationExist != null)
                {
                    FillOperator();
                    TempData["Error"] = operatorConfigurationFormModel.OperatorName + " Record Exists.";
                    return View(operatorConfigurationFormModel);
                }
                operatorConfigurationFormModel.Addeddate = DateTime.Now;
                CreateOrUpdateOperatorConfigurationCommand command = Mapper.Map<OperatorConfigurationFormModel, CreateOrUpdateOperatorConfigurationCommand>(operatorConfigurationFormModel);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    TempData["status"] = "Operator " + operatorConfigurationFormModel.OperatorName + " configuration is added successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(operatorConfigurationFormModel);
        }

        //Edit Operator Configuration
        [Route("UpdateOperatorConfiguration")]
        public ActionResult UpdateOperatorConfiguration(int id)
        {
            var operatorConfigurations = _operatorConfigurationRepository.Get(top => top.OperatorConfigurationId == id);
            OperatorConfigurationFormModel command = Mapper.Map<OperatorConfiguration, OperatorConfigurationFormModel>(operatorConfigurations);
            ViewBag.OperatorName = operatorConfigurations.Operator.OperatorName;
            FillOperator();
            return View(command);
        }

        //Update Operator Configuration
        [Route("UpdateOperatorConfiguration")]
        [HttpPost]
        public ActionResult UpdateOperatorConfiguration(OperatorConfigurationFormModel operatorConfigurationFormModel)
        {
            if (ModelState.IsValid)
            {
                operatorConfigurationFormModel.Updateddate = DateTime.Now;
                CreateOrUpdateOperatorConfigurationCommand command = Mapper.Map<OperatorConfigurationFormModel, CreateOrUpdateOperatorConfigurationCommand>(operatorConfigurationFormModel);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    TempData["status"] = "Operator " + operatorConfigurationFormModel.OperatorName + " updated successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(operatorConfigurationFormModel);
        }

        // Search Operator Configuration
        [Route("SearchOperatorConfiguration")]
        public ActionResult SearchOperatorConfiguration(int?[] OperatorId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<OperatorConfigurationResult> _result = new List<OperatorConfigurationResult>();
                var finalresult = new List<OperatorConfigurationResult>();
                _result = FillOperatorMaxAdvertResult();
                finalresult = getOperatorMaxAdvertResult(_result, OperatorId);
                return PartialView("_OperatorConfigurationDetails", finalresult);
            }
            else
            {
                return PartialView("_OperatorConfigurationDetails", "notauthorise");
            }
        }

        // Filter Operator Max Advert
        public List<OperatorConfigurationResult> getOperatorMaxAdvertResult(List<OperatorConfigurationResult> operatorConfigurationResults, int?[] OperatorId)
        {
            if (OperatorId != null)
                operatorConfigurationResults = operatorConfigurationResults.Where(top => OperatorId.Contains(top.OperatorId)).ToList();
            return operatorConfigurationResults;
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