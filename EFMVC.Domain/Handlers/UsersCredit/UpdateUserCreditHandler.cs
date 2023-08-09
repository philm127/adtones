using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers
{
    public class UpdateUserCreditHandler : ICommandHandler<UpdateUserCreditCommand>
    {
        /// <summary>
        /// The _usercredit repository
        /// </summary>
        private readonly IUsersCreditRepository _usercreditRepository;


        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCreditHandler(IUsersCreditRepository usercreditRepository, IUnitOfWork unitOfWork)
        {
            _usercreditRepository = usercreditRepository;
            _unitOfWork = unitOfWork;
        }
        public ICommandResult Execute(UpdateUserCreditCommand command)
        {
            UsersCredit userCredit = _usercreditRepository.Get(top=>top.UserId==command.UserId);
            userCredit.UserId = command.UserId;
            userCredit.AvailableCredit = command.AvailableCredit;
            _unitOfWork.Commit();

            return new CommandResult(true);
        }
    }
}
