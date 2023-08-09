// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="DeleteProfileHandler.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands.BlockedNumber;

/// <summary>
/// The BlockedNumber namespace.
/// </summary>

namespace EFMVC.Domain.Handlers.BlockedNumber
{
    /// <summary>
    /// Class DeleteBlockedNumberHandler.
    /// </summary>
    public class DeleteBlockedNumberHandler : ICommandHandler<DeleteBlockedNumberCommand>
    {
        /// <summary>
        /// The _blocked number repository
        /// </summary>
        private readonly IBlockedNumberRepository _blockedNumberRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteBlockedNumberHandler"/> class.
        /// </summary>
        /// <param name="blockedNumberRepository">The blocked number repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeleteBlockedNumberHandler(IBlockedNumberRepository blockedNumberRepository, IUnitOfWork unitOfWork)
        {
            _blockedNumberRepository = blockedNumberRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<DeleteBlockedNumberCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(DeleteBlockedNumberCommand command)
        {
            Model.BlockedNumber blockedNumber = _blockedNumberRepository.GetById(command.Id);
            _blockedNumberRepository.Delete(blockedNumber);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}