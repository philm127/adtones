// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileTimeSettingFormModel.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class UserProfileTimeSettingFormModel.
    /// </summary>
    public class UserProfileTimeSettingFormModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileTimeSettingFormModel"/> class.
        /// </summary>
        public UserProfileTimeSettingFormModel()
        {
            MondayPostedTimes = new PostedTimesModel();
            MondaySelectedTimes = new List<TimeOfDay>();
            TuesdayPostedTimes = new PostedTimesModel();
            TuesdaySelectedTimes = new List<TimeOfDay>();
            WednesdayPostedTimes = new PostedTimesModel();
            WednesdaySelectedTimes = new List<TimeOfDay>();
            ThursdayPostedTimes = new PostedTimesModel();
            ThursdaySelectedTimes = new List<TimeOfDay>();
            FridayPostedTimes = new PostedTimesModel();
            FridaySelectedTimes = new List<TimeOfDay>();
            SaturdayPostedTimes = new PostedTimesModel();
            SaturdaySelectedTimes = new List<TimeOfDay>();
            SundayPostedTimes = new PostedTimesModel();
            SundaySelectedTimes = new List<TimeOfDay>();
        }

        /// <summary>
        /// Gets or sets the user profile time settings identifier.
        /// </summary>
        /// <value>The user profile time settings identifier.</value>
        [Key]
        public int UserProfileTimeSettingsId { get; set; }

        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>The user profile identifier.</value>
        public int UserProfileId { get; set; }
        public int OperatorId { get; set; }
        /// <summary>
        /// Gets or sets the user profile.
        /// </summary>
        /// <value>The user profile.</value>
        public UserProfileFormModel UserProfile { get; set; }

        /// <summary>
        /// Gets or sets the available times.
        /// </summary>
        /// <value>The available times.</value>
        public IEnumerable<TimeOfDay> AvailableTimes { get; set; }

        /// <summary>
        /// Gets or sets the monday selected times.
        /// </summary>
        /// <value>The monday selected times.</value>
        public IEnumerable<TimeOfDay> MondaySelectedTimes { get; set; }

        /// <summary>
        /// Gets or sets the monday posted times.
        /// </summary>
        /// <value>The monday posted times.</value>
        public PostedTimesModel MondayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the tuesday selected times.
        /// </summary>
        /// <value>The tuesday selected times.</value>
        public IEnumerable<TimeOfDay> TuesdaySelectedTimes { get; set; }

        /// <summary>
        /// Gets or sets the tuesday posted times.
        /// </summary>
        /// <value>The tuesday posted times.</value>
        public PostedTimesModel TuesdayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the wednesday selected times.
        /// </summary>
        /// <value>The wednesday selected times.</value>
        public IEnumerable<TimeOfDay> WednesdaySelectedTimes { get; set; }

        /// <summary>
        /// Gets or sets the wednesday posted times.
        /// </summary>
        /// <value>The wednesday posted times.</value>
        public PostedTimesModel WednesdayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the thursday selected times.
        /// </summary>
        /// <value>The thursday selected times.</value>
        public IEnumerable<TimeOfDay> ThursdaySelectedTimes { get; set; }

        /// <summary>
        /// Gets or sets the thursday posted times.
        /// </summary>
        /// <value>The thursday posted times.</value>
        public PostedTimesModel ThursdayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the friday selected times.
        /// </summary>
        /// <value>The friday selected times.</value>
        public IEnumerable<TimeOfDay> FridaySelectedTimes { get; set; }

        /// <summary>
        /// Gets or sets the friday posted times.
        /// </summary>
        /// <value>The friday posted times.</value>
        public PostedTimesModel FridayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the saturday selected times.
        /// </summary>
        /// <value>The saturday selected times.</value>
        public IEnumerable<TimeOfDay> SaturdaySelectedTimes { get; set; }

        /// <summary>
        /// Gets or sets the saturday posted times.
        /// </summary>
        /// <value>The saturday posted times.</value>
        public PostedTimesModel SaturdayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the sunday selected times.
        /// </summary>
        /// <value>The sunday selected times.</value>
        public IEnumerable<TimeOfDay> SundaySelectedTimes { get; set; }

        /// <summary>
        /// Gets or sets the sunday posted times.
        /// </summary>
        /// <value>The sunday posted times.</value>
        public PostedTimesModel SundayPostedTimes { get; set; }

        //Add 27-02-2019
        public int UserId { get; set; }
    }
}