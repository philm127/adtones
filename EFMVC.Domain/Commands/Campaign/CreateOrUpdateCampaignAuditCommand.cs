// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignAuditCommand.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations.Schema;
using EFMVC.CommandProcessor.Command;

/// <summary>
/// The Commands namespace.
/// </summary>

namespace EFMVC.Domain.Commands
{
    /// <summary>
    /// Class CreateOrUpdateCampaignAuditCommand.
    /// </summary>
    public class CreateOrUpdateCampaignAuditCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the campaign audit identifier.
        /// </summary>
        /// <value>The campaign audit identifier.</value>
        public int CampaignAuditId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>The user profile identifier.</value>
        public int UserProfileId { get; set; }

        /// <summary>
        /// Gets or sets the bid value.
        /// </summary>
        /// <value>The bid value.</value>
        public double BidValue { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>The start time.</value>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>The end time.</value>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the play length ticks.
        /// </summary>
        /// <value>The play length ticks.</value>
        public Int64 PlayLengthTicks { get; set; }

        /// <summary>
        /// Gets or sets the length of the play.
        /// </summary>
        /// <value>The length of the play.</value>
        [NotMapped]
        public TimeSpan PlayLength { get; set; }

        /// <summary>
        /// Gets or sets the SMS.
        /// </summary>
        /// <value>The SMS.</value>
        public string SMS { get; set; }

        /// <summary>
        /// Gets or sets the SMS cost.
        /// </summary>
        /// <value>The SMS cost.</value>
        public double SMSCost { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the email cost.
        /// </summary>
        /// <value>The email cost.</value>
        public double EmailCost { get; set; }

        /// <summary>
        /// Gets or sets the total cost.
        /// </summary>
        /// <value>The total cost.</value>
        public double TotalCost { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public string Status { get; set; }
       
        
    }
}