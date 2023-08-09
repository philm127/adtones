using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Contacts;
using EFMVC.Model;
using EFMVC.Model.Entities;
using EFMVC.Web.Core.ActionFilters;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.Core.Models;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EFMVC.Web.Controllers
{
    [CompressResponse]
    [Authorize(Roles = "Advertiser")]

    public class CurrencyController : Controller
    {
        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// The contacts repository
        /// </summary>
        private readonly IContactsRepository _contactsRepository;

        /// <summary>
        /// The country repository
        /// </summary>
        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// The currency repository
        /// </summary>
        private readonly ICurrencyRepository _currencyRepository;
        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileController"/> class.
        /// </summary>
        /// <param name="commandBus">The command bus.</param>
        /// <param name="contactsRepository">The contacts repository.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="countryRepository">The country repository.</param>
        /// /// <param name="currencyRepository">The currency repository.</param>
        public CurrencyController(ICommandBus commandBus, IContactsRepository contactsRepository, IUserRepository userRepository, ICountryRepository countryRepository, ICurrencyRepository currencyRepository)
        {
            _commandBus = commandBus;
            _contactsRepository = contactsRepository;
            _userRepository = userRepository;
            _countryRepository = countryRepository;
            _currencyRepository = currencyRepository;
        }

        private void FillCountry(CurrencyFormModel currencyFormModel)
        {
            //var currencyData = _currencyRepository.GetAll().OrderBy(top => top.CurrencyCode);
            var currencyData = _currencyRepository.GetAll().OrderBy(c => c.CurrencyCode).Skip(1);
            var currencyResult = Mapper.Map<IEnumerable<Currency>, IEnumerable<CurrencyFormModel>>(currencyData);
            var currencyList = currencyResult.Select(s => new SelectListItem { Text = s.CurrencyCode, Value = s.CurrencyId.ToString() }).ToList();
            currencyList.Insert(0, new SelectListItem { Text = "--Select Country--", Value = "0" });
            ViewBag.currencyList = currencyList;
        }

        // GET: Currency
        public ActionResult Index()
        {
            CurrencyFormModel currencyFormModel = new CurrencyFormModel();
            FillCountry(currencyFormModel);
            EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
            var contactinfo = _contactsRepository.GetMany(top => top.UserId == efmvcUser.UserId).FirstOrDefault();
            if (contactinfo != null)
            {
                if (contactinfo.CurrencyId == null)
                {
                    var currencyId = _currencyRepository.GetAll().Where(top => top.CountryId == contactinfo.CountryId).Select(top => top.CurrencyId).FirstOrDefault();
                    //var CurrencyCode = _currencyRepository.GetAll().Where(top => top.CountryId == contactinfo.CountryId).Select(top => top.CurrencyCode).FirstOrDefault();
                    if (currencyId != 0)
                    {
                        currencyFormModel.CurrencyId = currencyId;
                        //currencyFormModel.CurrencyCode = CurrencyCode;
                    }
                    else
                    {
                        //var currency = _currencyRepository.Get(top => top.CurrencyCode.Contains("USD"));
                        currencyFormModel.CurrencyId = currencyId;
                        //currencyFormModel.CurrencyCode = currency.CurrencyCode;
                    }
                }
                else
                {
                    if (contactinfo.CurrencyId == 2)
                    {
                        currencyFormModel.CurrencyId = 5;
                    }
                    else
                    {
                        currencyFormModel.CurrencyId = Convert.ToInt32(contactinfo.CurrencyId);
                    }
                    //currencyFormModel.CurrencyCode =contactinfo.Currency.CurrencyCode;
                }
            }
            else
            {
                var currencyId = _currencyRepository.GetAll().Where(top => top.CountryId == 8).Select(top => top.CurrencyId).FirstOrDefault();
                currencyFormModel.CurrencyId = Convert.ToInt32(currencyId);
                //var CurrencyCode = _currencyRepository.GetAll().Where(top => top.CountryId == 8).Select(top => top.CurrencyCode).FirstOrDefault();
                //currencyFormModel.CurrencyCode = CurrencyCode;
            }
            return View(currencyFormModel);
        }

        [HttpPost]
        public ActionResult SaveCurrency(CurrencyFormModel currencyFormModel)
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (ModelState.IsValid)
                    {
                        EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                        ContactsFormModel model = new ContactsFormModel();

                        var contactinfo = _contactsRepository.GetMany(top => top.UserId == efmvcUser.UserId).FirstOrDefault();

                        ChangeContactsCurrencyCommand command = new ChangeContactsCurrencyCommand();
                        command.Id = contactinfo.Id;
                        if (currencyFormModel.CurrencyId != 0)
                        {
                            command.CurrencyId = currencyFormModel.CurrencyId;
                        }
                        else
                        {
                            command.CurrencyId = 3;
                        }
                        ICommandResult result = _commandBus.Submit(command);
                        if (result.Success)
                        {
                            return Json("success");
                        }
                    }
                    return Json("fail");
                }
                else
                {
                    return Json("notauthorise");
                }
            }
            catch (Exception ex)
            {
                return Json(ex.InnerException.Message);
            }

        }
    }
}