// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateUserProfileMobileCommand.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;

/// <summary>
/// The Commands namespace.
/// </summary>

namespace EFMVC.Domain.Commands
{
    /// <summary>
    /// Class CreateOrUpdateUserProfileMobileCommand.
    /// </summary>
    public class CreateOrUpdateUserProfileMobileCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the user profile mobile identifier.
        /// </summary>
        /// <value>The user profile mobile identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>The user profile identifier.</value>
        public int UserProfileId { get; set; }

        /// <summary>
        /// Gets or sets the type of the contract.
        /// </summary>
        /// <value>The type of the contract.</value>
        public string ContractType_Mobile { get; set; }

        /// <summary>
        /// Gets or sets the spend.
        /// </summary>
        /// <value>The spend.</value>
        public string Spend_Mobile { get; set; }
        public int? CountryId { get; set; }
        public int OperatorId { get; set; }
    }
}