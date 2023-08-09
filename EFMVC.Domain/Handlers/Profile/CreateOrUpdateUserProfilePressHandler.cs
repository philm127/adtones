// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfilePressHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateUserProfilePressHandler.
    /// </summary>
    public class CreateOrUpdateUserProfilePressHandler : ICommandHandler<CreateOrUpdateUserProfilePressCommand>
    {
        /// <summary>
        /// The _press repository
        /// </summary>
        private readonly IUserProfilePreferenceRepository _userProfilePreferenceRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateUserProfilePressHandler"/> class.
        /// </summary>
        /// <param name="pressRepository">The press repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateUserProfilePressHandler(IUserProfilePreferenceRepository userProfilePreferenceRepository, IUnitOfWork unitOfWork)
        {
            _userProfilePreferenceRepository = userProfilePreferenceRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateUserProfilePressCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateUserProfilePressCommand command)
        {
            var press = new UserProfilePreference
            {
                FreeNewpapers_Press = command.FreeNewpapers_Press,
                Local_Press = command.Local_Press,
                Magazines_Press = command.Magazines_Press,
                National_Press = command.National_Press,
                UserProfileId = command.UserProfileId,
                Id = command.Id
            };

            if (press.Id == 0)
            {
                _userProfilePreferenceRepository.Add(press);
            }
            else
            {
                UserProfilePreference userprofile = _userProfilePreferenceRepository.GetById(command.Id);
                userprofile.FreeNewpapers_Press = command.FreeNewpapers_Press;
                userprofile.Local_Press = command.Local_Press;
                userprofile.Magazines_Press = command.Magazines_Press;
                userprofile.National_Press = command.National_Press;
                userprofile.UserProfileId = command.UserProfileId;
                userprofile.Id = userprofile.Id;
                _userProfilePreferenceRepository.Update(userprofile);
            }
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}