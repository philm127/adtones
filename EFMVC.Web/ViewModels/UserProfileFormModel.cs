// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-22-2014
// ***********************************************************************
// <copyright file="UserProfileFormModel.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class UserProfileFormModel. This class cannot be inherited.
    /// </summary>
    public sealed class UserProfileFormModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileFormModel"/> class.
        /// </summary>
        public UserProfileFormModel()
        {
            UserProfileAttitudes = new HashSet<UserProfileAttitudeFormModel>();
            UserProfileAdverts = new HashSet<UserProfileAdvertFormModel>();
            UserProfileCinemas = new HashSet<UserProfileCinemaFormModel>();
            UserProfileInternets = new HashSet<UserProfileInternetFormModel>();
            UserProfileMobiles = new HashSet<UserProfileMobileFormModel>();
            UserProfilePresses = new HashSet<UserProfilePressFormModel>();
            UserProfileProductsServices = new HashSet<UserProfileProductsServiceFormModel>();
            UserProfileRadios = new HashSet<UserProfileRadioFormModel>();
            UserProfileTimeSettings = new HashSet<UserProfileTimeSettingFormModel>();
            UserProfileTvs = new HashSet<UserProfileTvFormModel>();
        }

        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>The user profile identifier.</value>
        public int UserProfileId { get; set; }

        /// <summary>
        /// Gets or sets the dob.
        /// </summary>
        /// <value>The dob.</value>
        [DataType(DataType.Date)]
        [DisplayName("Date of Birth")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>The gender.</value>
        [DataType(DataType.Text)]
        [DisplayName("Gender")]
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the gender list.
        /// </summary>
        /// <value>The gender list.</value>
        public IEnumerable<SelectListItem> GenderList { get; set; }

        [DataType(DataType.Text)]
        [DisplayName("Location")]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the gender list.
        /// </summary>
        /// <value>The gender list.</value>
        public IEnumerable<SelectListItem> LocationList { get; set; }

        /// <summary>
        /// Gets or sets the income bracket.
        /// </summary>
        /// <value>The income bracket.</value>
        [DataType(DataType.Text)]
        [DisplayName("Income Bracket")]
        public string IncomeBracket { get; set; }

        /// <summary>
        /// Gets or sets the income bracket list.
        /// </summary>
        /// <value>The income bracket list.</value>
        public IEnumerable<SelectListItem> IncomeBracketList { get; set; }

        /// <summary>
        /// Gets or sets the working status.
        /// </summary>
        /// <value>The working status.</value>
        [DataType(DataType.Text)]
        [DisplayName("Working Status")]
        public string WorkingStatus { get; set; }

        /// <summary>
        /// Gets or sets the working status list.
        /// </summary>
        /// <value>The working status list.</value>
        public IEnumerable<SelectListItem> WorkingStatusList { get; set; }

        /// <summary>
        /// Gets or sets the relationship status.
        /// </summary>
        /// <value>The relationship status.</value>
        [DataType(DataType.Text)]
        [DisplayName("Relationship Status")]
        public string RelationshipStatus { get; set; }

        /// <summary>
        /// Gets or sets the relationship status list.
        /// </summary>
        /// <value>The relationship status list.</value>
        public IEnumerable<SelectListItem> RelationshipStatusList { get; set; }

        /// <summary>
        /// Gets or sets the education.
        /// </summary>
        /// <value>The education.</value>
        [DataType(DataType.Text)]
        [DisplayName("Education")]
        public string Education { get; set; }

        /// <summary>
        /// Gets or sets the education list.
        /// </summary>
        /// <value>The education list.</value>
        public IEnumerable<SelectListItem> EducationList { get; set; }

        /// <summary>
        /// Gets or sets the household status.
        /// </summary>
        /// <value>The household status.</value>
        [DataType(DataType.Text)]
        [DisplayName("Household Status")]
        public string HouseholdStatus { get; set; }

        /// <summary>
        /// Gets or sets the household status list.
        /// </summary>
        /// <value>The household status list.</value>
        public IEnumerable<SelectListItem> HouseholdStatusList { get; set; }

        public bool DisplayLocation { get; set; }
        public bool DisplayGender { get; set; }
        public bool DisplayHouseholdStatus { get; set; }
        public bool DisplayWorkingStatus { get; set; }
        public bool DisplayRelationshipStatus { get; set; }
        public bool DisplayEducation { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        /// 
        //code commented on 30-03-2017
        //[DataType(DataType.Text)]
        //[DisplayName("Location")]
        //public string Location { get; set; }

        //Code added on 30-03-2017
        [RegularExpression(@"^[0-9a-zA-Z''-'\s]{1,40}$", ErrorMessage = "Special characters are not allowed.")]
        public string Postcode { get; set; }

        /// <summary>
        /// Gets or sets the location list.
        /// </summary>
        /// <value>The location list.</value>
        /// 
        //code commented on 30-03-2017
        //public IEnumerable<SelectListItem> LocationList { get; set; }

        /// <summary>
        /// Gets or sets the msisdn.
        /// </summary>
        /// <value>The msisdn.</value>
        [DataType(DataType.Text)]
        [DisplayName("MSISDN")]
        public string MSISDN { get; set; }


        [DataType(DataType.Text)]
        [DisplayName("Email")]
        //[Required]
        [EmailAddress]
        public string Email { get; set; }

        //public string Age { get; set; }

        /// <summary>
        /// Gets or sets the user profile adverts.
        /// </summary>
        /// <value>The user profile adverts.</value>
        public ICollection<UserProfileAdvertFormModel> UserProfileAdverts { get; set; }

        /// <summary>
        /// Gets or sets the user profile attitudes.
        /// </summary>
        /// <value>The user profile attitudes.</value>
        public ICollection<UserProfileAttitudeFormModel> UserProfileAttitudes { get; set; }

        /// <summary>
        /// Gets or sets the user profile cinemas.
        /// </summary>
        /// <value>The user profile cinemas.</value>
        public ICollection<UserProfileCinemaFormModel> UserProfileCinemas { get; set; }

        /// <summary>
        /// Gets or sets the user profile internets.
        /// </summary>
        /// <value>The user profile internets.</value>
        public ICollection<UserProfileInternetFormModel> UserProfileInternets { get; set; }

        /// <summary>
        /// Gets or sets the user profile mobiles.
        /// </summary>
        /// <value>The user profile mobiles.</value>
        public ICollection<UserProfileMobileFormModel> UserProfileMobiles { get; set; }

        /// <summary>
        /// Gets or sets the user profile presses.
        /// </summary>
        /// <value>The user profile presses.</value>
        public ICollection<UserProfilePressFormModel> UserProfilePresses { get; set; }

        /// <summary>
        /// Gets or sets the user profile products services.
        /// </summary>
        /// <value>The user profile products services.</value>
        public ICollection<UserProfileProductsServiceFormModel> UserProfileProductsServices { get; set; }

        /// <summary>
        /// Gets or sets the user profile radios.
        /// </summary>
        /// <value>The user profile radios.</value>
        public ICollection<UserProfileRadioFormModel> UserProfileRadios { get; set; }

        /// <summary>
        /// Gets or sets the user profile time settings.
        /// </summary>
        /// <value>The user profile time settings.</value>
        public ICollection<UserProfileTimeSettingFormModel> UserProfileTimeSettings { get; set; }

        /// <summary>
        /// Gets or sets the user profile TVS.
        /// </summary>
        /// <value>The user profile TVS.</value>
        public ICollection<UserProfileTvFormModel> UserProfileTvs { get; set; }

        /// <summary>
        /// Gets or sets the user profile adverts received.
        /// </summary>
        /// <value>The user profile adverts received.</value>
        public ICollection<UserProfileAdvertsReceivedFromModel> UserProfileAdvertsReceived { get; set; }

        /// <summary>
        /// Gets or sets the user profile credits received.
        /// </summary>
        /// <value>The user profile credits received.</value>
        public ICollection<UserProfileCreditsReceivedFormModel> UserProfileCreditsReceived { get; set; }
    }
}