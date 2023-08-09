using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.BlockedNumber;
using EFMVC.Model;
using EFMVC.Web.Areas.Users.Models;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Areas.UsersAdmin.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "UserAdmin")]
    [RouteArea("UsersAdmin")]
    [RoutePrefix("UserAdminBlockedNumber")]
    public class UserAdminBlockedNumberController : Controller
    {
        // GET: Users/Index

        /// <summary>
        /// The _blocked number repository
        /// </summary>
        private readonly IBlockedNumberRepository _blockedNumberRepository;
        private readonly IUserRepository _userRepository;
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockedNumberController"/> class.
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="blockedNumberRepository">The blocked number repository.</param>
        public UserAdminBlockedNumberController(ICommandBus commandBus, IBlockedNumberRepository blockedNumberRepository, IUserRepository userRepository)
        {
            _commandBus = commandBus;
            _blockedNumberRepository = blockedNumberRepository;
            _userRepository = userRepository;
        }
        [Route("Index")]
        public ActionResult Index()
        {
            if (Session["userId"] != null)
            {
                List<BlockedNumberResult> _result = FillUserBlockedNumberResult();
                SearchClass.BlockedNumberFilter _filterCritearea = new SearchClass.BlockedNumberFilter();
                return View(Tuple.Create(_result, _filterCritearea));
            }
            else
            {
                return RedirectToAction("Index", "Login", new { area = "UserAdmin" });
            }
        }
        [Route("Create")]
        public ActionResult Create()
        {
            var model = new BlockedNumberFormModel { Active = true };

            return View(model);
        }
        [Route("Save")]
        [HttpPost]
        public ActionResult Save(BlockedNumberFormModel model)
        {
            if (ModelState.IsValid)
            {
                //EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                var userId = Convert.ToInt32(Session["userId"]);

                CreateOrUpdateBlockedNumberCommand command =
                    Mapper.Map<BlockedNumberFormModel, CreateOrUpdateBlockedNumberCommand>(model);
                //command.UserId = efmvcUser.UserId;
                command.UserId = userId;
                command.OperatorId = _userRepository.GetById(userId).OperatorId;
                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (model.Id == 0)
                    {
                        TempData["success"] = "Record added successfully.";
                    }
                    else
                    {
                        TempData["success"] = "Record updated successfully.";
                    }
                    if (result.Success) return RedirectToAction("Index");
                }
            }
            //if fail
            if (model.Id == 0)
                return View("Create", model);

            return View("Edit", model);
        }
        [Route("Edit")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            //EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();
            //Add 15-02-2019
            var userId = Convert.ToInt32(Session["userId"]);

            if (_blockedNumberRepository.Count(x => x.Id == id && x.UserId == userId) == 0)
                return RedirectToAction("Index");

            BlockedNumber blockedNumber = _blockedNumberRepository.GetById(id);

            BlockedNumberFormModel model = Mapper.Map<BlockedNumber, BlockedNumberFormModel>(blockedNumber);

            return View(model);
        }
        [Route("Delete")]
        public ActionResult Delete(int id)
        {
            //EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            //Add 15-02-2019
            var userId = Convert.ToInt32(Session["userId"]);

            if (_blockedNumberRepository.Count(x => x.Id == id && x.UserId == userId) == 0)
                return RedirectToAction("Index");

            var command = new DeleteBlockedNumberCommand { Id = id };
            if (_commandBus != null) _commandBus.Submit(command);
            TempData["success"] = "Record deleted successfully.";
            return RedirectToAction("Index");
        }
        public List<BlockedNumberResult> FillUserBlockedNumberResult()
        {
            List<BlockedNumberResult> _blockNumber = new List<BlockedNumberResult>();
            var userId = Convert.ToInt32(Session["userId"]);
            IEnumerable<BlockedNumber> blockedNumbers =
                _blockedNumberRepository.GetMany(x => x.UserId == userId);
            foreach (var item in blockedNumbers)
            {
                _blockNumber.Add(new BlockedNumberResult { Id = item.Id, Active = item.Active, Name = item.Name, TelephoneNumber = item.TelephoneNumber, UserId = item.UserId });
            }
            return _blockNumber;
        }
        [Route("SearchBlockNumber")]
        public ActionResult SearchBlockNumber([Bind(Prefix = "Item2")]SearchClass.BlockedNumberFilter _filterCritearea)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<BlockedNumberResult> _result = new List<BlockedNumberResult>();
                var finalresult = new List<BlockedNumberResult>();
                if (_filterCritearea != null)
                {
                    _result = FillUserBlockedNumberResult();
                    finalresult = getblockNumberResult(_result, _filterCritearea);
                }
                else
                {
                    _result = FillUserBlockedNumberResult();
                    finalresult = getblockNumberResult(_result, _filterCritearea);
                }

                return PartialView("_BlockedNumberDetails", finalresult);
            }
            else
            {
                return PartialView("_BlockedNumberDetails", "notauthorise");
            }
        }
        public List<BlockedNumberResult> getblockNumberResult(List<BlockedNumberResult> blockedresult, SearchClass.BlockedNumberFilter _filterCritearea)
        {
            if (blockedresult != null && _filterCritearea != null)
            {
                if (!String.IsNullOrEmpty(_filterCritearea.Name))
                {
                    blockedresult = blockedresult.Where(top => top.Name.Contains(_filterCritearea.Name)).ToList();

                }
                if (!String.IsNullOrEmpty(_filterCritearea.TelephoneNumber))
                {
                    blockedresult = blockedresult.Where(top => top.TelephoneNumber.Contains(_filterCritearea.TelephoneNumber)).ToList();

                }
            }

            return blockedresult;
        }
    }
}