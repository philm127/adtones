// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileAdvertsReceivedCommand.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using EFMVC.CommandProcessor.Command;

/// <summary>
/// The Commands namespace.
/// </summary>

namespace EFMVC.Domain.Commands
{
    /// <summary>
    /// Class CreateOrUpdateUserProfileAdvertsReceivedCommand.
    /// </summary>
    public class CreateOrUpdateUserProfileAdvertsReceivedCommand : ICommand
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
        /// Gets or sets the advert reference.
        /// </summary>
        /// <value>The advert reference.</value>
        public string AdvertRef { get; set; }

        /// <summary>
        /// Gets or sets the name of the advert.
        /// </summary>
        /// <value>The name of the advert.</value>
        public string AdvertName { get; set; }

        /// <summary>
        /// Gets or sets the brand.
        /// </summary>
        /// <value>The brand.</value>
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the date time played.
        /// </summary>
        /// <value>The date time played.</value>
        public DateTime DateTimePlayed { get; set; }

        /// <summary>
        /// Gets or sets the credits received.
        /// </summary>
        /// <value>The credits received.</value>
        public string CreditsReceived { get; set; }
    }
}