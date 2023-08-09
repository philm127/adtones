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

using System.Collections.Generic;
using EFMVC.CommandProcessor.Command;

/// <summary>
/// The Commands namespace.
/// </summary>

namespace EFMVC.Domain.Commands
{
    /// <summary>
    /// Class CreateOrUpdateBucketAuditCommand.
    /// </summary>
    public class CreateOrUpdateBucketAuditCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the bucket identifier.
        /// </summary>
        /// <value>The bucket identifier.</value>
        public int BucketId { get; set; }

        /// <summary>
        /// Gets or sets the msisdn.
        /// </summary>
        /// <value>The msisdn.</value>
        public string MSISDN { get; set; }

        /// <summary>
        /// Gets or sets the bucket period start.
        /// </summary>
        /// <value>The bucket period start.</value>
        public string BucketPeriodStart { get; set; }

        /// <summary>
        /// Gets or sets the target delivery server.
        /// </summary>
        /// <value>The target delivery server.</value>
        public string TargetDeliveryServer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CreateOrUpdateBucketAuditCommand"/> is processed.
        /// </summary>
        /// <value><c>true</c> if processed; otherwise, <c>false</c>.</value>
        public bool Processed { get; set; }

        /// <summary>
        /// Gets or sets the bucket audit rows.
        /// </summary>
        /// <value>The bucket audit rows.</value>
        public virtual ICollection<CreateOrUpdateBucketAuditRowCommand> BucketAuditRows { get; set; }
    }
}