// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateCampaignProfileMobileCommand.cs" company="Noat">
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
    /// Class CreateOrUpdateCampaignProfileMobileCommand.
    /// </summary>
    public class CreateOrUpdateCampaignProfileMobileCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the campaign profile mobile identifier.
        /// </summary>
        /// <value>The campaign profile mobile identifier.</value>
        public int CampaignProfileMobileId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the type of the contract.
        /// </summary>
        /// <value>The type of the contract.</value>
        public string ContractType_Mobile { get; set; }

        /// <summary>
        /// Gets or sets the spend.
        /// </summary>
        /// <value>The spend.</value>
        public string Spend_Mobile { get; set; }

        public bool NextStatus { get; set; }
    }
}