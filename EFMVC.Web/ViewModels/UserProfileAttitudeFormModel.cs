// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileAttitudeFormModel.cs" company="Noat">
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
    /// Class UserProfileAttitudeFormModel.
    /// </summary>
    public class UserProfileAttitudeFormModel
    {
        /// <summary>
        /// Gets or sets the user profile attitude identifier.
        /// </summary>
        /// <value>The user profile attitude identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>The user profile identifier.</value>
        public int UserProfileId { get; set; }

        /// <summary>
        /// Gets or sets the user profile.
        /// </summary>
        /// <value>The user profile.</value>
        public UserProfileFormModel UserProfile { get; set; }

        /// <summary>
        /// Gets or sets the fitness.
        /// </summary>
        /// <value>The fitness.</value>
        [Display(Name = "Fitness")]
        public string Fitness_Attitude { get; set; }

        /// <summary>
        /// Gets or sets the holidays.
        /// </summary>
        /// <value>The holidays.</value>
        [Display(Name = "Holidays")]
        public string Holidays_Attitude { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        [Display(Name = "Environment")]
        public string Environment_Attitude { get; set; }

        /// <summary>
        /// Gets or sets the going out.
        /// </summary>
        /// <value>The going out.</value>
        [Display(Name = "Going Out")]
        public string GoingOut_Attitude { get; set; }

        /// <summary>
        /// Gets or sets the financial stabiity.
        /// </summary>
        /// <value>The financial stabiity.</value>
        [Display(Name = "Financial Stabiity")]
        public string FinancialStabiity_Attitude { get; set; }

        /// <summary>
        /// Gets or sets the religion.
        /// </summary>
        /// <value>The religion.</value>
        [Display(Name = "Religion")]
        public string Religion_Attitude { get; set; }

        /// <summary>
        /// Gets or sets the fashion.
        /// </summary>
        /// <value>The fashion.</value>
        [Display(Name = "Fashion")]
        public string Fashion_Attitude { get; set; }

        /// <summary>
        /// Gets or sets the music.
        /// </summary>
        /// <value>The music.</value>
        [Display(Name = "Music")]
        public string Music_Attitude { get; set; }
    }
}