// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignAdvertCommand.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignAdvertCommand.
    /// </summary>
    public class CreateOrUpdateCampaignAdvertCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the campaign advert identifier.
        /// </summary>
        /// <value>The campaign advert identifier.</value>
        public int CampaignAdvertId { get; set; }

        /// <summary>
        /// Gets or sets the advert identifier.
        /// </summary>
        /// <value>The advert identifier.</value>
        public int AdvertId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        public bool NextStatus { get; set; }
    }
}