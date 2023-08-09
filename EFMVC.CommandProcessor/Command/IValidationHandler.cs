// ***********************************************************************
// Assembly         : EFMVC.CommandProcessor
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="IValidationHandler.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using EFMVC.Core.Common;

/// <summary>
/// The Command namespace.
/// </summary>

namespace EFMVC.CommandProcessor.Command
{
    /// <summary>
    /// Interface IValidationHandler
    /// </summary>
    /// <typeparam name="TCommand">The type of the t command.</typeparam>
    public interface IValidationHandler<in TCommand> where TCommand : ICommand
    {
        /// <summary>
        /// Validates the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IEnumerable&lt;ValidationResult&gt;.</returns>
        IEnumerable<ValidationResult> Validate(TCommand command);
    }
}