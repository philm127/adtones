// ***********************************************************************
// Assembly         : EFMVC.Core
// Author           : Darren Lucraft
// Created          : 10-07-2013
//
// Last Modified By : Darren Lucraft
// Last Modified On : 10-07-2013
// ***********************************************************************
// <copyright file="Md5Encrypt.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// The Common namespace.
/// </summary>

namespace EFMVC.Core.Common
{
    /// <summary>
    /// Class Md5Encrypt.
    /// </summary>
    public static class Md5Encrypt
    {
        /// <summary>
        /// MD5s the encrypt password.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        public static string Md5EncryptPassword(string data)
        {
            var encoding = new ASCIIEncoding();
            byte[] bytes = encoding.GetBytes(data);
            byte[] hashed = MD5.Create().ComputeHash(bytes);
            return Encoding.UTF8.GetString(hashed);
        }
    }
}