// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileTvHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignProfileTvHandler.
    /// </summary>
    public class CreateOrUpdateCampaignProfileTvHandler : ICommandHandler<CreateOrUpdateCampaignProfileTvCommand>
    {
        /// <summary>
        /// The _campaign tv repository
        /// </summary>
        private readonly ICampaignProfilePreferenceRepository _campaignTvRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignProfileTvHandler"/> class.
        /// </summary>
        /// <param name="campaignTvRepository">The campaign tv repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignProfileTvHandler(ICampaignProfilePreferenceRepository campaignTvRepository, IUnitOfWork unitOfWork)
        {
            _campaignTvRepository = campaignTvRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateCampaignProfileTvCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateCampaignProfileTvCommand command)
        {
            var tv = new CampaignProfilePreference
                         {
                             Cable_TV = command.Cable_TV,
                             Internet_TV = command.Internet_TV,
                             Satallite_TV = command.Satallite_TV,
                             Terrestrial_TV = command.Terrestrial_TV,
                             CampaignProfileId = command.CampaignProfileId,
                             Id = command.CampaignProfileTvId
                         };

            if (tv.Id == 0)
            {
                _campaignTvRepository.Add(tv);
            }
            else
            {
                CampaignProfilePreference campaignProfile = _campaignTvRepository.GetById(command.CampaignProfileTvId);
                campaignProfile.Cable_TV = command.Cable_TV;
                campaignProfile.Internet_TV = command.Internet_TV;
                campaignProfile.Satallite_TV = command.Satallite_TV;
                campaignProfile.Terrestrial_TV = command.Terrestrial_TV;
                campaignProfile.CampaignProfileId = command.CampaignProfileId;
                campaignProfile.Id = campaignProfile.Id;
                _campaignTvRepository.Update(campaignProfile);
            }
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}