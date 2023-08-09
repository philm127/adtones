using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;

using EFMVC.Model;

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// Class CreateOrUpdateUsersCreditHandler.
    /// </summary>
    /// <seealso cref="ICommandHandler{EFMVC.Domain.Commands.UsersCredit.CreateOrUpdateUsersCreditCommand}" />
    public class CreateOrUpdateUsersCreditHandler : ICommandHandler<CreateOrUpdateUsersCreditCommand>
    {
        /// <summary>
        /// The _usercredit repository
        /// </summary>
        private readonly IUsersCreditRepository _usercreditRepository;


        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateUsersCreditHandler"/> class.
        /// </summary>
        /// <param name="usercreditRepository">The usercredit repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateUsersCreditHandler(IUsersCreditRepository usercreditRepository, IUnitOfWork unitOfWork)
        {
            _usercreditRepository = usercreditRepository;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>
        /// ICommandResult.
        /// </returns>
        public ICommandResult Execute(CreateOrUpdateUsersCreditCommand command)
        {
            if (command.Id == 0)
            {
                var usercredit = new UsersCredit
                {
                    UserId = command.UserId,
                    AssignCredit=command.AssignCredit,
                    AvailableCredit=command.AvailableCredit,
                    CurrencyId = command.CurrencyId,
                    CreatedDate =command.CreatedDate,
                    UpdatedDate=command.UpdatedDate
                };

                _usercreditRepository.Add(usercredit);
            }
            else
            {
                UsersCredit userCredit = _usercreditRepository.GetById(command.Id);
                userCredit.UserId = command.UserId;
                userCredit.CurrencyId = command.CurrencyId;
                userCredit.AssignCredit = command.AssignCredit;
                userCredit.AvailableCredit = command.AvailableCredit;
                userCredit.UpdatedDate = command.UpdatedDate;

                _usercreditRepository.Update(userCredit);
            }

            _unitOfWork.Commit();

            return new CommandResult(true);
        }
    }
}
