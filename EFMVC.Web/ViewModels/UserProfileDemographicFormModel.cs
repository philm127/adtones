// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileMobileFormModel.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class UserProfileMobileFormModel.
    /// </summary>
    public class UserProfileDemographicFormModel
    {
        [Key]
        public int Id { get; set; }

        public int UserProfileId { get; set; }


        public int UserId { get; set; }

        public DateTime? DOB { get; set; }

        public string MSISDN { get; set; }

        public UserProfileFormModel UserProfile { get; set; }

        [Display(Name = "Location")]
        public string Location_Demographics { get; set; }
        public IEnumerable<SelectListItem> LocationList { get; set; }

        [Display(Name = "Age")]
        public string Age_Demographics { get; set; }
        public IEnumerable<SelectListItem> AgeList { get; set; }

        [Display(Name = "Gender")]
        public string Gender_Demographics { get; set; }
        public IEnumerable<SelectListItem> GenderList { get; set; }

        [Display(Name = "HouseholdStatus")]
        public string HouseholdStatus_Demographics { get; set; }
        public IEnumerable<SelectListItem> HouseholdStatusList { get; set; }

        [Display(Name = "RelationshipStatus")]
        public string RelationshipStatus_Demographics { get; set; }
        public IEnumerable<SelectListItem> RelationshipStatusList { get; set; }

        [Display(Name = "IncomeBracket")]
        public string IncomeBracket_Demographics { get; set; }
        public IEnumerable<SelectListItem> IncomeBracketList { get; set; }

        [Display(Name = "Education")]
        public string Education_Demographics { get; set; }
        public IEnumerable<SelectListItem> EducationList { get; set; }

        [Display(Name = "WorkingStatus")]
        public string WorkingStatus_Demographics { get; set; }
        public IEnumerable<SelectListItem> WorkingStatusList { get; set; }

        public string Location { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string HouseholdStatus { get; set; }
        public string RelationshipStatus { get; set; }
        public string IncomeBracket { get; set; }
        public string Education { get; set; }
        public string WorkingStatus { get; set; }

        public bool DisplayLocation { get; set; }
        public bool DisplayAge { get; set; }
        public bool DisplayGender { get; set; }
        public bool DisplayHouseholdStatus { get; set; }
        public bool DisplayRelationshipStatus { get; set; }
        public bool DisplayIncomeBracket { get; set; }
        public bool DisplayEducation { get; set; }
        public bool DisplayWorkingStatus { get; set; }
    }
}