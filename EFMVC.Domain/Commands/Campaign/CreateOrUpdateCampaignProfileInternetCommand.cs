// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileInternetCommand.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignProfileInternetCommand.
    /// </summary>
    public class CreateOrUpdateCampaignProfileInternetCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the campaign profile internet identifier.
        /// </summary>
        /// <value>The campaign profile internet identifier.</value>
        public int CampaignProfileInternetId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the social networking.
        /// </summary>
        /// <value>The social networking.</value>
        public string SocialNetworking_Internet { get; set; }

        /// <summary>
        /// Gets or sets the video.
        /// </summary>
        /// <value>The video.</value>
        public string Video_Internet { get; set; }

        /// <summary>
        /// Gets or sets the research.
        /// </summary>
        /// <value>The research.</value>
        public string Research_Internet { get; set; }

        /// <summary>
        /// Gets or sets the auctions.
        /// </summary>
        /// <value>The auctions.</value>
        public string Auctions_Internet { get; set; }

        /// <summary>
        /// Gets or sets the shopping.
        /// </summary>
        /// <value>The shopping.</value>
        public string Shopping_Internet { get; set; }
    }
}