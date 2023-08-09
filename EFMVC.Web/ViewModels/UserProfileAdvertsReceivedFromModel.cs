// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileAdvertsReceivedFromModel.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class UserProfileAdvertsReceivedFromModel.
    /// </summary>
    public class UserProfileAdvertsReceivedFromModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>The user profile identifier.</value>
        public int UserProfileId { get; set; }

        /// <summary>
        /// Gets or sets the advert reference.
        /// </summary>
        /// <value>The advert reference.</value>
        public string AdvertRef { get; set; }

        /// <summary>
        /// Gets or sets the name of the advert.
        /// </summary>
        /// <value>The name of the advert.</value>
        public string AdvertName { get; set; }

        /// <summary>
        /// Gets or sets the brand.
        /// </summary>
        /// <value>The brand.</value>
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the date time played.
        /// </summary>
        /// <value>The date time played.</value>
        public DateTime DateTimePlayed { get; set; }

        /// <summary>
        /// Gets or sets the credits received.
        /// </summary>
        /// <value>The credits received.</value>
        //public string CreditsReceived { get; set; }
        public decimal CreditsReceived { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string Status { get; set; }
        /// <summary>
        /// Gets or sets the play length ticks.
        /// </summary>
        /// <value>
        /// The play length ticks.
        /// </value>
        public Int64 PlayLengthTicks { get; set; }

        //Add 18-02-2019
        /// <summary>
        /// Gets or sets the rewards.
        /// </summary>
        /// <value>
        /// The rewards.
        /// </value>
        public string Rewards { get; set; }

        /// <summary>
        /// Gets or sets the user profile.
        /// </summary>
        /// <value>The user profile.</value>
        public virtual UserProfileFormModel UserProfile { get; set; }
       // public virtual CampaignAuditFormModel CampaignAudit { get; set; }
    }
}