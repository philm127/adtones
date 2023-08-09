// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateBucketAuditRowHandler.cs" company="Noat">
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
    /// Class CreateOrUpdateBucketAuditRowHandler.
    /// </summary>
    public class CreateOrUpdateBucketAuditRowHandler : ICommandHandler<CreateOrUpdateBucketAuditRowCommand>
    {
        /// <summary>
        /// The _bucket audit row repository
        /// </summary>
        private readonly IBucketAuditRowRepository _bucketAuditRowRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateBucketAuditRowHandler"/> class.
        /// </summary>
        /// <param name="bucketAuditRowRepository">The bucket audit row repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateBucketAuditRowHandler(IBucketAuditRowRepository bucketAuditRowRepository,
                                                   IUnitOfWork unitOfWork)
        {
            _bucketAuditRowRepository = bucketAuditRowRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateBucketAuditRowCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateBucketAuditRowCommand command)
        {
            var row = new BucketAuditRow
                          {
                              Id = command.Id,
                              BucketAuditId = command.BucketAuditId,
                              CampaignProfileId = command.CampaignProfileId,
                              BidValue = command.BidValue,
                              Dtmf = command.Dtmf,
                              Email = command.Email,
                              End = command.End,
                              MediaUrl = command.MediaUrl,
                              Sms = command.Sms,
                              Start = command.Start,
                              State = command.State,
                              Processed = command.Processed
                          };

            if (command.Id != 0)
                _bucketAuditRowRepository.Update(row);
            else
                _bucketAuditRowRepository.Add(row);

            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}