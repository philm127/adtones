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
using EFMVC.Domain.Commands.Profile;
using EFMVC.Model;

/// <summary>
/// The Handlers namespace.
/// </summary>

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// Class DeleteProfileHandler.
    /// </summary>
    public class DeleteProfileHandler : ICommandHandler<DeleteProfileCommand>
    {
        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly IProfileRepository _profileRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteProfileHandler"/> class.
        /// </summary>
        /// <param name="profileRepository">The profile repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeleteProfileHandler(IProfileRepository profileRepository, IUnitOfWork unitOfWork)
        {
            _profileRepository = profileRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<DeleteProfileCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(DeleteProfileCommand command)
        {
            UserProfile userProfile = _profileRepository.GetById(command.Id);
            _profileRepository.Delete(userProfile);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}