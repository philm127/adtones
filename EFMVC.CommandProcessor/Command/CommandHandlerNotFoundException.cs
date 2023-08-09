// ***********************************************************************
// Assembly         : EFMVC.CommandProcessor
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CommandHandlerNotFoundException.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

/// <summary>
/// The Command namespace.
/// </summary>

namespace EFMVC.CommandProcessor.Command
{
    /// <summary>
    /// Class CommandHandlerNotFoundException.
    /// </summary>
    public class CommandHandlerNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandHandlerNotFoundException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public CommandHandlerNotFoundException(Type type)
            : base(string.Format("Command handler not found for command type: {0}", type))
        {
        }
    }
}