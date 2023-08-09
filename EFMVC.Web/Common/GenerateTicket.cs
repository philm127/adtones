using AutoMapper;
using EFMVC.CommandProcessor.Command;
using EFMVC.CommandProcessor.Dispatcher;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Web.Core.Models;
using EFMVC.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EFMVC.Web.Common
{
    public class GenerateTicket
    {
        private readonly ICommandBus _commandBus;
        private readonly IUserRepository _userRepository;
        public GenerateTicket(ICommandBus commandBus, IUserRepository userRepository)
        {
            _commandBus = commandBus;
            _userRepository = userRepository;
        }

        // Advertiser submit ads
        // Ad transfer issue when advert admin or operator admin live the ads
        // Soap api issue of upload or delete tone when advert admin or operator admin live the ads
        public void CreateAdTicket(int userId, string sub, string msg,int subjectId, int advertId)
        {           
            var userDetail = _userRepository.GetById(userId);
            string subject = sub;
            string message = msg;
            string ticketCode = LiveAgent.CreateTicket(subject, message, userDetail.Email);
           
            QuestionFormModel model = new QuestionFormModel();
                       
            model.UserId = userId;          
            model.QNumber = ticketCode;
            model.SubjectId = subjectId;
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
            if(advertId == 0)
            {
                model.AdvertId = null;
            }
            else
            {
                model.AdvertId = advertId;
            }
            //_model.UserName = userName;
            //model.Email = email;

            CreateOrUpdateQuestionCommand command =
               Mapper.Map<QuestionFormModel, CreateOrUpdateQuestionCommand>(model);
            ICommandResult results = _commandBus.Submit(command);
        }

        public void CreateAdTicketForBilling(int userId, string sub, string msg, int subjectId,int? ClientId, int CampaignProfileId, int PaymentMethodId)
        {
            var userDetail = _userRepository.GetById(userId);
            string subject = sub;
            string message = msg;
            string ticketCode = LiveAgent.CreateTicket(subject, message, userDetail.Email);

            QuestionFormModel model = new QuestionFormModel();

            model.UserId = userId;
            model.QNumber = ticketCode;
            model.SubjectId = subjectId;            
            model.ClientId = ClientId == 0 ? null : ClientId;
            model.CampaignProfileId = CampaignProfileId;
            model.PaymentMethodId = PaymentMethodId;
            model.Title = subject;
            model.Description = message;
            model.CreatedDate = DateTime.Now;
            model.UpdatedDate = DateTime.Now;
            model.LastResponseDateTime = null;
            model.LastResponseDateTimeByUser = null;
            model.Status = (int)QuestionStatus.New;
            model.UpdatedBy = null;           
            model.AdvertId = null;
           
            //_model.UserName = userName;
            //model.Email = email;

            CreateOrUpdateQuestionCommand command =
               Mapper.Map<QuestionFormModel, CreateOrUpdateQuestionCommand>(model);
            ICommandResult results = _commandBus.Submit(command);
        }
    }
}