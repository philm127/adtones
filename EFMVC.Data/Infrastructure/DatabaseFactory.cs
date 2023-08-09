// ***********************************************************************
// Assembly         : EFMVC.Data
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 11-08-2013
// ***********************************************************************
// <copyright file="DatabaseFactory.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

 /// <summary>
/// The Infrastructure namespace.
/// </summary>

namespace EFMVC.Data.Infrastructure
{
    /// <summary>
    /// Class DatabaseFactory.
    /// </summary>
    public class DatabaseFactory : Disposable, IDatabaseFactory
    {
        /// <summary>
        /// The data context
        /// </summary>
        private EFMVCDataContex dataContext;

        #region IDatabaseFactory Members

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>EFMVCDataContex.</returns>
        public EFMVCDataContex Get()
        {
            return dataContext ?? (dataContext = new EFMVCDataContex());
        }

        #endregion

        /// <summary>
        /// Disposes the core.
        /// </summary>
        protected override void DisposeCore()
        {
            if (dataContext != null)
                dataContext.Dispose();
        }
    }
}