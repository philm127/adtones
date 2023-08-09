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
    public class CreateOrUpdateOperatorConfigurationHandler : ICommandHandler<CreateOrUpdateOperatorConfigurationCommand>
    {

        /// <summary>
        /// The _operatorConfiguration repository
        /// </summary>
        private readonly IOperatorConfigurationRepository _operatorConfigurationRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateOperatorConfigurationHandler"/> class.
        /// </summary>
        /// <param name="operatorRepository">The country repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateOperatorConfigurationHandler(IOperatorConfigurationRepository operatorConfigurationRepository, IUnitOfWork unitOfWork)
        {
            _operatorConfigurationRepository = operatorConfigurationRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(CreateOrUpdateOperatorConfigurationCommand command)
        {
            var operatorConfiguration = new Model.Entities.OperatorConfiguration
            {
                OperatorConfigurationId = command.OperatorConfigurationId,
                OperatorId = command.OperatorId,
                Days = command.Days,   
                IsActive = command.IsActive,
                AddedDate = command.Addeddate,
                UpdatedDate = command.Updateddate,
                AdtoneServerOperatorConfigurationId = null
            };

            var ConnString = ConnectionString.GetSingleConnectionStringByOperatorId(command.OperatorId);

            if (operatorConfiguration.OperatorConfigurationId == 0)
            {
                _operatorConfigurationRepository.Add(operatorConfiguration);
                _unitOfWork.Commit();

                if (!string.IsNullOrEmpty(ConnString))
                {
                    EFMVCDataContex db = new EFMVCDataContex(ConnString);
                    var externalServerOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, command.OperatorId);
                    if(externalServerOperatorId != 0)
                    {
                        var operatorConfiguration2 = new Model.Entities.OperatorConfiguration
                        {
                            OperatorConfigurationId = command.OperatorConfigurationId,
                            OperatorId = externalServerOperatorId,
                            Days = command.Days,
                            IsActive = command.IsActive,
                            AddedDate = command.Addeddate,
                            UpdatedDate = command.Updateddate,
                            AdtoneServerOperatorConfigurationId = operatorConfiguration.OperatorConfigurationId,
                        };
                        db.OperatorConfigurations.Add(operatorConfiguration2);
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                _operatorConfigurationRepository.Update(operatorConfiguration);
                _unitOfWork.Commit();
                if (!string.IsNullOrEmpty(ConnString))
                {
                    EFMVCDataContex db = new EFMVCDataContex(ConnString);
                    var operatorConfigurationInfo = db.OperatorConfigurations.Where(s => s.AdtoneServerOperatorConfigurationId == command.OperatorConfigurationId).FirstOrDefault();
                    if (operatorConfigurationInfo != null)
                    {
                        var externalServerOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, command.OperatorId);
                        if (externalServerOperatorId != 0)
                        {
                            operatorConfigurationInfo.OperatorId = externalServerOperatorId;
                            operatorConfigurationInfo.Days = command.Days;
                            operatorConfigurationInfo.IsActive = command.IsActive;
                            operatorConfigurationInfo.AddedDate = command.Addeddate;
                            operatorConfigurationInfo.UpdatedDate = command.Updateddate;
                            db.SaveChanges();
                        }
                    }
                }
            }
            return new CommandResult(true);
        }
    }
}
