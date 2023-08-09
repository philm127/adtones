// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileInternetHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignProfileInternetHandler.
    /// </summary>
    public class CreateOrUpdateCampaignProfileInternetHandler :
        ICommandHandler<CreateOrUpdateCampaignProfileInternetCommand>
    {
        /// <summary>
        /// The _internet repository
        /// </summary>
        private readonly ICampaignProfilePreferenceRepository _internetRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignProfileInternetHandler"/> class.
        /// </summary>
        /// <param name="internetRepository">The internet repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignProfileInternetHandler(ICampaignProfilePreferenceRepository internetRepository,
                                                            IUnitOfWork unitOfWork)
        {
            _internetRepository = internetRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateCampaignProfileInternetCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateCampaignProfileInternetCommand command)
        {
            var internet = new CampaignProfilePreference
                               {
                                   Auctions_Internet = command.Auctions_Internet,
                                   Research_Internet = command.Research_Internet,
                                   Shopping_Internet = command.Shopping_Internet,
                                   SocialNetworking_Internet = command.SocialNetworking_Internet,
                                   Video_Internet = command.Video_Internet,
                                   CampaignProfileId = command.CampaignProfileId,
                                   Id = command.CampaignProfileInternetId
                               };

            if (internet.Id == 0)
            {
                _internetRepository.Add(internet);
            }
            else
            {
                CampaignProfilePreference campaignProfile = _internetRepository.GetById(command.CampaignProfileInternetId);
                campaignProfile.Auctions_Internet = command.Auctions_Internet;
                campaignProfile.Research_Internet = command.Research_Internet;
                campaignProfile.Shopping_Internet = command.Shopping_Internet;
                campaignProfile.SocialNetworking_Internet = command.SocialNetworking_Internet;
                campaignProfile.Video_Internet = command.Video_Internet;
                campaignProfile.CampaignProfileId = command.CampaignProfileId;
                campaignProfile.Id = campaignProfile.Id;
                _internetRepository.Update(campaignProfile);
            }
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}