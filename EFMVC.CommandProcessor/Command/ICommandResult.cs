// ***********************************************************************
// Assembly         : EFMVC.CommandProcessor
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="ICommandResult.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/// <summary>
/// The Command namespace.
/// </summary>

namespace EFMVC.CommandProcessor.Command
{
    /// <summary>
    /// Interface ICommandResult
    /// </summary>
    public interface ICommandResult
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommandResult"/> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        bool Success { get; }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        int Id { get; }
    }
}