using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.CurrencyRate;
using EFMVC.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers.CurrencyRate
{
    public class CreateOrUpdateCurrencyRateHandler : ICommandHandler<CreateOrUpdateCurrencyRateCommand>
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The currency rate repository
        /// </summary>
        private readonly ICurrencyRateRepository _currencyRateRepository;

        public CreateOrUpdateCurrencyRateHandler(ICurrencyRateRepository currencyRateRepository, IUnitOfWork unitOfWork)
        {
            this._currencyRateRepository = currencyRateRepository;
            this.unitOfWork = unitOfWork;
        }

        public ICommandResult Execute(CreateOrUpdateCurrencyRateCommand command)
        {
            Model.Entities.CurrencyRate currencyRate = _currencyRateRepository.Get(c => c.CurrencyCode == command.CurrencyCode);
            currencyRate.CurrencyRateAmount = command.CurrencyRateAmount;
            currencyRate.UpdatedDate = System.DateTime.Now;
            _currencyRateRepository.Update(currencyRate);
            unitOfWork.Commit();
            return new CommandResult(true);
        }
    }
}
