// ***********************************************************************
// Assembly         : EFMVC.CommandProcessor
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="ICommandResults.cs" company="Noat">
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
    /// Interface ICommandResults
    /// </summary>
    public interface ICommandResults
    {
        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <value>The results.</value>
        ICommandResult[] Results { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommandResults"/> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        bool Success { get; }
    }
}