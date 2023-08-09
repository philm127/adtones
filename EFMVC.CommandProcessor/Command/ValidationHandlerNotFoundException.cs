// ***********************************************************************
// Assembly         : EFMVC.CommandProcessor
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="ValidationHandlerNotFoundException.cs" company="Noat">
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
    /// Class ValidationHandlerNotFoundException.
    /// </summary>
    public class ValidationHandlerNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationHandlerNotFoundException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public ValidationHandlerNotFoundException(Type type)
            : base(string.Format("Validation handler not found for command type: {0}", type))
        {
        }
    }
}