// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileCreditsReceivedCommand.cs" company="Noat">
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
    /// Class CreateOrUpdateUserProfileCreditsReceivedCommand.
    /// </summary>
    public class CreateOrUpdateUserProfileCreditsReceivedCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>The user profile identifier.</value>
        public int UserProfileId { get; set; }

        /// <summary>
        /// Gets or sets the total credits.
        /// </summary>
        /// <value>The total credits.</value>
        public string TotalCredits { get; set; }

        /// <summary>
        /// Gets or sets the last month credits.
        /// </summary>
        /// <value>The last month credits.</value>
        public string LastMonthCredits { get; set; }

        /// <summary>
        /// Gets or sets the current month credits.
        /// </summary>
        /// <value>The current month credits.</value>
        public string CurrentMonthCredits { get; set; }
    }
}