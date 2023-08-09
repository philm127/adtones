// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfilePressCommand.cs" company="Noat">
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
    /// Class CreateOrUpdateUserProfilePressCommand.
    /// </summary>
    public class CreateOrUpdateUserProfilePressCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the user profile press identifier.
        /// </summary>
        /// <value>The user profile press identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>The user profile identifier.</value>
        public int UserProfileId { get; set; }

        /// <summary>
        /// Gets or sets the local.
        /// </summary>
        /// <value>The local.</value>
        public string Local_Press { get; set; }

        /// <summary>
        /// Gets or sets the national.
        /// </summary>
        /// <value>The national.</value>
        public string National_Press { get; set; }

        /// <summary>
        /// Gets or sets the free newpapers.
        /// </summary>
        /// <value>The free newpapers.</value>
        public string FreeNewpapers_Press { get; set; }

        /// <summary>
        /// Gets or sets the magazines.
        /// </summary>
        /// <value>The magazines.</value>
        public string Magazines_Press { get; set; }
    }
}