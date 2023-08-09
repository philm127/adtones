// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileDateSettingsHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignProfileDateSettingsHandler.
    /// </summary>
    public class CreateOrUpdateCampaignProfileDateSettingsHandler :
        ICommandHandler<CreateOrUpdateCampaignDateSettingsCommand>
    {
        /// <summary>
        /// The _campaign date settings repository
        /// </summary>
        private readonly ICampaignDateSettingsRepository _campaignDateSettingsRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignProfileDateSettingsHandler"/> class.
        /// </summary>
        /// <param name="campaignDateSettingsRepository">The campaign date settings repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignProfileDateSettingsHandler(
            ICampaignDateSettingsRepository campaignDateSettingsRepository, IUnitOfWork unitOfWork)
        {
            _campaignDateSettingsRepository = campaignDateSettingsRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateCampaignDateSettingsCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateCampaignDateSettingsCommand command)
        {
            var date = new CampaignProfileDateSettings
                           {
                               Active = command.Active,
                               CampaignDate = command.CampaignDate,
                               CampaignDateSettingsId = command.CampaignDateSettingsId,
                               CampaignProfileId = command.CampaignProfileId
                           };

            if (date.CampaignDateSettingsId == 0)
                _campaignDateSettingsRepository.Add(date);
            else
                _campaignDateSettingsRepository.Update(date);

            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}