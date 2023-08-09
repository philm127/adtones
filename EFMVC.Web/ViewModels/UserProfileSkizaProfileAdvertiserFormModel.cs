// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-23-2014
// ***********************************************************************
// <copyright file="UserProfileAdvertFormModel.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class UserProfileAdvertFormModel.
    /// </summary>
    public class UserProfileSkizaProfileAdvertiserFormModel
    {
        [Display(Name = "Hustlers")]
        public string Hustlers_AdType { get; set; }

        [Display(Name = "Youth")]
        public string Youth_AdType { get; set; }

        [Display(Name = "Discerning Professionals")]
        public string DiscerningProfessionals_AdType { get; set; }

        [Display(Name = "Mass")]
        public string Mass_AdType { get; set; }
    }
}