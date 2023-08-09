// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileTimeSettingCommand.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;

/// <summary>
/// The Commands namespace.
/// </summary>

namespace EFMVC.Domain.Commands
{
    /// <summary>
    /// Class CreateOrUpdateUserProfileTimeSettingCommand.
    /// </summary>
    public class CreateOrUpdateUserProfileTimeSettingCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the user profile time settings identifier.
        /// </summary>
        /// <value>The user profile time settings identifier.</value>
        public int UserProfileTimeSettingsId { get; set; }

        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>The user profile identifier.</value>
        public int UserProfileId { get; set; }
        public int OperatorId { get; set; }
        /// <summary>
        /// Gets or sets the monday posted times.
        /// </summary>
        /// <value>The monday posted times.</value>
        public PostedTimes MondayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the tuesday posted times.
        /// </summary>
        /// <value>The tuesday posted times.</value>
        public PostedTimes TuesdayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the wednesday posted times.
        /// </summary>
        /// <value>The wednesday posted times.</value>
        public PostedTimes WednesdayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the thursday posted times.
        /// </summary>
        /// <value>The thursday posted times.</value>
        public PostedTimes ThursdayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the friday posted times.
        /// </summary>
        /// <value>The friday posted times.</value>
        public PostedTimes FridayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the saturday posted times.
        /// </summary>
        /// <value>The saturday posted times.</value>
        public PostedTimes SaturdayPostedTimes { get; set; }

        /// <summary>
        /// Gets or sets the sunday posted times.
        /// </summary>
        /// <value>The sunday posted times.</value>
        public PostedTimes SundayPostedTimes { get; set; }
    }

    /// <summary>
    /// Class PostedTimes.
    /// </summary>
    public class PostedTimes
    {
        /// <summary>
        /// Gets or sets the day ids.
        /// </summary>
        /// <value>The day ids.</value>
        public string[] DayIds { get; set; }
    }
}