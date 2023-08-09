using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;

namespace EFMVC.Domain.Handlers
{
    public  class CreateOrUpdateQuestionImagesHandler : ICommandHandler<CreateOrUpdateQuestionImagesCommand>
    {
        private readonly IQuestionImagesRepository _questionImagesRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateOrUpdateQuestionImagesHandler(IQuestionImagesRepository questionImagesRepository, IUnitOfWork unitOfWork)
        {
            _questionImagesRepository = questionImagesRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(CreateOrUpdateQuestionImagesCommand command)
        {
            if (command.Id == 0)
            {
                var questionImages = new QuestionImages
                {
                    QuestionId = command.QuestionId,
                    UploadImage = command.UploadImage,
                };

                _questionImagesRepository.Add(questionImages);
                _unitOfWork.Commit();
                return new CommandResult(true, questionImages.Id);
            }
            else
            {
                QuestionImages questionImages = _questionImagesRepository.GetById(command.Id);
                questionImages.QuestionId = command.QuestionId;
                questionImages.UploadImage = command.UploadImage;
                _questionImagesRepository.Update(questionImages);
                _unitOfWork.Commit();
                return new CommandResult(true);

            }



        }

    }
}
