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
   public class CreateOrUpdateQuestionCommentHandler : ICommandHandler<CreateOrUpdateQuestionCommentCommand>
    {
        /// <summary>
        /// The _questioncomment repository
        /// </summary>
        private readonly IQuestionCommentRepository _questioncommentRepository;
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        public CreateOrUpdateQuestionCommentHandler(IQuestionCommentRepository questioncommentRepository, IUnitOfWork unitOfWork)
        {
            _questioncommentRepository = questioncommentRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(CreateOrUpdateQuestionCommentCommand command)
        {
            if (command.Id == 0)
            {
                var questioncomment = new QuestionComment
                {
                   UserId=command.UserId,
                   QuestionId=command.QuestionId,
                   Title=command.Title,
                   Description=command.Description,
                   ResponseDatetime=command.ResponseDatetime,
                   TicketCode = command.TicketCode
                };

                _questioncommentRepository.Add(questioncomment);
                _unitOfWork.Commit();
                return new CommandResult(true, questioncomment.Id);
            }
            else
            {
                QuestionComment questioncomment = _questioncommentRepository.GetById(command.Id);
                questioncomment.UserId = command.UserId;
                questioncomment.QuestionId = command.QuestionId;
                questioncomment.Title = command.Title;
                questioncomment.Description = command.Description;
                questioncomment.ResponseDatetime = command.ResponseDatetime;
                questioncomment.TicketCode = command.TicketCode;
                _questioncommentRepository.Update(questioncomment);
                _unitOfWork.Commit();
                return new CommandResult(true);

            }



        }
    }
}
