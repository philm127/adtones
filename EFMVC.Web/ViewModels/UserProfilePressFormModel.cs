// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-23-2014
// ***********************************************************************
// <copyright file="UserProfilePressFormModel.cs" company="Noat">
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
    /// Class UserProfilePressFormModel.
    /// </summary>
    public class UserProfilePressFormModel
    {
        /// <summary>
        /// The _free newpapers
        /// </summary>
        private string _FreeNewpapers_Press;

        /// <summary>
        /// The _local
        /// </summary>
        private string _Local_Presss;

        /// <summary>
        /// The _magazines
        /// </summary>
        private string _Magazines_Press;

        /// <summary>
        /// The _national
        /// </summary>
        private string _National_Press;

        /// <summary>
        /// Gets or sets the user profile press identifier.
        /// </summary>
        /// <value>The user profile press identifier.</value>
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
        /// Gets or sets the local.
        /// </summary>
        /// <value>The local.</value>
        public string Local_Press
        {
            get
            {
                if (_Local_Presss == null) return "B";
                return _Local_Presss;
            }
            set { _Local_Presss = value; }
        }

        /// <summary>
        /// Gets or sets the national.
        /// </summary>
        /// <value>The national.</value>
        public string National_Press
        {
            get
            {
                if (_National_Press == null) return "B";
                return _National_Press;
            }
            set { _National_Press = value; }
        }

        /// <summary>
        /// Gets or sets the free newpapers.
        /// </summary>
        /// <value>The free newpapers.</value>
        public string FreeNewpapers_Press
        {
            get
            {
                if (_FreeNewpapers_Press == null) return "B";
                return _FreeNewpapers_Press;
            }
            set { _FreeNewpapers_Press = value; }
        }

        /// <summary>
        /// Gets or sets the magazines.
        /// </summary>
        /// <value>The magazines.</value>
        public string Magazines_Press
        {
            get
            {
                if (_Magazines_Press == null) return "B";
                return _Magazines_Press;
            }
            set { _Magazines_Press = value; }
        }
    }
}