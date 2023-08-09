// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileCreditsReceievedFormModel.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class UserProfileCreditsReceivedFormModel.
    /// </summary>
    public class UserProfileCreditsReceivedFormModel
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
        /// Gets or sets the total credits.
        /// </summary>
        /// <value>The total credits.</value>
        public string TotalCredits { get; set; }

        /// <summary>
        /// Gets or sets the last month credits.
        /// </summary>
        /// <value>The last month credits.</value>
        public string LastMonthCredits { get; set; }

        /// <summary>
        /// Gets or sets the current month credits.
        /// </summary>
        /// <value>The current month credits.</value>
        public string CurrentMonthCredits { get; set; }

        /// <summary>
        /// Gets or sets the user profile.
        /// </summary>
        /// <value>The user profile.</value>
        public virtual UserProfileFormModel UserProfile { get; set; }
    }
}