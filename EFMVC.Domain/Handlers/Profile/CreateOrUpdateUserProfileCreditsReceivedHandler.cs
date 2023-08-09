// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileCreditsReceivedHandler.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Domain.Commands;
using EFMVC.Model;

/// <summary>
/// The Handlers namespace.
/// </summary>

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// Class CreateOrUpdateUserProfileCreditsReceivedHandler.
    /// </summary>
    public class CreateOrUpdateUserProfileCreditsReceivedHandler :
        ICommandHandler<CreateOrUpdateUserProfileCreditsReceivedCommand>
    {
        /// <summary>
        /// The _repository
        /// </summary>
        private readonly IRepository<UserProfileCreditsReceived> _repository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateUserProfileCreditsReceivedHandler"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateUserProfileCreditsReceivedHandler(IRepository<UserProfileCreditsReceived> repository,
                                                               IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateUserProfileCreditsReceivedCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateUserProfileCreditsReceivedCommand command)
        {
            var profile = new UserProfileCreditsReceived
                              {
                                  CurrentMonthCredits = command.CurrentMonthCredits,
                                  Id = command.Id,
                                  LastMonthCredits = command.LastMonthCredits,
                                  TotalCredits = command.TotalCredits,
                                  UserProfileId = command.UserProfileId
                              };

            if (profile.UserProfileId == 0)
                _repository.Add(profile);
            else
                _repository.Update(profile);

            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}