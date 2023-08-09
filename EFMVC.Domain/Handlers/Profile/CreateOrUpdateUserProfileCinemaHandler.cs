// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileCinemaHandler.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;
using EFMVC.Data.Infrastructure;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Model;

/// <summary>
/// The Handlers namespace.
/// </summary>

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// Class CreateOrUpdateUserProfileCinemaHandler.
    /// </summary>
    public class CreateOrUpdateUserProfileCinemaHandler : ICommandHandler<CreateOrUpdateUserProfileCinemaCommand>
    {
        /// <summary>
        /// The _cinema repository
        /// </summary>
        private readonly IUserProfilePreferenceRepository _userProfilePreferenceRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateUserProfileCinemaHandler"/> class.
        /// </summary>
        /// <param name="cinemaRepository">The cinema repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateUserProfileCinemaHandler(IUserProfilePreferenceRepository userProfilePreferenceRepository, IUnitOfWork unitOfWork)
        {
            _userProfilePreferenceRepository = userProfilePreferenceRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateUserProfileCinemaCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateUserProfileCinemaCommand command)
        {
            var cinema = new UserProfilePreference
                             {
                                Cinema_Cinema = command.Cinema_Cinema,
                                 UserProfileId = command.UserProfileId,
                                 Id = command.Id
                             };

            if (cinema.Id == 0)
            {
                _userProfilePreferenceRepository.Add(cinema);
            }
            else
            {
                UserProfilePreference userprofile = _userProfilePreferenceRepository.GetById(command.Id);
                userprofile.Cinema_Cinema = command.Cinema_Cinema;
                userprofile.UserProfileId = command.UserProfileId;
                _userProfilePreferenceRepository.Update(userprofile);
            }
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}