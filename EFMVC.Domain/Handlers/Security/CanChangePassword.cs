// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="CanChangePassword.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using EFMVC.CommandProcessor.Command;
using EFMVC.Core.Common;
using EFMVC.Data.Repositories;
using EFMVC.Domain.Commands;
using EFMVC.Domain.Properties;
using EFMVC.Model;

/// <summary>
/// The Handlers namespace.
/// </summary>

namespace EFMVC.Domain.Handlers
{
    /// <summary>
    /// Class CanChangePassword.
    /// </summary>
    public class CanChangePassword : IValidationHandler<ChangePasswordCommand>
    {
        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanChangePassword"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public CanChangePassword(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        #region IValidationHandler<ChangePasswordCommand> Members

        /// <summary>
        /// Validates the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IEnumerable{ValidationResult}.</returns>
        public IEnumerable<ValidationResult> Validate(ChangePasswordCommand command)
        {
            User user = userRepository.GetById(command.UserId);
            string encoded = Md5Encrypt.Md5EncryptPassword(command.OldPassword);

            if (!user.PasswordHash.Equals(encoded))
            {
                yield return new ValidationResult("OldPassword", Resources.Password);
            }
        }

        #endregion
    }
}