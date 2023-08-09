// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateAdvertRejectionCommand.cs" company="Noat">
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
    /// Class CreateOrUpdateAdvertRejectionCommand.
    /// </summary>
    public class CreateOrUpdateAdvertRejectionCommand : ICommand
    {
        public int AdvertRejectionId { get; set; }
        public int? UserId { get; set; }
        public int? AdvertId { get; set; }
        public string RejectionReason { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}