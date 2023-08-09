// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateBucketAuditRowCommand.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;

/// <summary>
/// The Commands namespace.
/// </summary>

namespace EFMVC.Domain.Commands
{
    /// <summary>
    /// Class CreateOrUpdateBucketAuditRowCommand.
    /// </summary>
    public class CreateOrUpdateBucketAuditRowCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the bucket audit identifier.
        /// </summary>
        /// <value>The bucket audit identifier.</value>
        public int BucketAuditId { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public int State { get; set; }

        /// <summary>
        /// Gets or sets the media URL.
        /// </summary>
        /// <value>The media URL.</value>
        public string MediaUrl { get; set; }

        /// <summary>
        /// Gets or sets the bid value.
        /// </summary>
        /// <value>The bid value.</value>
        public string BidValue { get; set; }

        /// <summary>
        /// Gets or sets the DTMF.
        /// </summary>
        /// <value>The DTMF.</value>
        public string Dtmf { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>The start.</value>
        public string Start { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>The end.</value>
        public string End { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the SMS.
        /// </summary>
        /// <value>The SMS.</value>
        public int Sms { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public int Email { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CreateOrUpdateBucketAuditRowCommand"/> is processed.
        /// </summary>
        /// <value><c>true</c> if processed; otherwise, <c>false</c>.</value>
        public bool Processed { get; set; }
    }
}