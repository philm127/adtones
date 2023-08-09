using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using System.Linq;

namespace EFMVC.Domain.Handlers
{
    public class DeleteCopyRightHandler : ICommandHandler<DeleteCopyRightCommand>
    {
        /// <summary>
        /// The _copyRight Repository
        /// </summary>
        private readonly ICopyRightRepository _copyRightRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCopyRightHandler(ICopyRightRepository copyRightRepository, IUnitOfWork unitOfWork)
        {
            _copyRightRepository = copyRightRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(DeleteCopyRightCommand command)
        {
            Model.Entities.CopyRight copyRightInfo = _copyRightRepository.GetById(command.Id);
            _copyRightRepository.Delete(copyRightInfo);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }
    }
}
