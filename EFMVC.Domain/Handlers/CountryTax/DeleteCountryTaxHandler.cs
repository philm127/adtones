using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers
{
   public class DeleteCountryTaxHandler : ICommandHandler<DeleteCountryTaxCommand>
    {
        /// <summary>
      /// The _country tax repository
      /// </summary>
        private readonly ICountryTaxRepository _countryTaxRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCountryTaxHandler(ICountryTaxRepository countryTaxRepository, IUnitOfWork unitOfWork)
        {
            _countryTaxRepository = countryTaxRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(DeleteCountryTaxCommand command)
        {
            Model.CountryTax countryTaxInfo = _countryTaxRepository.GetById(command.Id);
            _countryTaxRepository.Delete(countryTaxInfo);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }
    }
}
