using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;

namespace EFMVC.Domain.Handlers
{
    public class CreateOrUpdateQuestionHandler : ICommandHandler<CreateOrUpdateQuestionCommand>
    {
        /// <summary>
        /// The _question repository
        /// </summary>
        private readonly IQuestionRepository _questionRepository;
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        public CreateOrUpdateQuestionHandler(IQuestionRepository questionRepository, IUnitOfWork unitOfWork)
        {
            _questionRepository = questionRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(CreateOrUpdateQuestionCommand command)
        {
            if (command.Id == 0)
            {
                var question = new Question
                {
                    UserId = command.UserId,
                    ClientId = command.ClientId,
                    CampaignProfileId = command.CampaignProfileId,
                    SubjectId = command.SubjectId,
                    QNumber = command.QNumber,
                    Title = command.Title,
                    Description = command.Description,
                    CreatedDate = command.CreatedDate,
                    UpdatedDate = command.UpdatedDate,
                    LastResponseDateTime = command.LastResponseDateTime,
                    LastResponseDateTimeByUser = command.LastResponseDateTimeByUser,
                    PaymentMethodId = command.PaymentMethodId,
                    Status = command.Status,
                    UpdatedBy = command.UpdatedBy,
                    UserName = command.UserName,
                    Email = command.Email,
                    AdvertId = command.AdvertId
                };

                _questionRepository.Add(question);
                _unitOfWork.Commit();
                return new CommandResult(true, question.Id);
            }
            else
            {
                Question question = _questionRepository.GetById(command.Id);
                question.UserId = command.UserId;
                question.ClientId = command.ClientId;
                question.CampaignProfileId = command.CampaignProfileId;
                question.SubjectId = command.SubjectId;
                question.QNumber = command.QNumber;
                question.Title = command.Title;
                question.Description = command.Description;
                question.CreatedDate = command.CreatedDate;
                question.UpdatedDate = command.UpdatedDate;
                question.LastResponseDateTime = command.LastResponseDateTime;
                question.LastResponseDateTimeByUser = command.LastResponseDateTimeByUser;
                question.PaymentMethodId = command.PaymentMethodId;
                question.Status = command.Status;
                question.UpdatedBy = command.UpdatedBy;
                question.UserName = command.UserName;
                question.Email = command.Email;
                question.AdvertId = command.AdvertId;
                _questionRepository.Update(question);
                _unitOfWork.Commit();
                return new CommandResult(true);

            }



        }
    }
}
