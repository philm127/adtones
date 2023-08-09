// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileInternetHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateUserProfileInternetHandler.
    /// </summary>
    public class CreateOrUpdateUserProfileInternetHandler : ICommandHandler<CreateOrUpdateUserProfileInternetCommand>
    {
        /// <summary>
        /// The _internet repository
        /// </summary>
        private readonly IUserProfilePreferenceRepository _userProfilePreferenceRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateUserProfileInternetHandler"/> class.
        /// </summary>
        /// <param name="internetRepository">The internet repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateUserProfileInternetHandler(IUserProfilePreferenceRepository userProfilePreferenceRepository, IUnitOfWork unitOfWork)
        {
            _userProfilePreferenceRepository = userProfilePreferenceRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateUserProfileInternetCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateUserProfileInternetCommand command)
        {
            var internet = new UserProfilePreference
            {
                Auctions_Internet = command.Auctions_Internet,
                Research_Internet = command.Research_Internet,
                Shopping_Internet = command.Shopping_Internet,
                SocialNetworking_Internet = command.SocialNetworking_Internet,
                Video_Internet = command.Video_Internet,
                UserProfileId = command.UserProfileId,
                Id = command.Id
            };

            if (internet.Id == 0)
            {
                _userProfilePreferenceRepository.Add(internet);
            }
            else
            {
                UserProfilePreference userprofile = _userProfilePreferenceRepository.GetById(command.Id);
                userprofile.Auctions_Internet = command.Auctions_Internet;
                userprofile.Research_Internet = command.Research_Internet;
                userprofile.Shopping_Internet = command.Shopping_Internet;
                userprofile.SocialNetworking_Internet = command.SocialNetworking_Internet;
                userprofile.Video_Internet = command.Video_Internet;
                userprofile.UserProfileId = command.UserProfileId;
                userprofile.Id = command.Id;
                _userProfilePreferenceRepository.Update(userprofile);
            }
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}