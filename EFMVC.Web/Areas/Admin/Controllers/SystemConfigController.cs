using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using EFMVC.Web.Areas.Admin.Models;
using EFMVC.Web.Areas.Admin.ViewModel;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Helpers;
using EFMVC.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.Admin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Admin")]
    [AdminRequired]
    [RouteArea("Admin")]
    [RoutePrefix("SystemConfig")]
    public class SystemConfigController : Controller
    {
        // GET: Admin/SystemConfig

        //
        // GET: /Admin/UserCredit/

        // GET: /AdminQuestion/


        /// <summary>
        /// The _system configuration repository
        /// </summary>
        private readonly ISystemConfigRepository _systemConfigRepository;


        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        public SystemConfigController(ICommandBus commandBus, ISystemConfigRepository systemConfigRepository)
        {
            _commandBus = commandBus;
            _systemConfigRepository = systemConfigRepository;
        }
        [Route("Index")]
        public ActionResult Index()
        {
            List<SystemConfigResult> _result = FillSytemConfigResult();
            SearchClass.SystemConfigFilter _filterCritearea = new SearchClass.SystemConfigFilter();
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
                List<SystemConfigResult> _result = new List<SystemConfigResult>();
                IEnumerable<SystemConfig> systemConfig = null;
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
                    string SystemConfigKey = string.Empty;
                    string SystemConfigValue = string.Empty;
                    DateTime CreatedDatefromdate = new DateTime();
                    DateTime CreatedDatetodate = new DateTime();

                    if (!String.IsNullOrEmpty(columnSearch[0]))
                    {
                        if (columnSearch[0] != "null")
                        {
                            SystemConfigKey = columnSearch[0].ToString();
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
                            SystemConfigValue = columnSearch[1].ToString();
                        }
                        else
                        {
                            columnSearch[1] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[2]))
                    {
                        if (columnSearch[2] != "null")
                        {
                            var data = columnSearch[2].Split(',').ToArray();
                            CreatedDatefromdate = Convert.ToDateTime(data[0]);
                            CreatedDatetodate = Convert.ToDateTime(data[1]);
                        }
                        else
                        {
                            columnSearch[2] = null;
                        }
                    }

                    systemConfig = _systemConfigRepository.GetAll().OrderByDescending(top => top.CreatedDateTime);
                    foreach (var item in systemConfig)
                    {
                        _result.Add(new SystemConfigResult { SystemConfigId = item.SystemConfigId, CreatedDateTime = item.CreatedDateTime.ToString("dd/MM/yyyy"), CreatedDateTimeSort = item.CreatedDateTime, SystemConfigKey = item.SystemConfigKey, SystemConfigValue = item.SystemConfigValue, SystemConfigType = item.SystemConfigType == null ? "-" : item.SystemConfigType });
                    }
                    if (columnSearch[0] != null)
                    {
                        _result = _result.Where(top => top.SystemConfigKey == SystemConfigKey).ToList();
                    }
                    if (columnSearch[1] != null)
                    {
                        _result = _result.Where(top => top.SystemConfigValue == SystemConfigValue).ToList();
                    }
                    if (columnSearch[2] != null)
                    {
                        _result = _result.Where(top => (top.CreatedDateTimeSort >= CreatedDatefromdate && top.CreatedDateTimeSort <= CreatedDatetodate)).ToList();
                    }

                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();

                    #endregion
                }
                else
                {
                    systemConfig = _systemConfigRepository.GetAll().OrderByDescending(top => top.CreatedDateTime);
                    foreach (var item in systemConfig)
                    {
                        _result.Add(new SystemConfigResult { SystemConfigId = item.SystemConfigId, CreatedDateTime = item.CreatedDateTime.ToString("dd/MM/yyyy"), CreatedDateTimeSort = item.CreatedDateTime, SystemConfigKey = item.SystemConfigKey, SystemConfigValue = item.SystemConfigValue, SystemConfigType = item.SystemConfigType == null ? "-" : item.SystemConfigType });
                    }
                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();
                }

                _result = ApplySorting(param, _result);
                //_result = _result.Skip(param.Start).Take(param.Length).ToList();

                DTResult<SystemConfigResult> result = new DTResult<SystemConfigResult>
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
        // Function For Filter/Sorting SystemConfig Data
        private static List<SystemConfigResult> ApplySorting(DTParameters param, List<SystemConfigResult> result)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.SystemConfigKey).ToList();
                    else
                        result = result.OrderByDescending(top => top.SystemConfigKey).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.SystemConfigValue).ToList();
                    else
                        result = result.OrderByDescending(top => top.SystemConfigValue).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.CreatedDateTimeSort).ToList();
                    else
                        result = result.OrderByDescending(top => top.CreatedDateTimeSort).ToList();
                }
                else if (paramOrderDetails.Column == 3)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.SystemConfigType).ToList();
                    else
                        result = result.OrderByDescending(top => top.SystemConfigType).ToList();
                }
            }
            return result;
        }

        [Route("Add")]
        public ActionResult Add()
        {
            SystemConfigFormModel _systemconfig = new SystemConfigFormModel();
            return View(_systemconfig);
        }
        [Route("Save")]
        [HttpPost]
        public ActionResult Save(SystemConfigFormModel _model)
        {
            if (ModelState.IsValid)
            {
                //Add 06-02-2019
                var systemConfigExist = _systemConfigRepository.Get(top => top.SystemConfigKey.Trim().ToLower().Equals(_model.SystemConfigKey.Trim().ToLower()) && top.SystemConfigType.ToLower().Equals(_model.SystemConfigType.ToLower()));
                if (systemConfigExist != null)
                {
                    TempData["Error"] = _model.SystemConfigKey + " Record Exists.";
                    return View("Add", _model);
                }

                SystemConfigFormModel _systemconfig = new SystemConfigFormModel();
                _systemconfig.SystemConfigKey = _model.SystemConfigKey;
                _systemconfig.SystemConfigValue = _model.SystemConfigValue;
                _systemconfig.SystemConfigType = _model.SystemConfigType;
                _systemconfig.CreatedDateTime = DateTime.Now;
                _systemconfig.UpdatedDateTime = DateTime.Now;
                CreateOrUpdateSystemConfigCommand command =
                Mapper.Map<SystemConfigFormModel, CreateOrUpdateSystemConfigCommand>(_systemconfig);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    //TempData["success"] = "Record inserted successfully.";
                    TempData["success"] = "System Config " + _model.SystemConfigKey + " added successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(_model);


        }
        [HttpGet]
        [Route("ConfigDetails")]

        public ActionResult ConfigDetails(int id)
        {
            SystemConfigFormModel _systemconfig = new SystemConfigFormModel();
            var result = _systemConfigRepository.Get(top => top.SystemConfigId == id);
            if (result == null)
            {
                return RedirectToAction("Index");
            }
            _systemconfig.SystemConfigId = result.SystemConfigId;
            _systemconfig.SystemConfigKey = result.SystemConfigKey;
            _systemconfig.SystemConfigValue = result.SystemConfigValue;
            _systemconfig.SystemConfigType = result.SystemConfigType;
            _systemconfig.CreatedDateTime = result.CreatedDateTime;
            _systemconfig.UpdatedDateTime = result.UpdatedDateTime;
            return View(_systemconfig);
        }
        [Route("Update")]
        public ActionResult Update(SystemConfigFormModel _model)
        {
            if (ModelState.IsValid)
            {
                //Add 06-02-2019
                var systemConfigExist = _systemConfigRepository.Get(top => top.SystemConfigKey.Trim().ToLower().Equals(_model.SystemConfigKey.Trim().ToLower()) && top.SystemConfigId != _model.SystemConfigId && top.SystemConfigType.ToLower().Equals(_model.SystemConfigType.ToLower()));
                if (systemConfigExist != null)
                {
                    TempData["Error"] = _model.SystemConfigKey + " Record Exists.";
                    return View("ConfigDetails", _model);
                }

                SystemConfigFormModel _systemconfig = new SystemConfigFormModel();
                _systemconfig.SystemConfigId = _model.SystemConfigId;
                _systemconfig.SystemConfigKey = _model.SystemConfigKey;
                _systemconfig.SystemConfigValue = _model.SystemConfigValue;
                _systemconfig.SystemConfigType = _model.SystemConfigType;
                _systemconfig.CreatedDateTime = _model.CreatedDateTime;
                _systemconfig.UpdatedDateTime = DateTime.Now;
                CreateOrUpdateSystemConfigCommand command =
                Mapper.Map<SystemConfigFormModel, CreateOrUpdateSystemConfigCommand>(_systemconfig);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    //TempData["success"] = "Record updated successfully.";
                    TempData["success"] = "System Config " + _model.SystemConfigKey + " updated successfully.";
                    return RedirectToAction("Index");
                }
            }
            return View(_model);


        }
        [Route("Delete")]
        public ActionResult Delete(int id)
        {
            DeleteSystemConfigCommand command = new DeleteSystemConfigCommand();
            command.Id = id;
            ICommandResult result = _commandBus.Submit(command);
            if (result.Success)
            {
                TempData["success"] = "Record deleted successfully.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public List<SystemConfigResult> FillSytemConfigResult()
        {
            List<SystemConfigResult> _systemConfigResult = new List<SystemConfigResult>();

            var result = _systemConfigRepository.GetAll().OrderByDescending(top => top.CreatedDateTime);
            foreach (var item in result)
            {
                _systemConfigResult.Add(new SystemConfigResult { SystemConfigId = item.SystemConfigId, CreatedDateTime = item.CreatedDateTime.ToString("dd/MM/yyyy"), CreatedDateTimeSort = item.CreatedDateTime, SystemConfigKey = item.SystemConfigKey, SystemConfigValue = item.SystemConfigValue, SystemConfigType = item.SystemConfigType == null ? "-" : item.SystemConfigType });
            }

            return _systemConfigResult;
        }
        [Route("SearchSystemConfig")]
        public ActionResult SearchSystemConfig([Bind(Prefix = "Item2")]SearchClass.SystemConfigFilter _filterCritearea, string[] SystemConfigType)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<SystemConfigResult> _result = new List<SystemConfigResult>();
                var finalresult = new List<SystemConfigResult>();
                if (_filterCritearea != null)
                {
                    _result = FillSytemConfigResult();
                    finalresult = getsystemconfigResult(_result, _filterCritearea, SystemConfigType);
                }
                else
                {
                    _result = FillSytemConfigResult();
                    finalresult = getsystemconfigResult(_result, _filterCritearea, SystemConfigType);
                }

                return PartialView("_SystemConfigDetails", finalresult);
            }
            else
            {
                return PartialView("_SystemConfigDetails", "notauthorise");
            }
        }
        public List<SystemConfigResult> getsystemconfigResult(List<SystemConfigResult> systemconfigresult, SearchClass.SystemConfigFilter _filterCritearea, string[] SystemConfigType)
        {
            if (systemconfigresult != null && _filterCritearea != null)
            {
                if (!String.IsNullOrEmpty(_filterCritearea.SystemConfigKey))
                {
                    systemconfigresult = systemconfigresult.Where(top => top.SystemConfigKey.ToLower().Contains(_filterCritearea.SystemConfigKey.ToLower())).ToList();

                }
                if (!String.IsNullOrEmpty(_filterCritearea.SystemConfigValue))
                {
                    systemconfigresult = systemconfigresult.Where(top => top.SystemConfigValue.ToLower().Contains(_filterCritearea.SystemConfigValue.ToLower())).ToList();

                }
                if (SystemConfigType != null)
                {
                    systemconfigresult = systemconfigresult.Where(top => top.SystemConfigType.ToLower().Contains(_filterCritearea.SystemConfigType.ToLower())).ToList();
                }
                if ((_filterCritearea.Fromdate != null && _filterCritearea.Todate != null))
                {
                    string strTodate = _filterCritearea.Todate;
                    DateTime Todate = DateTime.ParseExact(strTodate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    string strFromdate = _filterCritearea.Fromdate;
                    DateTime Fromdate = DateTime.ParseExact(strFromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    systemconfigresult = systemconfigresult.Where(top => top.CreatedDateTimeSort.Date >= Fromdate && top.CreatedDateTimeSort.Date <= Todate).ToList();
                    // systemconfigresult = systemconfigresult.Where(top => top.CreatedDateTime.Date >= _filterCritearea.Fromdate.Value.Date && top.CreatedDateTime.Date <= _filterCritearea.Todate.Value.Date).ToList();
                }

            }
            else
            {
                if (SystemConfigType != null)
                {
                    systemconfigresult = systemconfigresult.Where(top => top.SystemConfigType.ToLower().Contains(_filterCritearea.SystemConfigType.ToLower())).ToList();
                }
            }
            return systemconfigresult;
        }
        
    }
}