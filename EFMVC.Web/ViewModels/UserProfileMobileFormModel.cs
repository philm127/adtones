// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="UserProfileMobileFormModel.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class UserProfileMobileFormModel.
    /// </summary>
    public class UserProfileMobileFormModel
    {
        /// <summary>
        /// Gets or sets the user profile mobile identifier.
        /// </summary>
        /// <value>The user profile mobile identifier.</value>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user profile identifier.
        /// </summary>
        /// <value>The user profile identifier.</value>
        public int UserProfileId { get; set; }

        /// <summary>
        /// Gets or sets the user profile.
        /// </summary>
        /// <value>The user profile.</value>
        public UserProfileFormModel UserProfile { get; set; }

        /// <summary>
        /// Gets or sets the type of the contract.
        /// </summary>
        /// <value>The type of the contract.</value>
        //[Display(Name = "Contract Type")]
        [Display(Name = "Mobile plan")]
        public string ContractType_Mobile { get; set; }

        /// <summary>
        /// Gets or sets the contract type list.
        /// </summary>
        /// <value>The contract type list.</value>
        public IEnumerable<SelectListItem> ContractTypeList { get; set; }

        /// <summary>
        /// Gets or sets the spend.
        /// </summary>
        /// <value>The spend.</value>
        [Display(Name = "Average Monthly Spend")]
        public string Spend_Mobile { get; set; }

        /// <summary>
        /// Gets or sets the spend list.
        /// </summary>
        /// <value>The spend list.</value>
        public IEnumerable<SelectListItem> SpendList { get; set; }

        public string ContractType { get; set; }
        public string Spend { get; set; }

        public bool DisplayContractType { get; set; }
        public bool DisplaySpend { get; set; }

        //Add 27-02-2019
        public int UserId { get; set; }
    }
}