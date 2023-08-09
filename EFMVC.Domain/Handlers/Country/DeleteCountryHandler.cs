using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;

namespace EFMVC.Domain.Handlers
{
    public class DeleteCountryHandler : ICommandHandler<DeleteCountryCommand>
    {
        /// <summary>
        /// The _country repository
        /// </summary>
        private readonly ICountryRepository _countryRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCountryHandler(ICountryRepository countryRepository, IUnitOfWork unitOfWork)
        {
            _countryRepository = countryRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(DeleteCountryCommand command)
        {
            Model.Country countryInfo = _countryRepository.GetById(command.Id);
            _countryRepository.Delete(countryInfo);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }
    }
}
