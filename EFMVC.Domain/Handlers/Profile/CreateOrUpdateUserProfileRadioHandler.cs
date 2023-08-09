// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileRadioHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateUserProfileRadioHandler.
    /// </summary>
    public class CreateOrUpdateUserProfileRadioHandler : ICommandHandler<CreateOrUpdateUserProfileRadioCommand>
    {
        /// <summary>
        /// The _radio repository
        /// </summary>
        private readonly IUserProfilePreferenceRepository _userProfilePreferenceRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateUserProfileRadioHandler"/> class.
        /// </summary>
        /// <param name="radioRepository">The radio repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateUserProfileRadioHandler(IUserProfilePreferenceRepository userProfilePreferenceRepository, IUnitOfWork unitOfWork)
        {
            _userProfilePreferenceRepository = userProfilePreferenceRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateUserProfileRadioCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateUserProfileRadioCommand command)
        {
            var radio = new UserProfilePreference
            {
                Local_Radio = command.Local_Radio,
                Music_Radio = command.Music_Radio,
                National_Radio = command.National_Radio,
                Sport_Radio = command.Sport_Radio,
                Talk_Radio = command.Talk_Radio,
                UserProfileId = command.UserProfileId,
                Id = command.Id
            };

            if (radio.Id == 0)
            {
                _userProfilePreferenceRepository.Add(radio);
            }
            else
            {
                UserProfilePreference userprofile = _userProfilePreferenceRepository.GetById(command.Id);
                userprofile.Local_Radio = command.Local_Radio;
                userprofile.Music_Radio = command.Music_Radio;
                userprofile.National_Radio = command.National_Radio;
                userprofile.Sport_Radio = command.Sport_Radio;
                userprofile.Talk_Radio = command.Talk_Radio;
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