// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateBucketAuditCommand.cs" company="Noat">
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
    /// Class CreateOrUpdateBucketAuditHandler.
    /// </summary>
    public class CreateOrUpdateBucketAuditHandler : ICommandHandler<CreateOrUpdateBucketAuditCommand>
    {
        /// <summary>
        /// The _bucket audit repository
        /// </summary>
        private readonly IBucketAuditRepository _bucketAuditRepository;

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateOrUpdateBucketAuditHandler"/> class.
        /// </summary>
        /// <param name="bucketAuditRepository">The bucket audit repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateOrUpdateBucketAuditHandler(IBucketAuditRepository bucketAuditRepository, IUnitOfWork unitOfWork)
        {
            _bucketAuditRepository = bucketAuditRepository;
            _unitOfWork = unitOfWork;
        }

        #region ICommandHandler<CreateOrUpdateBucketAuditCommand> Members

        /// <summary>
        /// Executes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        public ICommandResult Execute(CreateOrUpdateBucketAuditCommand command)
        {
            var bucket = new BucketAudit
                             {
                                 Id = command.Id,
                                 BucketId = command.BucketId,
                                 BucketPeriodStart = command.BucketPeriodStart,
                                 MSISDN = command.MSISDN,
                                 TargetDeliveryServer = command.TargetDeliveryServer,
                                 Processed = command.Processed
                             };


            if (bucket.BucketAuditRows != null && bucket.BucketAuditRows.Count != 0)
            {
                foreach (CreateOrUpdateBucketAuditRowCommand auditRow in command.BucketAuditRows)
                {
                    var row = new BucketAuditRow
                                  {
                                      Id = auditRow.Id,
                                      BucketAuditId = auditRow.BucketAuditId,
                                      CampaignProfileId = auditRow.CampaignProfileId,
                                      BidValue = auditRow.BidValue,
                                      Dtmf = auditRow.Dtmf,
                                      Email = auditRow.Email,
                                      End = auditRow.End,
                                      MediaUrl = auditRow.MediaUrl,
                                      Sms = auditRow.Sms,
                                      Start = auditRow.Start,
                                      State = auditRow.State,
                                      Processed = auditRow.Processed
                                  };

                    bucket.BucketAuditRows.Add(row);
                }
            }

            if (command.Id != 0)
                _bucketAuditRepository.Update(bucket);
            else
                _bucketAuditRepository.Add(bucket);

            _unitOfWork.Commit();

            return new CommandResult(true);
        }

        #endregion
    }
}