// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 11-15-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 11-15-2013
// ***********************************************************************
// <copyright file="BlockedNumberController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.BlockedNumber;
using EFMVC.Model;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.ViewModels;

/// <summary>
/// The Controllers namespace.
/// </summary>

namespace EFMVC.Web.Controllers
{
    /// <summary>
    /// Class BlockedNumberController.
    /// </summary>
    [CompressResponse]
    [Authorize]
    public class BlockedNumberController : Controller
    {
        /// <summary>
        /// The _blocked number repository
        /// </summary>
        private readonly IBlockedNumberRepository _blockedNumberRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockedNumberController"/> class.
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="blockedNumberRepository">The blocked number repository.</param>
        public BlockedNumberController(ICommandBus commandBus, IBlockedNumberRepository blockedNumberRepository)
        {
            _commandBus = commandBus;
            _blockedNumberRepository = blockedNumberRepository;
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            IEnumerable<BlockedNumber> blockedNumbers =
                _blockedNumberRepository.GetMany(x => x.UserId == efmvcUser.UserId);
            return View(blockedNumbers);
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Create()
        {
            var model = new BlockedNumberFormModel {Active = true};

            return View(model);
        }

//        [HttpGet]
//        public ActionResult Details(int id)
//        {
//            return View();
//        }

        /// <summary>
        /// Edits the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_blockedNumberRepository.Count(x => x.Id == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            BlockedNumber blockedNumber = _blockedNumberRepository.GetById(id);

            BlockedNumberFormModel model = Mapper.Map<BlockedNumber, BlockedNumberFormModel>(blockedNumber);

            return View(model);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Delete(int id)
        {
            EFMVCUser efmvcUser = System.Web.HttpContext.Current.User.GetEFMVCUser();

            if (_blockedNumberRepository.Count(x => x.Id == id && x.UserId == efmvcUser.UserId) == 0)
                return RedirectToAction("Index");

            var command = new DeleteBlockedNumberCommand {Id = id};
            if (_commandBus != null) _commandBus.Submit(command);

//            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
//            IEnumerable<BlockedNumber> blockedNumbers = _blockedNumberRepository.GetMany(x => x.UserId == efmvcUser.UserId);
//
//            return PartialView("_blockedNumberList", blockedNumbers);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Save(BlockedNumberFormModel model)
        {
            if (ModelState.IsValid)
            {
                EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();

                CreateOrUpdateBlockedNumberCommand command =
                    Mapper.Map<BlockedNumberFormModel, CreateOrUpdateBlockedNumberCommand>(model);
                command.UserId = efmvcUser.UserId;

                if (ModelState.IsValid)
                {
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success) return RedirectToAction("Index");
                }
            }
            //if fail
            if (model.Id == 0)
                return View("Create", model);

            return View("Edit", model);
        }
    }
}