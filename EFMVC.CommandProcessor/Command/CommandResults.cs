// ***********************************************************************
// Assembly         : EFMVC.CommandProcessor
// Author           : Darren Lucraft
// Created          : 05-09-2014
//
// Last Modified By : Darren Lucraft
// Last Modified On : 05-09-2014
// ***********************************************************************
// <copyright file="CommandResults.cs" company="Noat">
//     Copyright (c) Noat. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using System.Linq;

/// <summary>
/// The Command namespace.
/// </summary>

namespace EFMVC.CommandProcessor.Command
{
    /// <summary>
    /// Class CommandResults.
    /// </summary>
    public class CommandResults : ICommandResults
    {
        /// <summary>
        /// The results
        /// </summary>
        private readonly List<ICommandResult> results = new List<ICommandResult>();

        #region ICommandResults Members

        /// <summary>
        /// Gets the results.
        /// </summary>
        /// <value>The results.</value>
        public ICommandResult[] Results
        {
            get { return results.ToArray(); }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="CommandResults"/> is success.
        /// </summary>
        /// <value><c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success
        {
            get { return results.All(result => result.Success); }
        }

        #endregion

        /// <summary>
        /// Adds the result.
        /// </summary>
        /// <param name="result">The result.</param>
        public void AddResult(ICommandResult result)
        {
            results.Add(result);
        }
    }
}