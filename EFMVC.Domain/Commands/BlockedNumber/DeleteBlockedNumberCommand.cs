// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="DeleteBlockedNumberCommand.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;

/// <summary>
/// The BlockedNumber namespace.
/// </summary>

namespace EFMVC.Domain.Commands.BlockedNumber
{
    /// <summary>
    /// Class DeleteBlockedNumberCommand.
    /// </summary>
    public class DeleteBlockedNumberCommand : ICommand
    {
        /// <summary>
        /// The identifier
        /// </summary>
        public int Id;
    }
}