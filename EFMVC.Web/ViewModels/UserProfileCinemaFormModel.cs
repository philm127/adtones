// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-22-2014
// ***********************************************************************
// <copyright file="UserProfileCinemaFormModel.cs" company="Noat">
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
    /// Class UserProfileCinemaFormModel.
    /// </summary>
    public class UserProfileCinemaFormModel
    {
        /// <summary>
        /// The _cinema
        /// </summary>
        private string _Cinema_Cinema;

        /// <summary>
        /// Gets or sets the user profile cinema identifier.
        /// </summary>
        /// <value>The user profile cinema identifier.</value>
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
        /// Gets or sets the cinema.
        /// </summary>
        /// <value>The cinema.</value>
        [Display(Name = "Cinema")]
        public string Cinema_Cinema
        {
            get
            {
                if (_Cinema_Cinema == null) return "A";
                return _Cinema_Cinema;
            }
            set { _Cinema_Cinema = value; }
        }
    }
}