using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;

namespace EFMVC.Domain.Handlers
{
    public class DeleteQuestionCommentHandler : ICommandHandler<DeleteQuestionCommentCommand>
    {
        /// <summary>
        /// The _questioncomment repository
        /// </summary>
        private readonly IQuestionCommentRepository _questioncommentRepository;
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        public DeleteQuestionCommentHandler(IQuestionCommentRepository questioncommentRepository, IUnitOfWork unitOfWork)
        {
            _questioncommentRepository = questioncommentRepository;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(DeleteQuestionCommentCommand command)
        {
            Model.QuestionComment commentinfo = _questioncommentRepository.GetById(command.Id);
            _questioncommentRepository.Delete(commentinfo);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }
    }
}
