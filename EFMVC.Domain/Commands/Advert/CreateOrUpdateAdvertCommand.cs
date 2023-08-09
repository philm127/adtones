// ***********************************************************************
// Assembly         : EFMVC.Domain
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CreateOrUpdateAdvertCommand.cs" company="Noat">
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
    public class CreateOrUpdateAdvertCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the advert identifier.
        /// </summary>
        /// <value>The advert identifier.</value>
        [Key]
        public int AdvertId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }

        public int? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the name of the advert.
        /// </summary>
        /// <value>The name of the advert.</value>
        public string AdvertName { get; set; }

        /// <summary>
        /// Gets or sets the advert description.
        /// </summary>
        /// <value>The advert description.</value>
        public string AdvertDescription { get; set; }

        /// <summary>
        /// Gets or sets the brand.
        /// </summary>
        /// <value>The brand.</value>
        public string Brand { get; set; }

        /// <summary>
        /// Gets or sets the script.
        /// </summary>
        /// <value>
        /// The script.
        /// </value>
        public string Script { get; set; }

        /// <summary>
        /// Gets or sets the script file location.
        /// </summary>
        /// <value>
        /// The script file location.
        /// </value>
        public string ScriptFileLocation { get; set; }

        /// <summary>
        /// Gets or sets the media file location.
        /// </summary>
        /// <value>The media file location.</value>
        public string MediaFileLocation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [uploaded to media server].
        /// </summary>
        /// <value><c>true</c> if [uploaded to media server]; otherwise, <c>false</c>.</value>
        public bool UploadedToMediaServer { get; set; }

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

        public int Status { get; set; }
        public int? AdvertCategoryId { get; set; }
        public int CountryId { get; set; }
        public int? OperatorId { get; set; }
        //Add 28-02-2019
        public int? CampaignProfileId { get; set; }
    }
}