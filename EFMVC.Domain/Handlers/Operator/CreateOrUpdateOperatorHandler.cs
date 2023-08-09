using EFMVC.CommandProcessor.Command;
using EFMVC.Data;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.CountryConnectionString;
using EFMVC.Domain.OperatorServerData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers
{
    public class CreateOrUpdateOperatorHandler : ICommandHandler<CreateOrUpdateOperatorCommand>
    {

        /// <summary>
        /// The _country repository
        /// </summary>
        private readonly IOperatorRepository _operatorRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateOperatorHandler"/> class.
        /// </summary>
        /// <param name="operatorRepository">The country repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateOperatorHandler(IOperatorRepository operatorRepository, IUnitOfWork unitOfWork)
        {
            _operatorRepository = operatorRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(CreateOrUpdateOperatorCommand command)
        {
            var operators = new Model.Entities.Operator
            {
                OperatorId = command.OperatorId,
                OperatorName = command.OperatorName,   
                CountryId = command.CountryId,
                IsActive = command.IsActive,
                EmailCost = command.EmailCost,
                SmsCost = command.SmsCost,
                CurrencyId = command.CurrencyId
            };

            var ConnString = ConnectionString.GetConnectionString(command.CountryId);

            if (operators.OperatorId == 0)
            {
                _operatorRepository.Add(operators);
                _unitOfWork.Commit();

                if (!string.IsNullOrEmpty(ConnString))
                {
                    EFMVCDataContex db = new EFMVCDataContex(ConnString);
                    var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);
                    if(externalServerCountryId != 0)
                    {
                        var operators2 = new Model.Entities.Operator
                        {
                            OperatorId = command.OperatorId,
                            OperatorName = command.OperatorName,
                            CountryId = externalServerCountryId,
                            AdtoneServerOperatorId = operators.OperatorId,
                            IsActive = command.IsActive,
                            EmailCost = command.EmailCost,
                            SmsCost = command.SmsCost,
                            CurrencyId = command.CurrencyId
                        };
                        db.Operator.Add(operators2);
                        db.SaveChanges();
                    }
                   
                }
            }
            else
            {
                _operatorRepository.Update(operators);
                _unitOfWork.Commit();
                if (!string.IsNullOrEmpty(ConnString))
                {
                    EFMVCDataContex db = new EFMVCDataContex(ConnString);
                    var operatorInfo = db.Operator.Where(s => s.AdtoneServerOperatorId == command.OperatorId).FirstOrDefault();
                    if(operatorInfo != null)
                    {
                        var externalServerCountryId = OperatorServer.GetCountryIdFromOperatorServer(db, command.CountryId);
                        if (externalServerCountryId != 0)
                        {
                            operatorInfo.OperatorName = command.OperatorName;
                            operatorInfo.CountryId = externalServerCountryId;
                            operatorInfo.IsActive = command.IsActive;
                            operatorInfo.EmailCost = command.EmailCost;
                            operatorInfo.SmsCost = command.SmsCost;
                            operatorInfo.CurrencyId = command.CurrencyId;
                            db.SaveChanges();
                        }
                            
                    }
                }
            }

           

            return new CommandResult(true);
        }
    }
}
