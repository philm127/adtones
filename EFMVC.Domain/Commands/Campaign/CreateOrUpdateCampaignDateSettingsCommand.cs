// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignDateSettingsCommand.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using EFMVC.CommandProcessor.Command;

/// <summary>
/// The Commands namespace.
/// </summary>

namespace EFMVC.Domain.Commands
{
    /// <summary>
    /// Class CreateOrUpdateCampaignDateSettingsCommand.
    /// </summary>
    public class CreateOrUpdateCampaignDateSettingsCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the campaign date settings identifier.
        /// </summary>
        /// <value>The campaign date settings identifier.</value>
        public int CampaignDateSettingsId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the campaign date.
        /// </summary>
        /// <value>The campaign date.</value>
        public DateTime CampaignDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CreateOrUpdateCampaignDateSettingsCommand"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; set; }
    }
}