// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Administrator
// Created          : 05-09-2014
//
// Last Modified By : Administrator
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="BucketAuditRowFormModel.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class BucketAuditRowFormModel.
    /// </summary>
    public class BucketAuditRowFormModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the bucket audit identifier.
        /// </summary>
        /// <value>The bucket audit identifier.</value>
        public int BucketAuditId { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>The state.</value>
        public int State { get; set; }

        /// <summary>
        /// Gets or sets the media URL.
        /// </summary>
        /// <value>The media URL.</value>
        public string MediaUrl { get; set; }

        /// <summary>
        /// Gets or sets the bid value.
        /// </summary>
        /// <value>The bid value.</value>
        public string BidValue { get; set; }

        /// <summary>
        /// Gets or sets the DTMF.
        /// </summary>
        /// <value>The DTMF.</value>
        public string Dtmf { get; set; }

        /// <summary>
        /// Gets or sets the start.
        /// </summary>
        /// <value>The start.</value>
        public string Start { get; set; }

        /// <summary>
        /// Gets or sets the end.
        /// </summary>
        /// <value>The end.</value>
        public string End { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile identifier.
        /// </summary>
        /// <value>The campaign profile identifier.</value>
        public int CampaignProfileId { get; set; }

        /// <summary>
        /// Gets or sets the SMS.
        /// </summary>
        /// <value>The SMS.</value>
        public int Sms { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public int Email { get; set; }

        /// <summary>
        /// Gets or sets the bucket audit.
        /// </summary>
        /// <value>The bucket audit.</value>
        public virtual BucketAuditFormModel BucketAudit { get; set; }

        /// <summary>
        /// Gets or sets the campaign profile.
        /// </summary>
        /// <value>The campaign profile.</value>
        public virtual CampaignProfileFormModel CampaignProfile { get; set; }
    }
}