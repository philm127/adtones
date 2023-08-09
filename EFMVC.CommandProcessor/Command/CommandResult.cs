// ***********************************************************************
// Assembly         : EFMVC.CommandProcessor
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CommandResult.cs" company="Noat">
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
    /// Class CommandResult.
    /// </summary>
    public class CommandResult : ICommandResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult"/> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        public CommandResult(bool success)
        {
            Success = success;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResult"/> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="id">The identifier.</param>
        public CommandResult(bool success, int id)
        {
            Success = success;
            Id = id;
        }

        #region ICommandResult Members

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CommandResult"/> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; protected set; }

        #endregion
    }
}