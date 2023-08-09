// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileRadioHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignProfileRadioHandler.
    /// </summary>
    public class CreateOrUpdateCampaignProfileRadioHandler : ICommandHandler<CreateOrUpdateCampaignProfileRadioCommand>
    {
        /// <summary>
        /// The _radio repository
        /// </summary>
        private readonly ICampaignProfilePreferenceRepository _radioRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignProfileRadioHandler"/> class.
        /// </summary>
        /// <param name="radioRepository">The radio repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignProfileRadioHandler(ICampaignProfilePreferenceRepository radioRepository,
                                                         IUnitOfWork unitOfWork)
        {
            _radioRepository = radioRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateCampaignProfileRadioCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateCampaignProfileRadioCommand command)
        {
            var radio = new CampaignProfilePreference
                            {
                                Local_Radio = command.Local_Radio,
                                Music_Radio = command.Music_Radio,
                                National_Radio = command.National_Radio,
                                Sport_Radio = command.Sport_Radio,
                                Talk_Radio = command.Talk_Radio,
                                CampaignProfileId = command.CampaignProfileId,
                                Id = command.CampaignProfileRadioId
                            };

            if (radio.Id == 0)
            {
                _radioRepository.Add(radio);
            }
            else
            {
                CampaignProfilePreference campaignProfile = _radioRepository.GetById(command.CampaignProfileRadioId);
                campaignProfile.Local_Radio = command.Local_Radio;
                campaignProfile.Music_Radio = command.Music_Radio;
                campaignProfile.National_Radio = command.National_Radio;
                campaignProfile.Sport_Radio = command.Sport_Radio;
                campaignProfile.Talk_Radio = command.Talk_Radio;
                campaignProfile.CampaignProfileId = command.CampaignProfileId;
                campaignProfile.Id = campaignProfile.Id;
                _radioRepository.Update(campaignProfile);
            }
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}