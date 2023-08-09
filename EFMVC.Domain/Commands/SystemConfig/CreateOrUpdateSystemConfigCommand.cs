// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateSystemConfigCommand.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using EFMVC.CommandProcessor.Command;

/// <summary>
/// The Commands namespace.
/// </summary>

namespace EFMVC.Domain.Commands
{
    /// <summary>
    /// Class CreateOrUpdateAdvertCommand.
    /// </summary>
    public class CreateOrUpdateSystemConfigCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the system config identifier.
        /// </summary>
        /// <value>The system config identifier.</value>
        [Key]
        public int SystemConfigId { get; set; }

        /// <summary>
        /// Gets or sets the key of the system config.
        /// </summary>
        /// <value>The key of the system config.</value>
        public string SystemConfigKey { get; set; }
        
        /// <summary>
        /// Gets or sets the value of the system config.
        /// </summary>
        /// <value>The value of the system config.</value>
        public string SystemConfigValue { get; set; }

        /// <summary>
        /// Gets or sets the type of the system config.
        /// </summary>
        /// <value>The type of the system config.</value>
        public string SystemConfigType { get; set; }

        /// <summary>
        /// Gets or sets the created date time.
        /// </summary>
        /// <value>The created date time.</value>
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the updated date time.
        /// </summary>
        /// <value>The updated date time.</value>
        public DateTime UpdatedDateTime { get; set; }
    }
}