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
    [RoutePrefix("CopyRight")]
    public class CopyRightController : Controller
    {
        /// <summary>
        /// The _copyRight Repository
        /// </summary>
        private readonly ICopyRightRepository _copyRightRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;
        public CopyRightController(ICommandBus commandBus, ICopyRightRepository copyRightRepository)
        {
            _commandBus = commandBus;
            _copyRightRepository = copyRightRepository;
        }

        // GET: Admin/CopyRight
        [Route("Index")]
        public ActionResult Index()
        {
            List<CopyRightResult> _result = FillCopyRightResult();
            SearchClass.CopyRightFilter _filterCritearea = new SearchClass.CopyRightFilter();
            return View(Tuple.Create(_result, _filterCritearea));
        }

        //Add 26-06-2019
        [Route("LoadData")]
        [HttpPost]
        public JsonResult LoadData(DTParameters param)
        {
            try
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                List<CopyRightResult> _result = new List<CopyRightResult>();
                IEnumerable<CopyRight> copyRight = null;
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
                    string CopyRightText = "";

                    if (!String.IsNullOrEmpty(columnSearch[0]))
                    {
                        if (columnSearch[0] != "null")
                        {
                            CopyRightText = columnSearch[0].ToString();
                        }
                        else
                        {
                            columnSearch[0] = null;
                        }
                    }

                    copyRight = _copyRightRepository.GetAll().OrderByDescending(top => top.CreatedDate);
                    foreach (var item in copyRight)
                    {
                        _result.Add(new CopyRightResult { Id = item.CopyRightId, Text = item.CopyRightText, Status = item.Active, CreatedDate = item.CreatedDate.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate });
                    }

                    if (columnSearch[0] != null)
                    {
                        _result = _result.Where(top => top.Text == CopyRightText).ToList();
                    }

                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();

                    #endregion
                }
                else
                {
                    copyRight = _copyRightRepository.GetAll().OrderByDescending(top => top.CreatedDate);
                    foreach (var item in copyRight)
                    {
                        _result.Add(new CopyRightResult { Id = item.CopyRightId, Text = item.CopyRightText, Status = item.Active, CreatedDate = item.CreatedDate.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate });
                    }
                    cnt = _result.Count();
                    _result = _result.Skip(param.Start).Take(param.Length).ToList();
                }

                _result = ApplySorting(param, _result);
                //_result = _result.Skip(param.Start).Take(param.Length).ToList();

                DTResult<CopyRightResult> result = new DTResult<CopyRightResult>
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
        // Function For Filter/Sorting CopyRight Data
        private static List<CopyRightResult> ApplySorting(DTParameters param, List<CopyRightResult> result)
        {
            if (param.Order != null)
            {
                var paramOrderDetails = param.Order.FirstOrDefault();
                if (paramOrderDetails.Column == 0)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Text).ToList();
                    else
                        result = result.OrderByDescending(top => top.Text).ToList();
                }
                else if (paramOrderDetails.Column == 1)
                {
                    if (paramOrderDetails.Dir == DTOrderDir.ASC)
                        result = result.OrderBy(top => top.Status).ToList();
                    else
                        result = result.OrderByDescending(top => top.Status).ToList();
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

        // Listing CopyRight
        public List<CopyRightResult> FillCopyRightResult()
        {
            List<CopyRightResult> _result = new List<CopyRightResult>();
            var copyRightResult = _copyRightRepository.GetAll().OrderByDescending(top => top.CreatedDate);
            foreach (var item in copyRightResult)
            {
                _result.Add(new CopyRightResult { Id = item.CopyRightId, Text = item.CopyRightText, Status = item.Active, CreatedDate = item.CreatedDate.ToString("dd/MM/yyyy"), CreatedDateSort = item.CreatedDate });
            }
            return _result;
        }

        // Edit CopyRight
        [Route("UpdateCopyRight")]
        public ActionResult UpdateCopyRight(int id)
        {
            var copyRight = _copyRightRepository.Get(top => top.CopyRightId == id);
            CopyRightFormModel command =
                Mapper.Map<CopyRight, CopyRightFormModel>(copyRight);
            return View(command);
        }

        //Update CopyRight
        [Route("UpdateCopyRight")]
        [HttpPost]
        public ActionResult UpdateCopyRight(CopyRightFormModel copyRightFormModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CopyRightFormModel copyRight = new CopyRightFormModel();
                    var copyRightData = _copyRightRepository.GetById(copyRightFormModel.Id);
                    var CopyRightExist = _copyRightRepository.Get(top => top.CopyRightText.Equals(copyRightFormModel.Text) && top.CopyRightId != copyRightFormModel.Id);
                    if (CopyRightExist != null)
                    {
                        TempData["Error"] = copyRightFormModel.Text + " Record Exist.";
                        return View("UpdateCopyRight", copyRightFormModel);
                    }

                    copyRight.Id = copyRightFormModel.Id;
                    copyRight.Text = copyRightFormModel.Text;
                    copyRight.CreatedDate = copyRightData.CreatedDate;
                    copyRight.Active = copyRightFormModel.Active;

                    CreateOrUpdateCopyRightCommand command =
                        Mapper.Map<CopyRightFormModel, CreateOrUpdateCopyRightCommand>(copyRight);

                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        //TempData["status"] = "Record updated successfully.";
                        TempData["status"] = "Copy Right Text updated successfully.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["Error"] = "Something went wrong.";
                    return View(copyRightFormModel);
                }
                return View(copyRightFormModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "fail";
                return View(copyRightFormModel);
            }
        }

        // Search CopyRight
        [Route("SearchCopyRight")]
        public ActionResult SearchCopyRight([Bind(Prefix = "Item2")]SearchClass.CopyRightFilter _filterCritearea)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<CopyRightResult> _result = new List<CopyRightResult>();
                var finalresult = new List<CopyRightResult>();
                if (_filterCritearea != null)
                {
                    _result = FillCopyRightResult();
                    finalresult = getCopyRightResult(_result, _filterCritearea);
                }
                else
                {
                    _result = FillCopyRightResult();
                    finalresult = getCopyRightResult(_result, _filterCritearea);
                }

                return PartialView("_CopyRightDetails", finalresult);
            }
            else
            {
                return PartialView("_CopyRightDetails", "notauthorise");
            }
        }

        // Search CopyRight
        public List<CopyRightResult> getCopyRightResult(List<CopyRightResult> copyRightresult, SearchClass.CopyRightFilter _filterCritearea)
        {
            if (copyRightresult != null && _filterCritearea != null)
            {

                if (_filterCritearea.Text != null)
                {
                    copyRightresult = copyRightresult.Where(top => top.Text.ToLower().Contains(_filterCritearea.Text.ToLower())).ToList();
                }
            }
            return copyRightresult;
        }
    }
}