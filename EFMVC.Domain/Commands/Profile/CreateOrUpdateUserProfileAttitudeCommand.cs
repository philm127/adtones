// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileAttitudeCommand.cs" company="Noat">
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
    /// Class CreateOrUpdateUserProfileAttitudeCommand.
    /// </summary>
    public class CreateOrUpdateUserProfileAttitudeCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the user profile attitude identifier.
        /// </summary>
        /// <value>The user profile attitude identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>The user profile identifier.</value>
        public int UserProfileId { get; set; }

        /// <summary>
        /// Gets or sets the fitness.
        /// </summary>
        /// <value>The fitness.</value>
        public string Fitness_Attitude { get; set; }

        /// <summary>
        /// Gets or sets the holidays.
        /// </summary>
        /// <value>The holidays.</value>
        public string Holidays_Attitude { get; set; }

        /// <summary>
        /// Gets or sets the environment.
        /// </summary>
        /// <value>The environment.</value>
        public string Environment_Attitude { get; set; }

        /// <summary>
        /// Gets or sets the going out.
        /// </summary>
        /// <value>The going out.</value>
        public string GoingOut_Attitude { get; set; }

        /// <summary>
        /// Gets or sets the financial stabiity.
        /// </summary>
        /// <value>The financial stabiity.</value>
        public string FinancialStabiity_Attitude { get; set; }

        /// <summary>
        /// Gets or sets the religion.
        /// </summary>
        /// <value>The religion.</value>
        public string Religion_Attitude { get; set; }

        /// <summary>
        /// Gets or sets the fashion.
        /// </summary>
        /// <value>The fashion.</value>
        public string Fashion_Attitude { get; set; }

        /// <summary>
        /// Gets or sets the music.
        /// </summary>
        /// <value>The music.</value>
        public string Music_Attitude { get; set; }
    }
}