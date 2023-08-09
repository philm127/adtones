// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="DeleteProfileCommand.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;

/// <summary>
/// The Profile namespace.
/// </summary>

namespace EFMVC.Domain.Commands.Profile
{
    /// <summary>
    /// Class DeleteProfileCommand.
    /// </summary>
    public class DeleteProfileCommand : ICommand
    {
        /// <summary>
        /// The identifier
        /// </summary>
        public int Id;
    }
}