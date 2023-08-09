// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Administrator
// Created          : 05-09-2014
//
// Last Modified By : Administrator
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CampaignAdvertFormModel.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.ComponentModel.DataAnnotations;

/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class CampaignAdvertFormModel.
    /// </summary>
    public class CampaignAdvertFormModel
    {
        /// <summary>
        /// Gets or sets the campaign advert identifier.
        /// </summary>
        /// <value>The campaign advert identifier.</value>
        [Key]
        [Display(Name = "Id")]
        public int CampaignAdvertId { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the advert identifier.
        /// </summary>
        /// <value>The advert identifier.</value>
        public int AdvertId { get; set; }
        public bool NextStatus { get; set; }

        /// <summary>
        /// Gets or sets the advert.
        /// </summary>
        /// <value>The advert.</value>
        public virtual AdvertFormModel Advert { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile.
        /// </summary>
        /// <value>The campaign profile.</value>
        public virtual CampaignProfileFormModel CampaignProfile { get; set; }
    }
}