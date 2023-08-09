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
   public class CreateOrUpdateQuestionCommentImagesHandler : ICommandHandler<CreateOrUpdateQuestionCommentImagesCommand>
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
        public CreateOrUpdateQuestionCommentImagesHandler(IQuestionCommentImagesRepository questioncommentImagesRepository, IUnitOfWork unitOfWork)
        {
            _questioncommentImagesRepository = questioncommentImagesRepository;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>
        /// ICommandResult.
        /// </returns>
        public ICommandResult Execute(CreateOrUpdateQuestionCommentImagesCommand command)
        {
            if (command.Id == 0)
            {
                var questioncommentImages = new QuestionCommentImages
                {
                    QuestionCommentId = command.QuestionCommentId,
                    UploadImages = command.UploadImages,
                };

                _questioncommentImagesRepository.Add(questioncommentImages);
                _unitOfWork.Commit();
                return new CommandResult(true, questioncommentImages.Id);
            }
            else
            {
                QuestionCommentImages questioncomment = _questioncommentImagesRepository.GetById(command.Id);
                questioncomment.QuestionCommentId = command.QuestionCommentId;
                questioncomment.UploadImages = command.UploadImages;
                _questioncommentImagesRepository.Update(questioncomment);
                _unitOfWork.Commit();
                return new CommandResult(true);

            }



        }
    }
}
