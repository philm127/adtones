// ***********************************************************************
// Assembly         : EFMVC.CommandProcessor
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="DefaultCommandBus.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Web.Mvc;
using EFMVC.CommandProcessor.Command;
using EFMVC.Core.Common;

/// <summary>
/// The Dispatcher namespace.
/// </summary>

namespace EFMVC.CommandProcessor.Dispatcher
{
    /// <summary>
    /// Class DefaultCommandBus.
    /// </summary>
    public class DefaultCommandBus : ICommandBus
    {
        #region ICommandBus Members

        /// <summary>
        /// Submits the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the t command.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>ICommandResult.</returns>
        /// <exception cref="EFMVC.CommandProcessor.Command.CommandHandlerNotFoundException"></exception>
        public ICommandResult Submit<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = DependencyResolver.Current.GetService<ICommandHandler<TCommand>>();
            if (!((handler != null) && handler is ICommandHandler<TCommand>))
            {
                throw new CommandHandlerNotFoundException(typeof (TCommand));
            }
            return handler.Execute(command);
        }

        /// <summary>
        /// Validates the specified command.
        /// </summary>
        /// <typeparam name="TCommand">The type of the t command.</typeparam>
        /// <param name="command">The command.</param>
        /// <returns>IEnumerable&lt;ValidationResult&gt;.</returns>
        /// <exception cref="EFMVC.CommandProcessor.Command.ValidationHandlerNotFoundException"></exception>
        public IEnumerable<ValidationResult> Validate<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = DependencyResolver.Current.GetService<IValidationHandler<TCommand>>();
            if (!((handler != null) && handler is IValidationHandler<TCommand>))
            {
                throw new ValidationHandlerNotFoundException(typeof (TCommand));
            }
            return handler.Validate(command);
        }

        #endregion
    }
}