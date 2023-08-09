// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateProfileCommand.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using EFMVC.CommandProcessor.Command;

/// <summary>
/// The Commands namespace.
/// </summary>

namespace EFMVC.Domain.Commands
{
    /// <summary>
    /// Class CreateOrUpdateProfileCommand.
    /// </summary>
    public class CreateOrUpdateProfileCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>The user profile identifier.</value>
        public int UserProfileId { get; set; }

        /// <summary>
        /// Gets or sets the dob.
        /// </summary>
        /// <value>The dob.</value>
        public DateTime? DOB { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>The gender.</value>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the income bracket.
        /// </summary>
        /// <value>The income bracket.</value>
        public string IncomeBracket { get; set; }

        /// <summary>
        /// Gets or sets the working status.
        /// </summary>
        /// <value>The working status.</value>
        public string WorkingStatus { get; set; }

        /// <summary>
        /// Gets or sets the relationship status.
        /// </summary>
        /// <value>The relationship status.</value>
        public string RelationshipStatus { get; set; }

        /// <summary>
        /// Gets or sets the education.
        /// </summary>
        /// <value>The education.</value>
        public string Education { get; set; }

        /// <summary>
        /// Gets or sets the household status.
        /// </summary>
        /// <value>The household status.</value>
        public string HouseholdStatus { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>The location.</value>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the msisdn.
        /// </summary>
        /// <value>The msisdn.</value>
        public string MSISDN { get; set; }

        public string Age { get; set; }

        public string Postcode { get; set; }

        public int? CountryId { get; set; }
        public int OperatorId { get; set; }
        /// <summary>
        /// Gets or sets the user profile adverts.
        /// </summary>
        /// <value>The user profile adverts.</value>
        public ICollection<CreateOrUpdateUserProfileAdvertCommand> UserProfileAdverts { get; set; }

        /// <summary>
        /// Gets or sets the user profile attitudes.
        /// </summary>
        /// <value>The user profile attitudes.</value>
        public ICollection<CreateOrUpdateUserProfileAttitudeCommand> UserProfileAttitudes { get; set; }

        /// <summary>
        /// Gets or sets the user profile cinemas.
        /// </summary>
        /// <value>The user profile cinemas.</value>
        public ICollection<CreateOrUpdateUserProfileCinemaCommand> UserProfileCinemas { get; set; }

        /// <summary>
        /// Gets or sets the user profile internets.
        /// </summary>
        /// <value>The user profile internets.</value>
        public ICollection<CreateOrUpdateUserProfileInternetCommand> UserProfileInternets { get; set; }

        /// <summary>
        /// Gets or sets the user profile mobiles.
        /// </summary>
        /// <value>The user profile mobiles.</value>
        public ICollection<CreateOrUpdateUserProfileMobileCommand> UserProfileMobiles { get; set; }

        /// <summary>
        /// Gets or sets the user profile presses.
        /// </summary>
        /// <value>The user profile presses.</value>
        public ICollection<CreateOrUpdateUserProfilePressCommand> UserProfilePresses { get; set; }

        /// <summary>
        /// Gets or sets the user profile products services.
        /// </summary>
        /// <value>The user profile products services.</value>
        public ICollection<CreateOrUpdateUserProfileProductsServiceCommand> UserProfileProductsServices { get; set; }

        /// <summary>
        /// Gets or sets the user profile radios.
        /// </summary>
        /// <value>The user profile radios.</value>
        public ICollection<CreateOrUpdateUserProfileRadioCommand> UserProfileRadios { get; set; }

        /// <summary>
        /// Gets or sets the user profile time settings.
        /// </summary>
        /// <value>The user profile time settings.</value>
        public ICollection<CreateOrUpdateUserProfileTimeSettingCommand> UserProfileTimeSettings { get; set; }

        /// <summary>
        /// Gets or sets the user profile TVS.
        /// </summary>
        /// <value>The user profile TVS.</value>
        public ICollection<CreateOrUpdateUserProfileTvCommand> UserProfileTvs { get; set; }
    }
}