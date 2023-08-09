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
    [RoutePrefix("Reward")]
    public class RewardController : Controller
    {
        // GET: Admin/Reward

        /// <summary>
        /// The _user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The _reward repository
        /// </summary>
        private readonly IRewardRepository _rewardRepository;

        /// <summary>
        /// The _operator repository
        /// </summary>
        private readonly IOperatorRepository _operatorRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        public RewardController(ICommandBus commandBus, IUserRepository userRepository, IRewardRepository rewardRepository, IOperatorRepository operatorRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _rewardRepository = rewardRepository;
            _operatorRepository = operatorRepository;
        }

        [Route("Index")]
        public ActionResult Index()
        {
            //List<RewardResult> _result = FillRewardResult();
            List<RewardResult> _result = new List<RewardResult>();
            SearchClass.RewardFilter _filterCritearea = new SearchClass.RewardFilter();
            FillOperator();
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
                List<RewardResult> _result = new List<RewardResult>();
                IEnumerable<Reward> reward = null;
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
                    int[] OperatorId = new int[cnt];
                    string RewardName = string.Empty;
                    decimal RewardValue = 0.00M;

                    if (!String.IsNullOrEmpty(columnSearch[0]))
                    {
                        if (columnSearch[0] != "null")
                        {
                            RewardName = columnSearch[0].ToString();
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
                            RewardValue = Convert.ToDecimal(columnSearch[1].ToString());
                        }
                        else
                        {
                            columnSearch[1] = null;
                        }
                    }

                    if (!String.IsNullOrEmpty(columnSearch[1]))
                    {
                        if (columnSearch[1] != "null")
                        {
                            OperatorId = columnSearch[1].Split(',').Select(a => (int)Convert.ToInt32(a)).ToArray();
                        }
                        else
                        {
                            columnSearch[1] = null;
                        }
                    }

                    reward = _rewardRepository.GetAll().OrderByDescending(top => top.AddedDate);
                    foreach (var item in reward)
                    {
                        _result.Add(new RewardResult { Id = item.RewardId, Name = item.RewardName, Value = Convert.ToDecimal(item.RewardValue), OperatorId = item.OperatorId, OperatorName = item.Operator.OperatorName, CreatedDate = item.AddedDate.ToString("dd/MM/yyyy"), CreatedDateSort = item.AddedDate });
                    }
                    if (columnSearch[0] != null)
                    {
                        _result = _result.Where(top => top.Name == RewardName).ToList();
                    }
                    if (columnSearch[1] != null)
                    {
                        _result = _result.Where(top => top.Value == RewardValue).ToList();
                    }
                    if (columnSearch[2] != null)
                    {
                        _result = _result.Where(top => (OperatorId.Contains(Convert.ToInt32(top.OperatorId)))).ToList();
                    }

                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();

                    #endregion
                }
                else
                {
                    reward = _rewardRepository.GetAll().OrderByDescending(top => top.AddedDate);
                    foreach (var item in reward)
                    {
                        _result.Add(new RewardResult { Id = item.RewardId, Name = item.RewardName, Value = Convert.ToDecimal(item.RewardValue), OperatorId = item.OperatorId, OperatorName = item.Operator.OperatorName, CreatedDate = item.AddedDate.ToString("dd/MM/yyyy"), CreatedDateSort = item.AddedDate });
                    }
                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();
                }

                _result = ApplySorting(param, _result);
                //_result = _result.Skip(param.Start).Take(param.Length).ToList();

                DTResult<RewardResult> result = new DTResult<RewardResult>
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
        // Function For Filter/Sorting Reward Data
        private static List<RewardResult> ApplySorting(DTParameters param, List<RewardResult> result)
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
                        result = result.OrderBy(top => top.Value).ToList();
                    else
                        result = result.OrderByDescending(top => top.Value).ToList();
                }
                else if (paramOrderDetails.Column == 2)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.OperatorName).ToList();
                    else
                        result = result.OrderByDescending(top => top.OperatorName).ToList();
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

        // Listing Reward
        public List<RewardResult> FillRewardResult()
        {
            List<RewardResult> _result = new List<RewardResult>();
            var rewardResult = _rewardRepository.GetAll().OrderByDescending(top => top.AddedDate);
            foreach (var item in rewardResult)
            {
                _result.Add(new RewardResult { Id = item.RewardId, Name = item.RewardName, Value = Convert.ToDecimal(item.RewardValue), OperatorId = item.OperatorId, OperatorName = item.Operator.OperatorName, CreatedDate = item.AddedDate.ToString("dd/MM/yyyy"), CreatedDateSort = item.AddedDate });
            }
            return _result;
        }

        //Add 21-02-2019
        //Fill Operator
        public void FillOperator()
        {
            var operatordetails = _operatorRepository.GetAll().Select(top => new
            {
                Name = top.OperatorName,
                Id = top.OperatorId
            }).ToList();
            ViewBag.operatordetails = new MultiSelectList(operatordetails, "Id", "Name");

        }

        // Add Reward
        [Route("AddReward")]
        public ActionResult AddReward()
        {
            RewardFormModel _rewardModel = new RewardFormModel();

            //Add 21-02-2019
            FillOperator();

            return View(_rewardModel);
        }

        // Save Reward
        [Route("AddReward")]
        [HttpPost]
        public ActionResult AddReward(RewardFormModel rewardFormModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RewardFormModel reward = new RewardFormModel();

                    var rewardExist = _rewardRepository.Get(top => top.RewardName.Equals(rewardFormModel.Name) && top.OperatorId == rewardFormModel.OperatorId);
                    if (rewardExist != null)
                    {
                        TempData["Error"] = rewardFormModel.Name + " Record Exist.";
                        FillOperator();
                        ViewBag.name = rewardFormModel.Name;
                        return View("AddReward", rewardFormModel);
                    }

                    reward.Name = rewardFormModel.Name;
                    reward.Value = rewardFormModel.Value;
                    reward.OperatorId = rewardFormModel.OperatorId;
                    reward.CreatedDate = DateTime.Now;

                    CreateOrUpdateRewardCommand command =
                    Mapper.Map<RewardFormModel, CreateOrUpdateRewardCommand>(reward);
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        //TempData["status"] = "Record added successfully.";
                        TempData["status"] = "Reward " + rewardFormModel.Name + " added successfully.";
                        return RedirectToAction("Index");
                    }
                }
                return View(rewardFormModel);
            }
            catch(Exception ex)
            {
                TempData["Error"] = "fail";
                return View(rewardFormModel);
            }
        }

        // Edit Reward
        [Route("UpdateReward")]
        public ActionResult UpdateReward(int id)
        {
            var reward = _rewardRepository.Get(top => top.RewardId == id);
            ViewBag.name = reward.RewardName;
            RewardFormModel command =
                Mapper.Map<Reward, RewardFormModel>(reward);
            FillOperator();
            return View(command);
        }

        // Update Reward
        [Route("UpdateReward")]
        [HttpPost]
        public ActionResult UpdateReward(RewardFormModel rewardFormModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    RewardFormModel reward = new RewardFormModel();

                    var rewardExist = _rewardRepository.Get(top => top.RewardName.Equals(rewardFormModel.Name) && top.OperatorId == rewardFormModel.OperatorId && top.RewardId != rewardFormModel.Id);
                    if (rewardExist != null)
                    {
                        TempData["Error"] = rewardFormModel.Name + " Record Exist.";
                        FillOperator();
                        ViewBag.name = rewardFormModel.Name;
                        return View("UpdateReward", rewardFormModel);
                    }

                    reward.Id = rewardFormModel.Id;
                    reward.Name = rewardFormModel.Name;
                    reward.Value = rewardFormModel.Value;
                    reward.OperatorId = rewardFormModel.OperatorId;
                    reward.CreatedDate = rewardFormModel.CreatedDate;

                    CreateOrUpdateRewardCommand command =
                        Mapper.Map<RewardFormModel, CreateOrUpdateRewardCommand>(reward);

                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        //TempData["status"] = "Record updated successfully.";
                        TempData["status"] = "Reward " + rewardFormModel.Name + " updated successfully.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Error"] = "Something went wrong.";
                    return View(rewardFormModel);
                }
                return View(rewardFormModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "fail";
                return View(rewardFormModel);
            }
        }

        // Delete Reward
        [Route("DeleteReward")]
        [HttpPost]
        public ActionResult DeleteReward(string id)
        {
            try
            {
                int rewardId = int.Parse(id);
                //var reward = _rewardRepository.Get(top => top.RewardId == rewardId);
                RewardFormModel model = new RewardFormModel();
                model.Id = rewardId;
                DeleteRewardCommand command =
                    Mapper.Map<RewardFormModel, DeleteRewardCommand>(model);
                ICommandResult result = _commandBus.Submit(command);
                if (result.Success)
                {
                    var finalresult = new List<RewardResult>();
                    finalresult = FillRewardResult();

                    return PartialView("_RewardDetails", finalresult);
                    //return Json("Success");
                }
                return Json("Fail");
            }
            catch (Exception ex)
            {
                return Json("Fail");
            }
        }

        // Search Reward
        [Route("SearchReward")]
        public ActionResult SearchReward([Bind(Prefix = "Item2")]SearchClass.RewardFilter _filterCritearea, int[] OperatorId)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<RewardResult> _result = new List<RewardResult>();
                var finalresult = new List<RewardResult>();
                if (_filterCritearea != null)
                {
                    _result = FillRewardResult();
                    finalresult = getRewardResult(_result, _filterCritearea, OperatorId);
                }
                else
                {
                    _result = FillRewardResult();
                    finalresult = getRewardResult(_result, _filterCritearea, OperatorId);
                }

                return PartialView("_RewardDetails", finalresult);
            }
            else
            {
                return PartialView("_RewardDetails", "notauthorise");
            }
        }

        // Search Reward
        public List<RewardResult> getRewardResult(List<RewardResult> rewardresult, SearchClass.RewardFilter _filterCritearea, int[] OperatorId)
        {
            if (rewardresult != null && _filterCritearea != null)
            {

                if (OperatorId != null)
                {
                    rewardresult = rewardresult.Where(top => OperatorId.Contains(top.OperatorId)).ToList();
                }
                if (_filterCritearea.Name != null)
                {
                    // rewardresult = rewardresult.Where(top => top.Name.ToLower() == _filterCritearea.Name.ToLower()).ToList();
                    rewardresult = rewardresult.Where(top => top.Name.ToLower().Contains(_filterCritearea.Name.ToLower())).ToList();

                }
                if (_filterCritearea.Value != null)
                {
                    // 03-04-2019 
                    // rewardresult = rewardresult.Where(top => top.Value == _filterCritearea.Value).ToList();
                    rewardresult = rewardresult.Where(top => top.Value.ToString().Contains(_filterCritearea.Value)).ToList();
                }

            }
            else
            {
                if (OperatorId != null)
                {
                    rewardresult = rewardresult.Where(top => OperatorId.Contains(top.OperatorId)).ToList();
                }
            }
            return rewardresult;
        }
    }
}