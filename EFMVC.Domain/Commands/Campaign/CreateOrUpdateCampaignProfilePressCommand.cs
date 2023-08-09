// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfilePressCommand.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignProfilePressCommand.
    /// </summary>
    public class CreateOrUpdateCampaignProfilePressCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the campaign profile press identifier.
        /// </summary>
        /// <value>The campaign profile press identifier.</value>
        public int CampaignProfilePressId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the local.
        /// </summary>
        /// <value>The local.</value>
        public string Local_Press { get; set; }

        /// <summary>
        /// Gets or sets the national.
        /// </summary>
        /// <value>The national.</value>
        public string National_Press { get; set; }

        /// <summary>
        /// Gets or sets the free newpapers.
        /// </summary>
        /// <value>The free newpapers.</value>
        public string FreeNewpapers_Press { get; set; }

        /// <summary>
        /// Gets or sets the magazines.
        /// </summary>
        /// <value>The magazines.</value>
        public string Magazines_Press { get; set; }
    }
}