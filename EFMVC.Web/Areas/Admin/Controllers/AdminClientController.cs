using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Clients;
using EFMVC.Model;
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
    [RoutePrefix("AdminClient")]
    public class AdminClientController : Controller
    {
        // GET: Admin/Client

        //
        // GET: /Client/

        /// The _client repository
        /// </summary>
        private readonly IClientRepository _clientRepository;

        /// <summary>
        /// The _command bus
        /// </summary>
        private readonly ICommandBus _commandBus;

        public AdminClientController(ICommandBus commandBus, IClientRepository clientRepository)
        {
            _commandBus = commandBus;
            _clientRepository = clientRepository;
        }


        [Route("Index")]
        public ActionResult Index()
        {
            return View();
        }
        public void FillStatus()
        {


            IEnumerable<Common.ClientStatus> clientstatusTypes = Enum.GetValues(typeof(Common.ClientStatus))
                                                     .Cast<Common.ClientStatus>();
            var clientStatus = (from action in clientstatusTypes
                                select new SelectListItem
                                {
                                    Text = action.ToString(),
                                    Value = ((int)action).ToString()
                                }).ToList();
            ViewBag.clientStatus = clientStatus;
        }
        [Route("ClientDetails")]
        [Route("{id}")]
        [HttpGet]
        public ActionResult ClientDetails(int? id)
        {
            FillStatus();
            var _clientdetails = _clientRepository.Get(x => x.Id == id);
            if (_clientdetails != null)
            {
                ClientModel client =
                    Mapper.Map<Client, ClientModel>(_clientdetails);
                ViewBag.clientname = client.Name;
                return View(client);
            }
            return View();
        }

        [Route("UpdateClient")]
        [HttpPost]
        public ActionResult UpdateClient(ClientModel _client)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_client.Id == 0)
                        _client.CreatedDate = DateTime.Now;

                    _client.UpdatedDate = DateTime.Now;
                    _client.UserId = _client.UserId;
                    _client.Name = _client.Name.Trim();
                    var companyexists = _clientRepository.Get(top => top.Name.Trim().ToLower() == _client.Name.Trim().ToLower() && top.UserId == _client.UserId && top.Id != _client.Id);
                    if (companyexists != null)
                    {
                        //TempData["Error"] = "Company name already exists.";
                        TempData["Error"] = _client.Name + " already exists.";
                        FillStatus();
                        //return View("UpdateClient", _client);
                        return RedirectToAction("ClientDetails", new { @id = Convert.ToInt32(_client.Id) });
                    }
                    CreateOrUpdateClientCommand command =
                        Mapper.Map<ClientModel, CreateOrUpdateClientCommand>(_client);
                    ICommandResult result = _commandBus.Submit(command);
                    if (result.Success)
                    {
                        //TempData["msgsuccess"] = "Record updated successfully.";
                        TempData["msgsuccess"] = "Client " + _client.Name + " updated successfully.";
                        FillStatus();
                        //return View("UpdateClient", _client);
                        return RedirectToAction("ClientDetails", new { @id = Convert.ToInt32(_client.Id) });
                    }
                }
                //return View(_client);
                return RedirectToAction("ClientDetails", new { @id = Convert.ToInt32(_client.Id) });
            }
            catch(Exception ex)
            {
                return RedirectToAction("ClientDetails", new { @id = Convert.ToInt32(_client.Id) });
            }
        }
    }
}