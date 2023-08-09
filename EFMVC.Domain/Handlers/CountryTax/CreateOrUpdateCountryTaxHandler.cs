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
   public class CreateOrUpdateCountryTaxHandler : ICommandHandler<CreateOrUpdateCountryTaxCommand>
    {

        /// <summary>
        /// The _country tax repository
        /// </summary>
        private readonly ICountryTaxRepository _countryTaxRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        public CreateOrUpdateCountryTaxHandler(ICountryTaxRepository countryTaxRepository, IUnitOfWork unitOfWork)
        {
            _countryTaxRepository = countryTaxRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(CreateOrUpdateCountryTaxCommand command)
        {
            var countryTax = new Model.CountryTax
            {
                Id = command.Id,
                UserId = command.UserId,
                CountryId = command.CountryId,
                TaxPercantage = command.TaxPercantage,
                CreatedDate = command.CreatedDate,
                UpdatedDate = DateTime.Now,
                Status = 1
            };

            if (countryTax.Id == 0)
                _countryTaxRepository.Add(countryTax);
            else
                _countryTaxRepository.Update(countryTax);

            _unitOfWork.Commit();

            return new CommandResult(true);
        }
    }
}
