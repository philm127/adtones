// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileAttitudeHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateUserProfileAttitudeHandler.
    /// </summary>
    public class CreateOrUpdateUserProfileAttitudeHandler : ICommandHandler<CreateOrUpdateUserProfileAttitudeCommand>
    {
        /// <summary>
        /// The _attitude repository
        /// </summary>
        private readonly IUserProfilePreferenceRepository _userProfilePreferenceRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateUserProfileAttitudeHandler"/> class.
        /// </summary>
        /// <param name="attitudeRepository">The attitude repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateUserProfileAttitudeHandler(IUserProfilePreferenceRepository userProfilePreferenceRepository, IUnitOfWork unitOfWork)
        {
            _userProfilePreferenceRepository = userProfilePreferenceRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateUserProfileAttitudeCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateUserProfileAttitudeCommand command)
        {
            var attitude = new UserProfilePreference
            {
                Environment_Attitude = command.Environment_Attitude,
                Fashion_Attitude = command.Fashion_Attitude,
                FinancialStabiity_Attitude = command.FinancialStabiity_Attitude,
                Fitness_Attitude = command.Fitness_Attitude,
                GoingOut_Attitude = command.GoingOut_Attitude,
                Holidays_Attitude = command.Holidays_Attitude,
                Music_Attitude = command.Music_Attitude,
                Religion_Attitude = command.Religion_Attitude,
                UserProfileId = command.UserProfileId,
                Id = command.Id
            };

            if (attitude.Id == 0)
            {
                _userProfilePreferenceRepository.Add(attitude);
            }
            else
            {
                UserProfilePreference userprofile = _userProfilePreferenceRepository.GetById(command.Id);
                userprofile.Environment_Attitude = command.Environment_Attitude;
                userprofile.Fashion_Attitude = command.Fashion_Attitude;
                userprofile.FinancialStabiity_Attitude = command.FinancialStabiity_Attitude;
                userprofile.Fitness_Attitude = command.Fitness_Attitude;
                userprofile.GoingOut_Attitude = command.GoingOut_Attitude;
                userprofile.Holidays_Attitude = command.Holidays_Attitude;
                userprofile.Music_Attitude = command.Music_Attitude;
                userprofile.Religion_Attitude = command.Religion_Attitude;
                _userProfilePreferenceRepository.Update(userprofile);

            }
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}