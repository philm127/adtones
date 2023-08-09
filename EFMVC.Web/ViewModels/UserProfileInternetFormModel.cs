// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileInternetFormModel.cs" company="Noat">
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
    /// Class UserProfileInternetFormModel.
    /// </summary>
    public class UserProfileInternetFormModel : ArtharFormModel
    {
        /// <summary>
        /// Gets or sets the user profile internet identifier.
        /// </summary>
        /// <value>The user profile internet identifier.</value>
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
        /// Gets or sets the social networking.
        /// </summary>
        /// <value>The social networking.</value>
        [Display(Name = "Social Networking")]
        public string SocialNetworking_Internet { get; set; }

        /// <summary>
        /// Gets or sets the video.
        /// </summary>
        /// <value>The video.</value>
        [Display(Name = "Video")]
        public string Video_Internet { get; set; }

        /// <summary>
        /// Gets or sets the research.
        /// </summary>
        /// <value>The research.</value>
        [Display(Name = "Research")]
        public string Research_Internet { get; set; }

        /// <summary>
        /// Gets or sets the auctions.
        /// </summary>
        /// <value>The auctions.</value>
        [Display(Name = "Auctions")]
        public string Auctions_Internet { get; set; }

        /// <summary>
        /// Gets or sets the shopping.
        /// </summary>
        /// <value>The shopping.</value>
        [Display(Name = "Shopping")]
        public string Shopping_Internet { get; set; }
    }
}