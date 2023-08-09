// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="ChangeVerificationStatusCommand.cs" company="Noat">
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
    /// Class ChangeVerificationStatusCommand.
    /// </summary>
    public class ChangeVerificationStatusCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [verification status].
        /// </summary>
        /// <value><c>true</c> if [verification status]; otherwise, <c>false</c>.</value>
        public bool VerificationStatus { get; set; }

        public bool IsEmailVerfication { get; set; }
    }
}