// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="ChangePasswordHandler.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;

/// <summary>
/// The Security namespace.
/// </summary>

namespace EFMVC.Domain.Handlers.Security
{
    /// <summary>
    /// Class ChangePasswordHandler.
    /// </summary>
    public class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand>
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
        public ChangePasswordHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
        }

        #region ICommandHandler<ChangePasswordCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(ChangePasswordCommand command)
        {
            User user = userRepository.GetById(command.UserId);
            user.Password = command.NewPassword;
            userRepository.Update(user);
            unitOfWork.Commit();
            return new CommandResult(true);
        }

        #endregion
    }
}