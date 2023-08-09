// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileAttitudeHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignProfileAttitudeHandler.
    /// </summary>
    public class CreateOrUpdateCampaignProfileAttitudeHandler :
        ICommandHandler<CreateOrUpdateCampaignProfileAttitudeCommand>
    {
        /// <summary>
        /// The _attitude repository
        /// </summary>
        private readonly ICampaignProfilePreferenceRepository _attitudeRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignProfileAttitudeHandler"/> class.
        /// </summary>
        /// <param name="attitudeRepository">The attitude repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignProfileAttitudeHandler(ICampaignProfilePreferenceRepository attitudeRepository,
                                                            IUnitOfWork unitOfWork)
        {
            _attitudeRepository = attitudeRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateCampaignProfileAttitudeCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateCampaignProfileAttitudeCommand command)
        {
            var attitude = new CampaignProfilePreference
                               {
                                   Environment_Attitude = command.Environment_Attitude,
                                   Fashion_Attitude = command.Fashion_Attitude,
                                   FinancialStabiity_Attitude = command.FinancialStabiity_Attitude,
                                   Fitness_Attitude = command.Fitness_Attitude,
                                   GoingOut_Attitude = command.GoingOut_Attitude,
                                   Holidays_Attitude = command.Holidays_Attitude,
                                   Music_Attitude = command.Music_Attitude,
                                   Religion_Attitude = command.Religion_Attitude,
                                   CampaignProfileId = command.CampaignProfileId,
                                   Id = command.CampaignProfileAttitudeId
                               };

            if (attitude.Id == 0)
            {
                _attitudeRepository.Add(attitude);
            }
            else
            {
                CampaignProfilePreference campaignProfile = _attitudeRepository.GetById(command.CampaignProfileAttitudeId);
                campaignProfile.Environment_Attitude = command.Environment_Attitude;
                campaignProfile.Fashion_Attitude = command.Fashion_Attitude;
                campaignProfile.FinancialStabiity_Attitude = command.FinancialStabiity_Attitude;
                campaignProfile.Fitness_Attitude = command.Fitness_Attitude;
                campaignProfile.GoingOut_Attitude = command.GoingOut_Attitude;
                campaignProfile.Holidays_Attitude = command.Holidays_Attitude;
                campaignProfile.Music_Attitude = command.Music_Attitude;
                campaignProfile.Religion_Attitude = command.Religion_Attitude;
                campaignProfile.CampaignProfileId = command.CampaignProfileId;
                campaignProfile.Id = campaignProfile.Id;
                _attitudeRepository.Update(campaignProfile);
            }
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}