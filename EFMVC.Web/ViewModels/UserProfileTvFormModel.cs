// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-23-2014
// ***********************************************************************
// <copyright file="UserProfileTvFormModel.cs" company="Noat">
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
    /// Class UserProfileTvFormModel.
    /// </summary>
    public class UserProfileTvFormModel
    {
        /// <summary>
        /// The _cable
        /// </summary>
        private string _Cable_TV;

        /// <summary>
        /// The _internet
        /// </summary>
        private string _Internet_TV;

        /// <summary>
        /// The _satallite
        /// </summary>
        private string _Satallite_TV;

        /// <summary>
        /// The _terrestrial
        /// </summary>
        private string _Terrestrial_TV;

        /// <summary>
        /// Gets or sets the user profile tv identifier.
        /// </summary>
        /// <value>The user profile tv identifier.</value>
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
        /// Gets or sets the satallite.
        /// </summary>
        /// <value>The satallite.</value>
        public string Satallite_TV
        {
            get
            {
                if (_Satallite_TV == null) return "A";
                return _Satallite_TV;
            }
            set { _Satallite_TV = value; }
        }

        /// <summary>
        /// Gets or sets the cable.
        /// </summary>
        /// <value>The cable.</value>
        public string Cable_TV
        {
            get
            {
                if (_Cable_TV == null) return "A";
                return _Cable_TV;
            }
            set { _Cable_TV = value; }
        }

        /// <summary>
        /// Gets or sets the terrestrial.
        /// </summary>
        /// <value>The terrestrial.</value>
        public string Terrestrial_TV
        {
            get
            {
                if (_Terrestrial_TV == null) return "A";
                return _Terrestrial_TV;
            }
            set { _Terrestrial_TV = value; }
        }

        /// <summary>
        /// Gets or sets the internet.
        /// </summary>
        /// <value>The internet.</value>
        public string Internet_TV
        {
            get
            {
                if (_Internet_TV == null) return "A";
                return _Internet_TV;
            }
            set { _Internet_TV = value; }
        }
    }
}