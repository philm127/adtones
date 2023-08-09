// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 10-09-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-09-2013
// ***********************************************************************
// <copyright file="BucketAuditFormModel.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

/// <summary>
/// The ViewModels namespace.
/// </summary>

namespace EFMVC.Web.ViewModels
{
    /// <summary>
    /// Class BucketAuditFormModel.
    /// </summary>
    public class BucketAuditFormModel

    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the bucket identifier.
        /// </summary>
        /// <value>The bucket identifier.</value>
        public int BucketId { get; set; }

        /// <summary>
        /// Gets or sets the msisdn.
        /// </summary>
        /// <value>The msisdn.</value>
        public string MSISDN { get; set; }

        /// <summary>
        /// Gets or sets the bucket period start.
        /// </summary>
        /// <value>The bucket period start.</value>
        public string BucketPeriodStart { get; set; }

        /// <summary>
        /// Gets or sets the target delivery server.
        /// </summary>
        /// <value>The target delivery server.</value>
        public string TargetDeliveryServer { get; set; }

        /// <summary>
        /// Gets or sets the bucket audit rows.
        /// </summary>
        /// <value>The bucket audit rows.</value>
        public virtual ICollection<BucketAuditRowFormModel> BucketAuditRows { get; set; }
    }
}