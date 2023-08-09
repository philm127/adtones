// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileAuditHandler.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
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
    /// Class CreateOrUpdateCampaignAuditHandler.
    /// </summary>
    public class CreateOrUpdateCampaignAuditHandler : ICommandHandler<CreateOrUpdateCampaignAuditCommand>
    {
        /// <summary>
        /// The _audit repository
        /// </summary>
        private readonly ICampaignAuditRepository _auditRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateCampaignAuditHandler"/> class.
        /// </summary>
        /// <param name="auditRepository">The audit repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateCampaignAuditHandler(ICampaignAuditRepository auditRepository, IUnitOfWork unitOfWork)
        {
            _auditRepository = auditRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateCampaignAuditCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateCampaignAuditCommand command)
        {
            var audit = new CampaignAudit
            {
                CampaignProfileId = command.CampaignProfileId,
                CampaignAuditId = command.CampaignAuditId,
                BidValue = command.BidValue,
                Email = command.Email,
                EmailCost = command.EmailCost,
                EndTime = command.EndTime,
                PlayLengthTicks = command.PlayLengthTicks,
                SMS = command.SMS,
                SMSCost = command.SMSCost,
                StartTime = command.StartTime,
                Status = command.Status,
                TotalCost = command.TotalCost,
                UserProfileId = command.UserProfileId
            };

            if (audit.CampaignAuditId == 0)
                _auditRepository.Add(audit);
            else
                _auditRepository.Update(audit);

            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}