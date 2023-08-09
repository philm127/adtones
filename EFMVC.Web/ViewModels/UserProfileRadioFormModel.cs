// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-23-2014
// ***********************************************************************
// <copyright file="UserProfileRadioFormModel.cs" company="Noat">
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
    /// Class UserProfileRadioFormModel.
    /// </summary>
    public class UserProfileRadioFormModel
    {
        /// <summary>
        /// The _local
        /// </summary>
        private string _Local_Radio;

        /// <summary>
        /// The _music
        /// </summary>
        private string _Music_Radio;

        /// <summary>
        /// The _national
        /// </summary>
        private string _National_Radio;

        /// <summary>
        /// The _sport
        /// </summary>
        private string _Sport_Radio;

        /// <summary>
        /// The _talk
        /// </summary>
        private string _Talk_Radio;

        /// <summary>
        /// Gets or sets the user profile radio identifier.
        /// </summary>
        /// <value>The user profile radio identifier.</value>
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
        /// Gets or sets the national.
        /// </summary>
        /// <value>The national.</value>
        [Display(Name = "National")]
        public string National_Radio
        {
            get
            {
                if (_National_Radio == null) return "A";
                return _National_Radio;
            }
            set { _National_Radio = value; }
        }

        /// <summary>
        /// Gets or sets the local.
        /// </summary>
        /// <value>The local.</value>
        [Display(Name = "Local")]
        public string Local_Radio
        {
            get
            {
                if (_Local_Radio == null) return "A";
                return _Local_Radio;
            }
            set { _Local_Radio = value; }
        }

        /// <summary>
        /// Gets or sets the music.
        /// </summary>
        /// <value>The music.</value>
        [Display(Name = "Music")]
        public string Music_Radio
        {
            get
            {
                if (_Music_Radio == null) return "A";
                return _Music_Radio;
            }
            set { _Music_Radio = value; }
        }

        /// <summary>
        /// Gets or sets the sport.
        /// </summary>
        /// <value>The sport.</value>
        [Display(Name = "Sport")]
        public string Sport_Radio
        {
            get
            {
                if (_Sport_Radio == null) return "A";
                return _Sport_Radio;
            }
            set { _Sport_Radio = value; }
        }

        /// <summary>
        /// Gets or sets the talk.
        /// </summary>
        /// <value>The talk.</value>
        [Display(Name = "Talk")]
        public string Talk_Radio
        {
            get
            {
                if (_Talk_Radio == null) return "A";
                return _Talk_Radio;
            }
            set { _Talk_Radio = value; }
        }
    }
}