// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserRegisterCommand.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using EFMVC.CommandProcessor.Command;
using System;

/// <summary>
/// The Commands namespace.
/// </summary>

namespace EFMVC.Domain.Commands
{
    /// <summary>
    /// Class UserRegisterCommand.
    /// </summary>
    public class UserRegisterCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the organisation.
        /// </summary>
        /// <value>
        /// The organisation.
        /// </value>
        public string Organisation { get; set; }

        /// <summary>
        /// Gets or sets the role identifier.
        /// </summary>
        /// <value>The role identifier.</value>
        public int RoleId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UserRegisterCommand"/> is activated.
        /// </summary>
        /// <value><c>true</c> if activated; otherwise, <c>false</c>.</value>
        public int Activated { get; set; }

        /// <summary>
        /// Gets or sets the outstandingdays.
        /// </summary>
        /// <value>
        /// The outstandingdays.
        /// </value>
        public int Outstandingdays { get; set; }

        /// <summary>
        /// Gets or sets the msisdn.
        /// </summary>
        /// <value>The msisdn.</value>
        public string MSISDN { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [verification status].
        /// </summary>
        /// <value><c>true</c> if [verification status]; otherwise, <c>false</c>.</value>
        public bool VerificationStatus { get; set; }

        public int OperatorId { get; set; }

        public bool IsMobileVerfication { get; set; }

        public int? OrganisationTypeId { get; set; }

        public string UserMatchTableName { get; set; }

        public bool IsMsisdnMatch { get; set; }

        public string TibcoMessageId { get; set; }
        public bool IsSessionFlag { get; set; } = false;
        public Nullable<DateTime> LockOutTime { get; set; }
        public DateTime LastPasswordChangedDate { get; set; } = DateTime.Now;
    }
}