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
    public class DeleteQuestionCommentImagesHandler : ICommandHandler<DeleteQuestionCommentImagesCommand>
    {
        /// <summary>
        /// The _questioncomment images repository
        /// </summary>
        private readonly IQuestionCommentImagesRepository _questioncommentImagesRepository;
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateQuestionCommentImagesHandler"/> class.
        /// </summary>
        /// <param name="questioncommentImagesRepository">The questioncomment images repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeleteQuestionCommentImagesHandler(IQuestionCommentImagesRepository questioncommentImagesRepository, IUnitOfWork unitOfWork)
        {
            _questioncommentImagesRepository = questioncommentImagesRepository;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(DeleteQuestionCommentImagesCommand command)
        {
            Model.QuestionCommentImages commentimagesinfo = _questioncommentImagesRepository.GetById(command.Id);
            _questioncommentImagesRepository.Delete(commentimagesinfo);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }
    }
}
