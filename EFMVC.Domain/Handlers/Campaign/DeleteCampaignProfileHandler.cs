// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="DeleteCampaignProfileHandler.cs" company="Noat">
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
    /// Class DeleteCampaignProfileHandler.
    /// </summary>
    public class DeleteCampaignProfileHandler : ICommandHandler<DeleteCamapignProfileCommand>
    {
        /// <summary>
        /// The _campaign repository
        /// </summary>
        private readonly ICampaignRepository _campaignRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCampaignProfileHandler"/> class.
        /// </summary>
        /// <param name="campaignRepository">The campaign repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public DeleteCampaignProfileHandler(ICampaignRepository campaignRepository, IUnitOfWork unitOfWork)
        {
            _campaignRepository = campaignRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<DeleteCamapignProfileCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(DeleteCamapignProfileCommand command)
        {
            CampaignProfile campaignProfile = _campaignRepository.GetById(command.Id);
            _campaignRepository.Delete(campaignProfile);
            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}