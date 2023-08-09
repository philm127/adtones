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
    public class CreateOrUpdateCopyRightHandler : ICommandHandler<CreateOrUpdateCopyRightCommand>
    {

        /// <summary>
        /// The _copyRight Repository
        /// </summary>
        private readonly ICopyRightRepository _copyRightRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCopyRightHandler"/> class.
        /// </summary>
        /// <param name="copyRightRepository">The reward repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCopyRightHandler(ICopyRightRepository copyRightRepository, IUnitOfWork unitOfWork)
        {
            _copyRightRepository = copyRightRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(CreateOrUpdateCopyRightCommand command)
        {
            Model.Entities.CopyRight copyRight = null;
            //EFMVCDataContex db = new EFMVCDataContex();
            if (command.Id != 0)
            {
                copyRight = _copyRightRepository.GetById(command.Id);
                if (copyRight != null)
                {
                    copyRight.CopyRightId = command.Id;
                    copyRight.CopyRightText = command.Text;
                    copyRight.Active = command.Active;
                    copyRight.CreatedDate = command.CreatedDate.Value;
                    copyRight.UpdatedDate = DateTime.Now;
                }
            }
            else
            {
                copyRight = new Model.Entities.CopyRight
                {
                    CopyRightId = command.Id,
                    CopyRightText = command.Text,
                    Active = command.Active,
                    CreatedDate = command.CreatedDate.Value,
                    UpdatedDate = DateTime.Now
                };
            }

            if (copyRight.CopyRightId == 0)
            {
                _copyRightRepository.Add(copyRight);
                _unitOfWork.Commit();
            }
            else
            {
                _copyRightRepository.Update(copyRight);
                _unitOfWork.Commit();
            }

            return new CommandResult(true);
        }
    }
}
