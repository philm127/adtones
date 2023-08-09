// ***********************************************************************
// Assembly         : EFMVC.CommandProcessor
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="ICommandBus.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using EFMVC.CommandProcessor.Command;
using EFMVC.Core.Common;

/// <summary>
/// The Dispatcher namespace.
/// </summary>

namespace EFMVC.CommandProcessor.Dispatcher
{
    /// <summary>
    /// Interface ICommandBus
    /// </summary>
    public interface ICommandBus
    {
        /// <summary>
        /// Submits the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the t command.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        ICommandResult Submit<TCommand>(TCommand command) where TCommand : ICommand;

        /// <summary>
        /// Validates the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the t command.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>IEnumerable&lt;ValidationResult&gt;.</returns>
        IEnumerable<ValidationResult> Validate<TCommand>(TCommand command) where TCommand : ICommand;
    }
}