// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileTvHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateUserProfileTvHandler.
    /// </summary>
    public class CreateOrUpdateUserProfileTvHandler : ICommandHandler<CreateOrUpdateUserProfileTvCommand>
    {
        /// <summary>
        /// The _TV repository
        /// </summary>
        private readonly IUserProfilePreferenceRepository _userProfilePreferenceRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateUserProfileTvHandler"/> class.
        /// </summary>
        /// <param name="tvRepository">The tv repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateUserProfileTvHandler(IUserProfilePreferenceRepository userProfilePreferenceRepository, IUnitOfWork unitOfWork)
        {
            _userProfilePreferenceRepository = userProfilePreferenceRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateUserProfileTvCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateUserProfileTvCommand command)
        {
            var cinema = new UserProfilePreference
            {
                Cable_TV = command.Cable_TV,
                Internet_TV = command.Internet_TV,
                Satallite_TV = command.Satallite_TV,
                Terrestrial_TV = command.Terrestrial_TV,
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
                userprofile.Cable_TV = command.Cable_TV;
                userprofile.Internet_TV = command.Internet_TV;
                userprofile.Satallite_TV = command.Satallite_TV;
                userprofile.Terrestrial_TV = command.Terrestrial_TV;
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