using EFMVC.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFMVC.Web.Common;
using EFMVC.Web.Core.Models;
using EFMVC.Web.Core.Extensions;
using EFMVC.Web.ViewModels;
using EFMVC.Domain.Commands;
using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using System.Data.Entity.Core.Objects;

namespace EFMVC.Web.Controllers
{
    public class AdvertApproveTicketController : Controller
    {       
        // GET: AdvertApproveTicket
        private readonly ICommandBus _commandBus;
        private readonly IUserRepository _userRepository;
        private readonly IAdvertRepository _advertRepository;
        public AdvertApproveTicketController(ICommandBus commandBus, IUserRepository userRepository, IAdvertRepository advertRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
            _advertRepository = advertRepository;
        }
        public ActionResult Index()
        {
            try
            {
                var previousDate = DateTime.Now.Date.AddDays(-1);
                var advertData = _advertRepository.GetMany(s => s.Status == (int)AdvertStatus.Waitingforapproval && EntityFunctions.TruncateTime(s.CreatedDateTime) == EntityFunctions.TruncateTime(previousDate)).ToList();
                foreach (var item in advertData)
                {
                    EFMVCUser efmvcUser = HttpContext.User.GetEFMVCUser();
                    var userDetail = _userRepository.GetById(efmvcUser.UserId);
                    string subject = "Advert Verification";
                    string message = @"There's a " + item.AdvertName + " advert in adtones. You'll be able to find new advert in your advert page. Please review the advert details and approve or reject advert.";
                    string ticketCode = LiveAgent.CreateTicket(subject, message, userDetail.Email);
                    QuestionFormModel model = new QuestionFormModel();

                    model.UserId = efmvcUser.UserId;
                    model.QNumber = ticketCode;
                    model.SubjectId = (int)QuestionSubjectStatus.Adreview;
                    model.ClientId = null;
                    model.CampaignProfileId = null;
                    model.PaymentMethodId = null;
                    model.Title = subject;
                    model.Description = message;
                    model.CreatedDate = DateTime.Now;
                    model.UpdatedDate = DateTime.Now;
                    model.LastResponseDateTime = null;
                    model.LastResponseDateTimeByUser = null;
                    model.Status = (int)QuestionStatus.New;
                    model.UpdatedBy = null;
                    //_model.UserName = userName;
                    //model.Email = email;

                    CreateOrUpdateQuestionCommand command =
                       Mapper.Map<QuestionFormModel, CreateOrUpdateQuestionCommand>(model);
                    ICommandResult results = _commandBus.Submit(command);
                }

                ViewBag.Suceess = "Suceess";
                ViewBag.Time = DateTime.Now;
            }
            catch(Exception ex)
            {
                ViewBag.Suceess = ex.Message;
                ViewBag.Time = DateTime.Now;
            }
            return View();
        }
    }
}