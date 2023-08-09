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
    public class CreateOrUpdateOperatorMaxAdvertHandler : ICommandHandler<CreateOrUpdateOperatorMaxAdvertCommand>
    {

        /// <summary>
        /// The _country repository
        /// </summary>
        private readonly IOperatorMaxAdvertRepository _operatorMaxAdvertRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateOperatorHandler"/> class.
        /// </summary>
        /// <param name="operatorRepository">The country repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateOperatorMaxAdvertHandler(IOperatorMaxAdvertRepository operatorMaxAdvertRepository, IUnitOfWork unitOfWork)
        {
            _operatorMaxAdvertRepository = operatorMaxAdvertRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(CreateOrUpdateOperatorMaxAdvertCommand command)
        {
            var operatorMaxAdvert = new Model.Entities.OperatorMaxAdvert
            {
                OperatorMaxAdvertId = command.OperatorMaxAdvertId,
                OperatorId = command.OperatorId,
                KeyName = command.KeyName,   
                KeyValue = command.KeyValue,
                Addeddate = command.Addeddate,
                Updateddate = command.Updateddate,
                AdtoneServerOperatorMaxAdvertId = null
            };

            var ConnString = ConnectionString.GetSingleConnectionStringByOperatorId(command.OperatorId);

            if (operatorMaxAdvert.OperatorMaxAdvertId == 0)
            {
                _operatorMaxAdvertRepository.Add(operatorMaxAdvert);
                _unitOfWork.Commit();

                if (!string.IsNullOrEmpty(ConnString))
                {
                    EFMVCDataContex db = new EFMVCDataContex(ConnString);
                    var externalServerOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, command.OperatorId);
                    if(externalServerOperatorId != 0)
                    {
                        var operatorMaxAdvert2 = new Model.Entities.OperatorMaxAdvert
                        {
                            OperatorMaxAdvertId = command.OperatorMaxAdvertId,
                            OperatorId = externalServerOperatorId,
                            KeyName = command.KeyName,
                            KeyValue = command.KeyValue,
                            Addeddate = command.Addeddate,
                            Updateddate = command.Updateddate,
                            AdtoneServerOperatorMaxAdvertId = operatorMaxAdvert.OperatorMaxAdvertId,
                        };
                        db.OperatorMaxAdverts.Add(operatorMaxAdvert2);
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                _operatorMaxAdvertRepository.Update(operatorMaxAdvert);
                _unitOfWork.Commit();
                if (!string.IsNullOrEmpty(ConnString))
                {
                    EFMVCDataContex db = new EFMVCDataContex(ConnString);
                    var operatorMaxAdvertInfo = db.OperatorMaxAdverts.Where(s => s.AdtoneServerOperatorMaxAdvertId == command.OperatorMaxAdvertId).FirstOrDefault();
                    if (operatorMaxAdvertInfo != null)
                    {
                        var externalServerOperatorId = OperatorServer.GetOperatorIdFromOperatorServer(db, command.OperatorId);
                        if (externalServerOperatorId != 0)
                        {
                            operatorMaxAdvertInfo.OperatorId = externalServerOperatorId;
                            operatorMaxAdvertInfo.KeyName = command.KeyName;
                            operatorMaxAdvertInfo.KeyValue = command.KeyValue;
                            operatorMaxAdvertInfo.Addeddate = command.Addeddate;
                            operatorMaxAdvertInfo.Updateddate = command.Updateddate;
                            db.SaveChanges();
                        }
                    }
                }
            }
            return new CommandResult(true);
        }
    }
}
