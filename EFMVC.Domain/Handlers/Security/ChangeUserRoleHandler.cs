using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.Security;
using EFMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFMVC.Domain.Handlers.Security
{
    public class ChangeUserRoleHandler : ICommandHandler<ChangeUserRoleCommand>
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangePasswordHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public ChangeUserRoleHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ICommandHandler<ChangeActiveStatusCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(ChangeUserRoleCommand command)
        {
            User user = userRepository.GetById(command.UserId);
            user.RoleId = command.RoleId;
            user.Outstandingdays = command.Outstandingdays;
            user.FirstName = command.Fname;
            user.LastName = command.Lname;
            user.Organisation = command.Organisation;
            userRepository.Update(user);
            unitOfWork.Commit();
            return new CommandResult(true);
        }

        #endregion
    }
}
