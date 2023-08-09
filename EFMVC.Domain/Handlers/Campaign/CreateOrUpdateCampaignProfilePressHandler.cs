// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfilePressHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignProfilePressHandler.
    /// </summary>
    public class CreateOrUpdateCampaignProfilePressHandler : ICommandHandler<CreateOrUpdateCampaignProfilePressCommand>
    {
        /// <summary>
        /// The _press repository
        /// </summary>
        private readonly ICampaignProfilePreferenceRepository _pressRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignProfilePressHandler"/> class.
        /// </summary>
        /// <param name="pressRepository">The press repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignProfilePressHandler(ICampaignProfilePreferenceRepository pressRepository,
                                                         IUnitOfWork unitOfWork)
        {
            _pressRepository = pressRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateCampaignProfilePressCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateCampaignProfilePressCommand command)
        {
            var press = new CampaignProfilePreference
                            {
                                FreeNewpapers_Press = command.FreeNewpapers_Press,
                                Local_Press = command.Local_Press,
                                Magazines_Press = command.Magazines_Press,
                                National_Press = command.National_Press,
                                CampaignProfileId = command.CampaignProfileId,
                                Id = command.CampaignProfilePressId
                            };

            if (press.Id == 0)
            {
                _pressRepository.Add(press);
            }
            else
            {
                CampaignProfilePreference campaignProfile = _pressRepository.GetById(command.CampaignProfilePressId);
                campaignProfile.FreeNewpapers_Press = command.FreeNewpapers_Press;
                campaignProfile.Local_Press = command.Local_Press;
                campaignProfile.Magazines_Press = command.Magazines_Press;
                campaignProfile.National_Press = command.National_Press;
                campaignProfile.CampaignProfileId = command.CampaignProfileId;
                campaignProfile.Id = campaignProfile.Id;
                _pressRepository.Update(campaignProfile);
            }
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}