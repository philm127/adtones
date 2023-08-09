// ***********************************************************************
// Assembly         : EFMVC.Core
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="ValidationResult.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

 /// <summary>
/// The Common namespace.
/// </summary>

namespace EFMVC.Core.Common
{
    /// <summary>
    /// Describes the result of a validation of a potential change through a business service.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult" /> class.
        /// </summary>
        public ValidationResult()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult" /> class.
        /// </summary>
        /// <param name="memeberName">Name of the memeber.</param>
        /// <param name="message">The message.</param>
        public ValidationResult(string memeberName, string message)
        {
            MemberName = memeberName;
            Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationResult" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ValidationResult(string message)
        {
            Message = message;
        }

        /// <summary>
        /// Gets or sets the name of the member.
        /// </summary>
        /// <value>The name of the member.  May be null for general validation issues.</value>
        public string MemberName { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }
    }
}