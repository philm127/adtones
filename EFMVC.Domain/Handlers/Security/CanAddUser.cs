// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="CanAddUser.cs" company="">
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
    /// Class CanAddUser.
    /// </summary>
    public class CanAddUser : IValidationHandler<UserRegisterCommand>
    {
        /// <summary>
        /// The _profile repository
        /// </summary>
        private readonly IProfileRepository _profileRepository;

        /// <summary>
        /// The user repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CanAddUser" /> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="profileRepository">The profile repository.</param>
        public CanAddUser(IUserRepository userRepository, IProfileRepository profileRepository)
        {
            _userRepository = userRepository;
            _profileRepository = profileRepository;
        }

        #region IValidationHandler<UserRegisterCommand> Members

        /// <summary>
        /// Validates the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>IEnumerable{ValidationResult}.</returns>
        public IEnumerable<ValidationResult> Validate(UserRegisterCommand command)
        {
           
            User isUserExists = null;
            var userexists = _userRepository.Get(c => c.Email == command.Email && c.Activated!=3);
            if(userexists!=null)
            {
                isUserExists = userexists;
            }
            if (isUserExists != null)
            {
                yield return new ValidationResult("EMail", Resources.UserExists);
            }
            if (command.RoleId == 2)
            {
                if (_profileRepository.Get(x => x.MSISDN == command.MSISDN) != null)
                {
                    yield return new ValidationResult("MSISDN", Resources.MsisdnExists);
                }
            }
        }

        #endregion
    }
}