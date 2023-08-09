// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileRadioCommand.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignProfileRadioCommand.
    /// </summary>
    public class CreateOrUpdateCampaignProfileRadioCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the campaign profile radio identifier.
        /// </summary>
        /// <value>The campaign profile radio identifier.</value>
        public int CampaignProfileRadioId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the national.
        /// </summary>
        /// <value>The national.</value>
        public string National_Radio { get; set; }

        /// <summary>
        /// Gets or sets the local.
        /// </summary>
        /// <value>The local.</value>
        public string Local_Radio { get; set; }

        /// <summary>
        /// Gets or sets the music.
        /// </summary>
        /// <value>The music.</value>
        public string Music_Radio { get; set; }

        /// <summary>
        /// Gets or sets the sport.
        /// </summary>
        /// <value>The sport.</value>
        public string Sport_Radio { get; set; }

        /// <summary>
        /// Gets or sets the talk.
        /// </summary>
        /// <value>The talk.</value>
        public string Talk_Radio { get; set; }
    }
}