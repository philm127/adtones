// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileTvCommand.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignProfileTvCommand.
    /// </summary>
    public class CreateOrUpdateCampaignProfileTvCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the campaign profile tv identifier.
        /// </summary>
        /// <value>The campaign profile tv identifier.</value>
        public int CampaignProfileTvId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the satallite.
        /// </summary>
        /// <value>The satallite.</value>
        public string Satallite_TV { get; set; }

        /// <summary>
        /// Gets or sets the cable.
        /// </summary>
        /// <value>The cable.</value>
        public string Cable_TV { get; set; }

        /// <summary>
        /// Gets or sets the terrestrial.
        /// </summary>
        /// <value>The terrestrial.</value>
        public string Terrestrial_TV { get; set; }

        /// <summary>
        /// Gets or sets the internet.
        /// </summary>
        /// <value>The internet.</value>
        public string Internet_TV { get; set; }
    }
}