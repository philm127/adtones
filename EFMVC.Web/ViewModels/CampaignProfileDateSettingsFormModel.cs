// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CampaignProfileDateSettingsFormModel.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class CampaignProfileDateSettingsFormModel.
    /// </summary>
    public class CampaignProfileDateSettingsFormModel
    {
        /// <summary>
        /// Gets or sets the campaign date settings identifier.
        /// </summary>
        /// <value>The campaign date settings identifier.</value>
        [Key]
        [Display(Name = "Id")]
        public int CampaignDateSettingsId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile.
        /// </summary>
        /// <value>The campaign profile.</value>
        public CampaignProfileFormModel CampaignProfile { get; set; }

        /// <summary>
        /// Gets or sets the campaign date.
        /// </summary>
        /// <value>The campaign date.</value>
        [Display(Name = "Date")]
        public DateTime CampaignDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CampaignProfileDateSettingsFormModel"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        [Display(Name = "Active")]
        public bool Active { get; set; }
    }
}