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
    public class UserProfileMobileAdvertiserFormModel
    {
        [Display(Name = "Mobile plan")]
        public string ContractType_Mobile { get; set; }

        [Display(Name = "Average Monthly Spend")]
        public string Spend_Mobile { get; set; }
    }
}