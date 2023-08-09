// ***********************************************************************
// Assembly         : EFMVC.Web
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="SiteHelper.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Configuration;
using System.Linq;
using System.Web;

/// <summary>
/// The Helpers namespace.
/// </summary>

namespace EFMVC.Web.Helpers
{
    /// <summary>
    /// Class SiteHelper.
    /// </summary>
    public class SiteHelper
    {
        /// <summary>
        /// The instance
        /// </summary>
        private static SiteHelper instance;

        /// <summary>
        /// Prevents a default instance of the <see cref="SiteHelper"/> class from being created.
        /// </summary>
        private SiteHelper()
        {
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static SiteHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SiteHelper();
                }

                return instance;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is advertiser URL.
        /// </summary>
        /// <value><c>true</c> if this instance is advertiser URL; otherwise, <c>false</c>.</value>
        public bool IsAdvertiserUrl
        {
            get
            {
                //Return variable declaration
                string appPath = string.Empty;

                //Getting the current context of HTTP request
                HttpContext httpContext = HttpContext.Current;

                //Checking the current context content
                if (httpContext != null)
                {
                    string advertiserUrlHosts = ConfigurationManager.AppSettings["AdvertiserUrlHosts"];
                    string[] hosts = advertiserUrlHosts.Split(",".ToCharArray());

                    if (hosts.Contains(httpContext.Request.Url.Host))
                        return true;
                }

                return false;
            }
        }
    }
}