// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="AdvertFormModel.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EFMVC.Model;

/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class AdvertFormModel.
    /// </summary>
    public class AdminAdvertFormModel
    {
        /// <summary>
        /// Gets or sets the advert identifier.
        /// </summary>
        /// <value>The advert identifier.</value>
        [Key]
        [Display(Name = "ID")]
        public int AdvertId { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int UserId { get; set; }
        //[Required(ErrorMessage = "The Client field is required.")]
        public int? ClientId { get; set; }

        /// <summary>
        /// Gets or sets the name of the advert.
        /// </summary>
        /// <value>The name of the advert.</value>
        [Display(Name = "Name")]
        [Required]
        public string AdvertName { get; set; }

        /// <summary>
        /// Gets or sets the advert description.
        /// </summary>
        /// <value>The advert description.</value>
        [Display(Name = "Description")]
        [Required]
        public string AdvertDescription { get; set; }

        /// <summary>
        /// Gets or sets the brand.
        /// </summary>
        /// <value>The brand.</value>
        [Display(Name = "Brand")]
        [Required]
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
        [Display(Name = "File")]
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
        [Display(Name = "Created Date/Time")]
        public DateTime CreatedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the updated date time.
        /// </summary>
        /// <value>The updated date time.</value>
        [Display(Name = "Updated Date/Time")]
        public DateTime UpdatedDateTime { get; set; }

        public virtual Client Clients { get; set; }

        /// <summary>
        /// Gets or sets the campaign adverts.
        /// </summary>
        /// <value>The campaign adverts.</value>
        public virtual ICollection<CampaignAdvertFormModel> CampaignAdverts { get; set; }

        //[Required]
        public int Status { get; set; }

        public bool IsAdminApproval { get; set; }

        public int? CampaignProfileId { get; set; }

        public int? AdvertCategoryId { get; set; }

        [Required(ErrorMessage = "The Country field is required.")]
        public int CountryId { get; set; }

        //[Range(typeof(bool), "true", "true", ErrorMessage = "Please Accept Terms & Condition.")]
        //public bool IsTermChecked { get; set; }
    }
}