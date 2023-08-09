// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-22-2014
// ***********************************************************************
// <copyright file="SkizaProfileFormModel.cs" company="Noat">
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
    /// Class SkizaProfileFormModel.
    /// </summary>
    public class SkizaProfileFormModel
    {
        /// <summary>
        /// The _cinema
        /// </summary>
        private string _Hustlers_AdType;
        private string _Youth_AdType;
        private string _DiscerningProfessionals_AdType;
        private string _Mass_AdType;


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

        //Add 27-02-2019
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id.</value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the cinema.
        /// </summary>
        /// <value>The cinema.</value>
        [Display(Name = "Hustlers")]
        public string Hustlers_AdType
        {
            get
            {
                if (_Hustlers_AdType == null) return "B";
                return _Hustlers_AdType;
            }
            set { _Hustlers_AdType = value; }
        }

        [Display(Name = "Youth")]
        public string Youth_AdType
        {
            get
            {
                if (_Youth_AdType == null) return "B";
                return _Youth_AdType;
            }
            set { _Youth_AdType = value; }
        }

        [Display(Name = "Discerning Professionals")]
        public string DiscerningProfessionals_AdType
        {
            get
            {
                if (_DiscerningProfessionals_AdType == null) return "B";
                return _DiscerningProfessionals_AdType;
            }
            set { _DiscerningProfessionals_AdType = value; }
        }

        [Display(Name = "Mass")]
        public string Mass_AdType
        {
            get
            {
                if (_Mass_AdType == null) return "B";
                return _Mass_AdType;
            }
            set { _Mass_AdType = value; }
        }
    }
}