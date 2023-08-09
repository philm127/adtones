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
    public class UserProfileDemographicAdvertiserFormModel
    {
        public string Location_Demographics { get; set; }

        public string Age_Demographics { get; set; }

        public string Gender_Demographics { get; set; }

        public string HouseholdStatus_Demographics { get; set; }

        public string RelationshipStatus_Demographics { get; set; }

        public string IncomeBracket_Demographics { get; set; }

        public string Education_Demographics { get; set; }

        public string WorkingStatus_Demographics { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Age")]
        public string Age { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "HouseholdStatus")]
        public string HouseholdStatus { get; set; }

        [Display(Name = "RelationshipStatus")]
        public string RelationshipStatus { get; set; }

        [Display(Name = "IncomeBracket")]
        public string IncomeBracket { get; set; }

        [Display(Name = "Education")]
        public string Education { get; set; }

        [Display(Name = "WorkingStatus")]
        public string WorkingStatus { get; set; }
    }
}